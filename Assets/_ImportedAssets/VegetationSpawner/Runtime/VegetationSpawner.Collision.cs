using UnityEngine;

namespace sc.terrain.vegetationspawner
{
    public partial class VegetationSpawner
    {
        public int cellSize = 64;
        public int cellDivisions = 4;
        [System.NonSerialized]
        public static bool VisualizeCells = false;
        public static bool VisualizeWaterlevel = false;
        [Tooltip("When enabled, raycasting is also performed on the corners of each cell. This is slower to calculate, but will yield higher precision around collider edges")]
        public bool highPrecisionCollision = true;
        public LayerMask collisionLayerMask = -1;
        
        public void RebuildCollisionCacheIfNeeded()
        {
            if (terrainCells.Count == 0) RebuildCollisionCache();
        }

        public void RebuildCollisionCache()
        {
            if (tempColliders != null)
            {
                for (int i = 0; i < tempColliders.Length; i++)
                {
                    if (tempColliders[i] == null) continue;
                    tempColliders[i].gameObject.SetActive(true);
                }
            }

            RaycastHit hit;

            terrainCells.Clear();

            foreach (Terrain terrain in terrains)
            {
                if(terrain == null) continue;;
                
                if(terrain.gameObject.activeInHierarchy == false) continue;
                
                int xCount = Mathf.CeilToInt(terrain.terrainData.size.x / cellSize);
                int zCount = Mathf.CeilToInt(terrain.terrainData.size.z / cellSize);

                Cell[,] cellGrid = new Cell[xCount, zCount];

                for (int x = 0; x < xCount; x++)
                {
                    for (int z = 0; z < zCount; z++)
                    {
                        Vector3 wPos = new Vector3(terrain.GetPosition().x + (x * cellSize) + (cellSize * 0.5f), 0f, terrain.GetPosition().z + (z * cellSize) + (cellSize * 0.5f));

                        Vector2 normalizeTerrainPos = terrain.GetNormalizedPosition(wPos);

                        terrain.SampleHeight(normalizeTerrainPos, out _, out wPos.y, out _);

                        Cell cell = Cell.New(wPos, cellSize);
                        cell.Subdivide(cellDivisions);

                        cellGrid[x, z] = cell;

                        for (int sX = 0; sX < cellDivisions; sX++)
                        {
                            for (int sZ = 0; sZ < cellDivisions; sZ++)
                            {
                                //Sample corners of cell
                                if (highPrecisionCollision)
                                {
                                    Bounds b = cell.subCells[sX, sZ].bounds;

                                    Vector3[] corners = new Vector3[]
                                    {
                                        //BL corner
                                        new Vector3(b.min.x, b.center.y, b.min.z),
                                        //TL corner
                                        new Vector3(b.min.x, b.center.y, b.min.z + b.size.z),
                                        //BR corner
                                        new Vector3(b.max.x, b.center.y, b.min.z),
                                        //TR corner
                                        new Vector3(b.max.x, b.center.y, b.max.z),
                                    };

                                    int hitCount = corners.Length;
                                    for (int i = 0; i < corners.Length; i++)
                                    {
                                        if (Physics.Raycast(corners[i] + (Vector3.up * 100f), -Vector3.up, out hit, 150f, collisionLayerMask))
                                        {
                                            //Require to check for type, since its possible to hit a neighboring terrains
                                            if (hit.collider.GetType() == typeof(TerrainCollider))
                                            {
                                                hitCount--;
                                            }
                                        }
                                        else
                                        {
                                            hitCount--;
                                        }
                                    }

                                    //Remove cell when all rays missed
                                    if (hitCount == 0) cell.subCells[sX, sZ] = null;
                                }
                                //Sample center of cell
                                else
                                {
                                    //Remove cell if hitting terrain
                                    if (Physics.Raycast(cell.subCells[sX, sZ].bounds.center + (Vector3.up * 50f), -Vector3.up, out hit, 100f, collisionLayerMask))
                                    {
                                        if (hit.collider.gameObject == terrain.gameObject)
                                        {
                                            cell.subCells[sX, sZ] = null;
                                        }
                                    }
                                }
                            }
                        }
                    }

                }

                terrainCells.Add(terrain, cellGrid);
            }

            if (tempColliders != null)
            {
                for (int i = 0; i < tempColliders.Length; i++)
                {
                    if (tempColliders[i] == null) continue;
                    tempColliders[i].gameObject.SetActive(false);
                }
            }
        }

        public bool InsideOccupiedCell(Terrain terrain, Vector3 worldPos, Vector2 normalizedPos)
        {
            if (terrainCells == null) return false;

            //No collision cells baked for terrain, user will probably notice
            if (terrainCells.ContainsKey(terrain) == false) return false;

            Cell[,] cells = terrainCells[terrain];

            Vector2Int cellIndex = Cell.PositionToCellIndex(terrain, normalizedPos, cellSize);
            Cell mainCell = cells[cellIndex.x, cellIndex.y];

            if (mainCell != null)
            {
                Cell subCell = mainCell.GetSubcell(worldPos, cellSize, cellDivisions);

                if (subCell != null)
                {
                    return true;
                }
                else
                {
                    //Cell doesn't exist
                    return false;
                }
            }
            else
            {
                Debug.LogErrorFormat("Position {0} falls outside of the cell grid", worldPos);
            }

            return false;
        }
    }
}