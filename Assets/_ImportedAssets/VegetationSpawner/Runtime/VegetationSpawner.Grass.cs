using System.Collections.Generic;
using UnityEngine;

namespace sc.terrain.vegetationspawner
{
    public partial class VegetationSpawner
    {
        public void RefreshGrassPrototypes()
        {
            foreach (Terrain terrain in terrains)
            {
                List<DetailPrototype> grassPrototypeCollection = new List<DetailPrototype>();
                int index = 0;
                foreach (GrassPrefab item in grassPrefabs)
                {
                    item.index = index;

                    DetailPrototype detailPrototype = new DetailPrototype();

                    UpdateGrassItem(item, detailPrototype);

                    grassPrototypeCollection.Add(detailPrototype);

                    index++;
                }
                if (grassPrototypeCollection.Count > 0) terrain.terrainData.detailPrototypes = grassPrototypeCollection.ToArray();
            }
        }

        public void SpawnAllGrass(Terrain targetTerrain = null)
        {
            RefreshGrassPrototypes();

            InitializeSeed();
            
            foreach (GrassPrefab item in grassPrefabs)
            {
                SpawnGrass(item, targetTerrain);
            }
        }

        public void UpdateProperties(GrassPrefab item)
        {
            foreach (Terrain terrain in terrains)
            {
                //Note only works when creating these copies :/
                DetailPrototype[] detailPrototypes = terrain.terrainData.detailPrototypes;
                DetailPrototype detailPrototype = GetGrassPrototype(item, terrain);

                //Could have been removed
                if(detailPrototype == null) continue;
                
                UpdateGrassItem(item, detailPrototype);

                detailPrototypes[item.index] = detailPrototype;
                terrain.terrainData.detailPrototypes = detailPrototypes;

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(terrain);
                UnityEditor.EditorUtility.SetDirty(terrain.terrainData);
#endif
            }

        }

        public void SetDetailResolution()
        {
            foreach (Terrain terrain in terrains)
            {
                terrain.terrainData.SetDetailResolution(detailResolution, grassPatchSize);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(terrain.terrainData);
#endif
            }
        }

        public void SpawnGrass(GrassPrefab item, Terrain targetTerrain = null)
        {
            if (item.collisionCheck) RebuildCollisionCacheIfNeeded();

            item.instanceCount = 0;

            if (targetTerrain == null)
            {
                foreach (Terrain terrain in terrains)
                {
                    SpawnGrassOnTerrain(terrain, item);
                }
            }
            else
            {
                SpawnGrassOnTerrain(targetTerrain, item);
            }
            
            onGrassRespawn?.Invoke(item);
        }

        private void SpawnGrassOnTerrain(Terrain terrain, GrassPrefab item)
        {
            //int[,] map = terrain.terrainData.GetDetailLayer(0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, item.index);
            int[,] map = new int[terrain.terrainData.detailWidth, terrain.terrainData.detailHeight];

            int counter = 0;
            int cellCount = terrain.terrainData.detailWidth * terrain.terrainData.detailHeight;
            
            if (item.enabled)
            {
                for (int x = 0; x < terrain.terrainData.detailWidth; x++)
                {
                    for (int y = 0; y < terrain.terrainData.detailHeight; y++)
                    {
                        counter++;
                        
 #if UNITY_EDITOR
                        //Show progress bar every 10%
                        if (counter % (cellCount / 10) == 0)
                        {
                            UnityEditor.EditorUtility.DisplayProgressBar("Vegetation Spawner", "Spawning " + item.name + " on " + terrain.name, (float)counter/(float)cellCount);
                        }
#endif
                        InitializeSeed(x * y + item.seed);

                        //Default
                        int instanceCount = 1;

                        //XZ world position
                        Vector3 wPos = terrain.DetailToWorld(y, x);

                        Vector2 normalizedPos = terrain.GetNormalizedPosition(wPos);

                        //Skip if failing probability check
                        if (((Random.value * 100f) <= item.probability) == false)
                        {
                            instanceCount = 0;
                            continue;
                        }

                        if (item.collisionCheck)
                        {
                            //Check for collision
                            if (InsideOccupiedCell(terrain, wPos, normalizedPos))
                            {
                                instanceCount = 0;
                                continue;
                            }
                        }

                        terrain.SampleHeight(normalizedPos, out _, out wPos.y, out _);

                        if (item.rejectUnderwater && wPos.y < waterHeight)
                        {
                            instanceCount = 0;
                            continue;
                        }
                        //Check height
                        if (wPos.y < item.heightRange.x || wPos.y > item.heightRange.y)
                        {
                            instanceCount = 0;
                            continue;
                        }

                        if (item.slopeRange.x > 0 || item.slopeRange.y < 90)
                        {
                            float slope = terrain.GetSlope(normalizedPos);
                            //Reject if slope check fails
                            if (slope < item.slopeRange.x || slope > item.slopeRange.y)
                            {
                                instanceCount = 0;
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
                                instanceCount = 0;
                                continue;
                            }
                        }

                        //Reject based on layer masks
                        float spawnChance = 0f;
                        if (item.layerMasks.Count == 0)
                        {
                            spawnChance = 100f;
                        }
                        else
                        {
                            splatmapTexelIndex = terrain.SplatmapTexelIndex(normalizedPos);
                        }

                        foreach (TerrainLayerMask layer in item.layerMasks)
                        {
                            Texture2D splat = terrain.terrainData.GetAlphamapTexture(GetSplatmapID(layer.layerID));

                            m_splatmapColor = splat.GetPixel(splatmapTexelIndex.x, splatmapTexelIndex.y);

                            int channel = layer.layerID % 4;
                            float value = SampleChannel(m_splatmapColor, channel);

                            if (value > 0)
                            {
                                value = Mathf.Clamp01(value - layer.threshold);
                            }
                            value *= 100f;

                            spawnChance += value;
                        }
                        InitializeSeed(x * y + item.seed);
                        if ((Random.value <= spawnChance) == false)
                        {
                            instanceCount = 0;
                        }

                        //if (instanceCount == 1) DebugPoints.Instance.Add(wPos, true, 0f);
                        item.instanceCount += instanceCount;
                        //Passed all conditions, spawn one instance here
                        map[x, y] = instanceCount;
                    }
                }
            }
            
            terrain.terrainData.SetDetailLayer(0, 0, item.index, map);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.ClearProgressBar();
#endif
        }
        
        private DetailPrototype GetGrassPrototype(GrassPrefab item, Terrain terrain)
        {
            if (item.index >= terrain.terrainData.detailPrototypes.Length) return null;
            
            return terrain.terrainData.detailPrototypes[item.index];
        }

        private void UpdateGrassItem(GrassPrefab item, DetailPrototype d)
        {
            d.healthyColor = item.mainColor;
            d.dryColor = item.linkColors ? item.mainColor : item.secondaryColor;

            d.minHeight = item.minMaxHeight.x;
            d.maxHeight = item.minMaxHeight.y;

            d.minWidth = item.minMaxWidth.x;
            d.maxWidth = item.minMaxWidth.y;

            d.noiseSpread = item.noiseSize;

            if (item.type == GrassType.Mesh)
            {
                d.renderMode = DetailRenderMode.Grass; //Actually a mesh
                d.usePrototypeMesh = true;
                d.prototype = item.prefab;
                d.prototypeTexture = null;

            }
            if (item.type == GrassType.Texture && item.billboard)
            {
                d.renderMode = item.renderAsBillboard ? DetailRenderMode.GrassBillboard : DetailRenderMode.Grass;
                d.usePrototypeMesh = false;
                d.prototypeTexture = item.billboard;
                d.prototype = null;
            }
        }
    }
}