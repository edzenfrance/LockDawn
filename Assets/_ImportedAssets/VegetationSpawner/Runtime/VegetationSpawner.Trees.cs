using System.Collections.Generic;
using UnityEngine;

namespace sc.terrain.vegetationspawner
{
    public partial class VegetationSpawner
    {
        [Tooltip("Tree item will automatically respawn after changing a parameter in the inspector")]
        public bool autoRespawnTrees = true;
        
        private void SpawnAllTrees(Terrain terrain = null)
        {
            if (treeTypes == null) return;

            if (treeTypes.Count == 0) return;

            InitializeSeed();

            RefreshTreePrefabs();

            int index = 0;
            foreach (TreeType item in treeTypes)
            {
#if UNITY_EDITOR
                UnityEditor.EditorUtility.DisplayProgressBar("Vegetation Spawner", "Spawning trees...", (float)index / (float)treeTypes.Count);
#endif
                SpawnTree(item, terrain);

                index++;
            }
#if UNITY_EDITOR
            UnityEditor.EditorUtility.ClearProgressBar();
#endif

        }

        public void RefreshTreePrefabs()
        {
            List<TreePrototype> treePrototypeCollection = new List<TreePrototype>();

            foreach (TreeType item in treeTypes)
            {
                foreach (TreePrefab p in item.prefabs)
                {
                    if (p.prefab == null) continue;

                    TreePrototype treePrototype = new TreePrototype();
                    treePrototype.prefab = p.prefab;
                    treePrototypeCollection.Add(treePrototype);
                    p.index = treePrototypeCollection.Count - 1;
                }
            }
            foreach (Terrain terrain in terrains)
            {
                terrain.terrainData.treePrototypes = treePrototypeCollection.ToArray();

                //Ensures prototypes are persistent
                terrain.terrainData.RefreshPrototypes();
            }
        }

        public void SpawnTree(TreeType item, Terrain targetTerrain = null)
        {
            if (item.collisionCheck) RebuildCollisionCacheIfNeeded();

            item.instanceCount = 0;
            RefreshTreePrefabs();

            if (targetTerrain == null)
            {
                foreach (Terrain terrain in terrains)
                {
                    SpawnTreeOnTerrain(terrain, item);
                }
            }
            else
            {
                SpawnTreeOnTerrain(targetTerrain, item);
            }
            
            for (int i = 0; i < item.prefabs.Count; i++)
            {
                onTreeRespawn?.Invoke(item.prefabs[i]);
            }
        }

        private void SpawnTreeOnTerrain(Terrain terrain, TreeType item)
        {
            float height, worldHeight, normalizedHeight;
            
            List<TreeInstance> treeInstanceCollection = new List<TreeInstance>(terrain.terrainData.treeInstances);

            //Clear all existing instances first, setting the tree instances is additive
            for (int i = 0; i < treeInstanceCollection.Count; i++)
            {
                foreach (TreePrefab prefab in item.prefabs)
                {
                    treeInstanceCollection.RemoveAll(x => x.prototypeIndex == prefab.index);
                }
            }

            if (item.enabled)
            {
                InitializeSeed(item.seed);
                
                item.spawnPoints = PoissonDisc.GetSpawnpoints(terrain, item.distance, item.seed + seed);

                foreach (Vector3 pos in item.spawnPoints)
                {
                    //InitializeSeed(item.seed + index);

                    //Relative position as 0-1 value
                    Vector2 normalizedPos = terrain.GetNormalizedPosition(pos);

                    InitializeSeed(item.seed + (int)pos.x * (int)pos.z);

                    //Skip if failing global probability check
                    if (((Random.value * 100f) <= item.probability) == false)
                    {
                        continue;
                    }

                    if (item.collisionCheck)
                    {
                        //Check for collision
                        if (InsideOccupiedCell(terrain, pos, normalizedPos))
                        {
                            continue;
                        }
                    }

                    TreePrefab prefab = SpawnerBase.GetProbableTree(item);

                    //Failed probability checks entirely
                    if (prefab == null) continue;

                    terrain.SampleHeight(normalizedPos, out height, out worldHeight, out normalizedHeight);

                    //Reject if lower than chosen water level
                    if (item.rejectUnderwater && worldHeight < waterHeight) continue;

                    //Check height
                    if (worldHeight < item.heightRange.x || worldHeight > item.heightRange.y)
                    {
                        continue;
                    }

                    if (item.slopeRange.x > 0 || item.slopeRange.y < 90f)
                    {
                        float slope = terrain.GetSlope(normalizedPos, false);

                        //Reject if slope check fails
                        if (!(slope >= (item.slopeRange.x) && slope <= (item.slopeRange.y)))
                        {
                            continue;
                        }
                    }

                    if (item.curvatureRange.x > 0 || item.curvatureRange.y < 1f)
                    {
                        float curvature = terrain.SampleConvexity(normalizedPos);
                        //0=concave, 0.5=flat, 1=convex
                        curvature = TerrainSampler.ConvexityToCurvature(curvature);
                        if (curvature < item.curvatureRange.x || curvature > item.curvatureRange.y)
                        {
                            continue;
                        }
                    }

                    float spawnChance = 0f;
                    if (item.layerMasks.Count == 0)
                    {
                        spawnChance = 100f;
                    }
                    else
                    {
                        //Reject based on layer masks
                        splatmapTexelIndex = terrain.SplatmapTexelIndex(normalizedPos);
                    }

                    foreach (TerrainLayerMask layer in item.layerMasks)
                    {
                        Texture2D splat = terrain.terrainData.GetAlphamapTexture(GetSplatmapID(layer.layerID));

                        Color color = splat.GetPixel(splatmapTexelIndex.x, splatmapTexelIndex.y);

                        int channel = layer.layerID % 4;
                        float value = SampleChannel(color, channel);

                        if (value > 0)
                        {
                            value = Mathf.Clamp01(value - layer.threshold);
                        }
                        value *= 100f;

                        spawnChance += value;

                    }
                    InitializeSeed((int)pos.x * (int)pos.z);
                    if ((Random.value <= spawnChance) == false)
                    {
                        continue;
                    }

                    //Passed all conditions, add instance
                    TreeInstance treeInstance = new TreeInstance();
                    treeInstance.prototypeIndex = prefab.index;

                    //Note: Sink amount should be converted to normalized 0-1 height
                    treeInstance.position = new Vector3(normalizedPos.x, normalizedHeight - (item.sinkAmount / (terrain.terrainData.size.y + 0.01f)), normalizedPos.y);
                    treeInstance.rotation = Random.Range(0f, 359f) * Mathf.Deg2Rad;

                    float scale = Random.Range(item.scaleRange.x, item.scaleRange.y);
                    treeInstance.heightScale = scale;
                    treeInstance.widthScale = scale;

                    treeInstance.color = Color.white;
                    treeInstance.lightmapColor = Color.white;

                    treeInstanceCollection.Add(treeInstance);

                    item.instanceCount++;
                }
            }

#if UNITY_2019_1_OR_NEWER
            terrain.terrainData.SetTreeInstances(treeInstanceCollection.ToArray(), false);
#else
            terrain.terrainData.treeInstances = treeInstanceCollection.ToArray();
#endif
        }

        private TreePrototype GetTreePrototype(TreePrefab item, Terrain terrain)
        {
            return terrain.terrainData.treePrototypes[item.index];
        }

        public void UpdateTreeItem(TreeType item)
        {
            foreach (Terrain terrain in terrains)
            {
                foreach (TreePrefab p in item.prefabs)
                {
                    //Not yet added
                    if (p.index >= terrain.terrainData.treePrototypes.Length) continue;

                    if (p.prefab == null) continue;

                    if (terrain.terrainData.treePrototypes[p.index] == null) continue;

                    //Note only works when creating these copies :/
                    TreePrototype[] treePrototypes = terrain.terrainData.treePrototypes;

                    TreePrototype t = new TreePrototype();

                    t.prefab = p.prefab;

                    treePrototypes[p.index] = t;
                    terrain.terrainData.treePrototypes = treePrototypes;
                }

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(terrain);
                UnityEditor.EditorUtility.SetDirty(terrain.terrainData);
#endif
            }
        }
    }
}