// Vegetation Spawner by Staggart Creations http://staggart.xyz
// Copyright protected under Unity Asset Store EULA

using System;
using System.Collections.Generic;
using UnityEngine;

namespace sc.terrain.vegetationspawner
{
    [ExecuteInEditMode]
    [AddComponentMenu("Vegetation Spawner")]
    public partial class VegetationSpawner : SpawnerBase
    {
        public const string Version = "1.0.6";
        public static VegetationSpawner Current;
        
        //Cannot be serialized, but is rebuilt when required
        public Dictionary<Terrain, Cell[,]> terrainCells = new Dictionary<Terrain, Cell[,]>();
        //This is used for the height range slider
        public Vector2 terrainMinMaxHeight = new Vector2(0, 2000f);
        public int detailResolution = 256;
        public int detailResolutionIndex;
        public int grassPatchSize = 128;
        public int grassPatchSizeIndex = 4;

        [Tooltip("Assign any collider objects that should temporarily be enabled when collision cache is rebuilt")]
        public Collider[] tempColliders;

        [Tooltip("Determines the height of a virtual water level. If a vegetation item has \"Remove underwater\" enabled, it will not spawn under this height")]
        public float waterHeight;
        
        public delegate void OnTreeRespawn(TreePrefab prefab);
        /// <summary>
        /// Triggers whenever a tree species is respawned. Passes the related SpawnerBase.TreePrefab as an argument.
        /// </summary>
        public static event OnTreeRespawn onTreeRespawn;
        
        public delegate void OnGrassRespawn(GrassPrefab prefab);
        /// <summary>
        /// Triggers whenever a grass item is respawned. Passes the related SpawnerBase.GrassPrefab as an argument.
        /// </summary>
        public static event OnGrassRespawn onGrassRespawn;
        
        private static Vector2Int splatmapTexelIndex;
        private static Color m_splatmapColor;

        private void OnEnable()
        {
            Current = this;
        }

        private void OnDisable()
        {
            Current = null;
        }
        
        /// <summary>
        /// Respawns all vegetation on the assigned terrains
        /// </summary>
        /// <param name="grass">Enable respawning of grass</param>
        /// <param name="trees">Enable respawning of trees</param>
        public void Respawn(bool grass = true, bool trees = true)
        {
            if (terrains == null) return;

            if(grass) SpawnAllGrass();
            if(trees) SpawnAllTrees();

            foreach (Terrain terrain in terrains)
            {
                if (!terrain) continue;

                terrain.Flush();

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(terrain.terrainData);
                UnityEditor.EditorUtility.SetDirty(terrain);
                UnityEditor.AssetDatabase.SaveAssets();
#endif
            }
        }
        
        /// <summary>
        /// Respawns all vegetation on a specific terrain
        /// </summary>
        /// <param name="grass">Enable respawning of grass</param>
        /// <param name="trees">Enable respawning of trees</param>
        public void RespawnTerrain(Terrain terrain, bool grass = true, bool trees = true)
        {
            if (grass) SpawnAllGrass(terrain);
            if (trees) SpawnAllTrees(terrain);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            /*
            foreach (Terrain terrain in terrains)
            {
                int cellSize = Mathf.CeilToInt(terrains[0].terrainData.size.x / detailResolution);
                for (int x = 0; x < detailResolution; x++)
                {
                    for (int y = 0; y < detailResolution; y++)
                    {
                        Vector3 cellPos = new Vector3((x * cellSize) + (cellSize / 2), 0f, (y * cellSize) + (cellSize / 2));

                        Vector2 normalizedPos = terrain.GetNormalizedPosition(cellPos);
                        float worldHeight = 0f;
                        terrain.SampleHeight(normalizedPos, out _, out worldHeight, out _);
                        
                        cellPos.y = worldHeight;
                        
                        if ((UnityEditor.SceneView.lastActiveSceneView.camera.transform.position - cellPos).magnitude > 25f) continue;

                        UnityEditor.Handles.DrawWireCube(cellPos, new Vector3(cellSize, 0f, cellSize));
                        //Gizmos.DrawWireCube();
                    }
                }
            }
            */


            if (VisualizeCells)
            {
                if (terrainCells == null) return;

                foreach (KeyValuePair<Terrain, Cell[,]> item in terrainCells)
                {
                    foreach (Cell cell in item.Value)
                    {
                        if ((UnityEditor.SceneView.lastActiveSceneView.camera.transform.position - cell.bounds.center).magnitude > 150f) continue;

                        foreach (Cell subCell in cell.subCells)
                        {
                            if (subCell == null) continue;
                            Gizmos.color = new Color(1f, 0.05f, 0.05f, 1f);
                            Gizmos.DrawWireCube(new Vector3(subCell.bounds.center.x, subCell.bounds.center.y, subCell.bounds.center.z),
                                new Vector3(subCell.bounds.size.x, subCell.bounds.size.y, subCell.bounds.size.z));
                        }

                        Gizmos.color = new Color(0.66f, 0.66f, 1f, 0.25f);
                        Gizmos.DrawWireCube(
                            new Vector3(cell.bounds.center.x, cell.bounds.center.y, cell.bounds.center.z),
                            new Vector3(cell.bounds.size.x, cell.bounds.size.y, cell.bounds.size.z)
                            );
                    }
                }
            }

            if (VisualizeWaterlevel)
            {
                Gizmos.color = new Color(0f, 0.8f, 1f, 0.75f);
                
                Gizmos.DrawCube(new Vector3(UnityEditor.SceneView.lastActiveSceneView.camera.transform.position.x, waterHeight, UnityEditor.SceneView.lastActiveSceneView.camera.transform.position.z), new Vector3(250f, 0f, 250f) );
            }
        }
#endif
    }
}