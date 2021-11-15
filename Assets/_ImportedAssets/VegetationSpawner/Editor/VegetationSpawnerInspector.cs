// Vegetation Spawner by Staggart Creations http://staggart.xyz
// Copyright protected under Unity Asset Store EULA

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using static sc.terrain.vegetationspawner.SpawnerBase;
using Random = UnityEngine.Random;
using System.Diagnostics;
using static sc.terrain.vegetationspawner.VegetationSpawnerEditor;
using Debug = UnityEngine.Debug;

namespace sc.terrain.vegetationspawner
{
    [CustomEditor(typeof(VegetationSpawner))]
    public class VegetationSpawnerInspector : Editor
    {
        VegetationSpawner spawner;
        
        SerializedProperty seed;
        SerializedProperty terrains;
        SerializedProperty terrainSettings;
        private SerializedProperty detailResolutionIndex;
        private SerializedProperty detailResolution;
        private SerializedProperty grassPatchSize;
        private SerializedProperty grassPatchSizeIndex;
        SerializedProperty cellSize;
        SerializedProperty cellDivisions;
        SerializedProperty collisionLayerMask;
        SerializedProperty highPrecisionCollision;
        SerializedProperty tempColliders;
        SerializedProperty waterHeight;
        private SerializedProperty autoRespawnTrees;
        
        private Stopwatch sw;
        
        private static string[] detailResolutions = new []{ "128px", "256px", "512px", "1024px", "2048px" };
        private static string[] patchSizes = new []{ "8x8","16x16","32x32", "64x64", "128x128"};
        
        private bool hasMissingTerrains;
           
        int newTreeprefabPickerWindowID = -1;
        int prefabPickerWindowID = -1;
        
        private void OnEnable()
        {
            spawner = (VegetationSpawner)target;

            if(spawner.terrains != null) hasMissingTerrains = SpawnerBase.HasMissingTerrain(spawner.terrains);
            
            seed = serializedObject.FindProperty("seed");
            terrains = serializedObject.FindProperty("terrains");
            detailResolutionIndex = serializedObject.FindProperty("detailResolutionIndex");
            detailResolution = serializedObject.FindProperty("detailResolution");
            grassPatchSize = serializedObject.FindProperty("grassPatchSize");
            grassPatchSizeIndex = serializedObject.FindProperty("grassPatchSizeIndex");
            terrainSettings = serializedObject.FindProperty("terrainSettings");
            cellSize = serializedObject.FindProperty("cellSize");
            cellDivisions = serializedObject.FindProperty("cellDivisions");
            collisionLayerMask = serializedObject.FindProperty("collisionLayerMask");
            highPrecisionCollision = serializedObject.FindProperty("highPrecisionCollision");
            tempColliders = serializedObject.FindProperty("tempColliders");

            waterHeight = serializedObject.FindProperty("waterHeight");
            autoRespawnTrees = serializedObject.FindProperty("autoRespawnTrees");

            VegetationSpawner.VisualizeCells = VisualizeCellsPersistent;

            Undo.undoRedoPerformed += OnUndoRedo;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= OnUndoRedo;
            
            VegetationSpawner.VisualizeCells = false;
            VegetationSpawner.VisualizeWaterlevel = false;
        }
        
        private int selectedLayerID;
        private Vector2 treeScrollPos;
        private Vector2 grassScrollPos;
        private Editor settingEditor;
        private Vector2 texScrollview;
        private float previewSize;
        private Texture2D previewTex;

        private static int TabID
        {
            get { return SessionState.GetInt("VegetationSpawnerInspector_TAB", 0); }
            set { SessionState.SetInt("VegetationSpawnerInspector_TAB", value); }
        }

        private static int selectedGrassID
        {
            get { return SessionState.GetInt("VegetationSpawnerInspector_selectedGrassID", 0); }
            set { SessionState.SetInt("VegetationSpawnerInspector_selectedGrassID", value); }
        }

        private static int selectedTreeID
        {
            get { return SessionState.GetInt("VegetationSpawnerInspector_selectedTreeID", 0); }
            set { SessionState.SetInt("VegetationSpawnerInspector_selectedTreeID", value); }
        }

        private static bool VisualizeCellsPersistent
        {
            get { return SessionState.GetBool("VegetationSpawnerInspector_VisualizeCells", false); }
            set { SessionState.SetBool("VegetationSpawnerInspector_VisualizeCells", value); }
        }

        private static bool ShowLog
        {
            get { return SessionState.GetBool("VegetationSpawnerInspector_ShowLog", false); }
            set { SessionState.SetBool("VegetationSpawnerInspector_ShowLog", value); }
        }
        private Vector2 logScrollPos;
        
        public override void OnInspectorGUI()
        {
            Rect versionRect = EditorGUILayout.GetControlRect();
            versionRect.y -= versionRect.height + 6f;
            versionRect.x = 240f;
            GUI.Label(versionRect, "Version " + VegetationSpawner.Version, EditorStyles.miniLabel);
            GUILayout.Space(-17f);

            EditorGUILayout.Space();
            
            if(hasMissingTerrains) EditorGUILayout.HelpBox("One or more terrains are missing", MessageType.Error);

            if (spawner.terrains.Count > 0)
            {
                TabID = GUILayout.Toolbar(TabID, new GUIContent[] {
                new GUIContent("  Terrains", TerrainIcon),
                new GUIContent("  Trees", TreeIcon),
                new GUIContent("  Grass", DetailIcon),
                new GUIContent("  Settings", EditorGUIUtility.IconContent("GameManager Icon").image)
                }, GUILayout.MaxHeight(30f));

                EditorGUILayout.Space();
                
                if (spawner.terrainSettings.drawTreesAndFoliage == false)
                {
                    EditorGUILayout.HelpBox("Vegetation rendering is disabled in the Terrain tab!", MessageType.Error);
                
                    GUILayout.Space(-32);
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button(new GUIContent("Enable"), GUILayout.Width(60)))
                        {
                            spawner.terrainSettings.drawTreesAndFoliage = true;
                            spawner.CopySettingsToTerrains();
                            EditorUtility.SetDirty(target);
                        }
                        GUILayout.Space(8);
                    }
                    GUILayout.Space(11);
                }

                serializedObject.Update();
                EditorGUI.BeginChangeCheck();
                
                switch (TabID)
                {
                    case 0:
                        DrawTerrain();
                        break;
                    case 1:
                        DrawTrees();
                        break;
                    case 2:
                        DrawGrass();
                        break;
                    case 3:
                        DrawSettings();
                        break;
                }

                VegetationSpawner.VisualizeCells = TabID == 3;
                VegetationSpawner.VisualizeWaterlevel = TabID == 3;
                
                EditorGUILayout.Space();
            }
            else
            {
                EditorGUILayout.HelpBox("Assign terrains to spawn on", MessageType.Info);
                DrawTerrain();
                
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Vegetation already placed on the terrain will be cleared!", MessageType.Warning);
                return;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                ShowLog = GUILayout.Toggle(ShowLog, "▼", EditorStyles.miniButtonMid, GUILayout.Width(30f));
                EditorGUILayout.LabelField("Log", EditorStyles.boldLabel, GUILayout.Width(35f));
            }

            if (ShowLog)
            {
                using (new EditorGUILayout.VerticalScope(EditorStyles.textArea, UnityEngine.GUILayout.MaxHeight(100f)))
                {
                    logScrollPos = EditorGUILayout.BeginScrollView(logScrollPos);

                    for (int i = VegetationSpawnerEditor.Log.items.Count - 1; i >= 0; i--)
                    {
                        EditorGUILayout.LabelField(VegetationSpawnerEditor.Log.items[i], EditorStyles.miniLabel);
                    }

                    logScrollPos.y += 10f;

                    EditorGUILayout.EndScrollView();
                }
            }
            
            EditorGUILayout.LabelField("- Staggart Creations -", EditorStyles.centeredGreyMiniLabel);

        }

        private void DrawTerrain()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(terrains, true);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.BeginChangeCheck();

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Add active terrains"))
                {
                    Terrain[] activeTerrains = Terrain.activeTerrains;

                    //Remove all null items
                    spawner.terrains.RemoveAll(x => x == null);
                    
                    for (int i = 0; i < activeTerrains.Length; i++)
                    {
                        if (spawner.terrains.Contains(activeTerrains[i]) == false) spawner.terrains.Add(activeTerrains[i]);
                    }

                    for (int i = 0; i < spawner.terrains.Count; i++)
                    {
                        spawner.terrainMinMaxHeight.x = Mathf.Min(spawner.terrainMinMaxHeight.x, spawner.terrains[i].terrainData.bounds.min.y);
                        spawner.terrainMinMaxHeight.y = Mathf.Max(spawner.terrainMinMaxHeight.y, spawner.terrains[i].terrainData.bounds.size.y);
                    }

                    hasMissingTerrains = false;
                    
                    spawner.RebuildCollisionCache();
                    EditorUtility.SetDirty(target);
                }

                if (GUILayout.Button("Clear"))
                {
                    hasMissingTerrains = false;

                    spawner.terrains.Clear();
                    EditorUtility.SetDirty(target);
                }
            }

            if (spawner.terrains.Count == 0) return;

            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            terrainSettings.isExpanded = true;

            EditorGUILayout.PropertyField(terrainSettings, true);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);

                serializedObject.ApplyModifiedProperties();
                spawner.CopySettingsToTerrains();
            }
        }

        private TreeType GetTreeType(int index)
        {
            if (index < 0 || spawner.treeTypes == null) return null;
            
            return spawner.treeTypes.Count > 0 ? spawner.treeTypes[selectedTreeID] : null;
        }

        private void DrawTrees()
        {
            EditorGUILayout.LabelField("Species", EditorStyles.boldLabel);

            selectedTreeID = Mathf.Min(selectedTreeID, spawner.treeTypes.Count);
            
            PrefabPickingActions();
            
            //Tree item view
            treeScrollPos = EditorGUILayout.BeginScrollView(treeScrollPos, EditorStyles.helpBox, GUILayout.MaxHeight(thumbSize + 10f));
            using (new EditorGUILayout.HorizontalScope())
            {
                for (int i = 0; i < spawner.treeTypes.Count; i++)
                {
                    if (spawner.treeTypes[i] == null) continue;

                    Texture2D thumb = EditorGUIUtility.IconContent("d_BuildSettings.Broadcom").image as Texture2D;

                    if (spawner.treeTypes[i].prefabs.Count > 0)
                    {
                        if (spawner.treeTypes[i].prefabs[0] != null)
                        {
                            if (spawner.treeTypes[i].prefabs[0].prefab) thumb = AssetPreview.GetAssetPreview(spawner.treeTypes[i].prefabs[0].prefab);
                        }
                    }

                    if (GUILayout.Button(new GUIContent(spawner.treeTypes[i].name, thumb), (selectedTreeID == i) ? VegetationSpawnerEditor.PreviewTexSelected : VegetationSpawnerEditor.PreviewTex, GUILayout.MaxHeight(thumbSize), GUILayout.Width(thumbSize)))
                    {
                        selectedTreeID = i;
                    }
                }
            }

            EditorGUILayout.EndScrollView();

            TreeType tree = GetTreeType(selectedTreeID);

            Undo.RecordObject(spawner, "Modified tree species");

            serializedObject.Update();
            using (var treeChange = new EditorGUI.ChangeCheckScope())
            {
                //Tree type view options
                using (new EditorGUILayout.HorizontalScope())
                {
                    if(tree != null) EditorGUILayout.LabelField("Instances: " + spawner.treeTypes[selectedTreeID].instanceCount.ToString("##,#"), EditorStyles.miniLabel);
                    
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button(new GUIContent("Add", EditorGUIUtility.IconContent(EditorGUIUtility.isProSkin ? "d_Toolbar Plus" : "Toolbar Plus").image, "Add new item"), EditorStyles.miniButtonLeft, GUILayout.Width(60f)))
                    {
                        newTreeprefabPickerWindowID = EditorGUIUtility.GetControlID(FocusType.Passive) + 200; 
                        EditorGUIUtility.ShowObjectPicker<GameObject>(null, false, "", newTreeprefabPickerWindowID);
                    }
                    if (spawner.treeTypes.Count > 0)
                    {
                        if (GUILayout.Button(new GUIContent("", EditorGUIUtility.IconContent("d_TreeEditor.Trash").image, "Remove"), EditorStyles.miniButtonRight))
                        {
                            spawner.treeTypes.RemoveAt(selectedTreeID);
                            selectedTreeID = spawner.treeTypes.Count - 1;

                            if (selectedTreeID < 0) selectedTreeID = 0;

                            spawner.RefreshTreePrefabs();
                        }
                    }
                }

                //Settings for selected

                if (tree != null)
                {
                    EditorGUI.BeginChangeCheck();
                    tree.enabled = EditorGUILayout.Toggle("Enabled", tree.enabled);
                    if (EditorGUI.EndChangeCheck() && autoRespawnTrees.boolValue)
                    {
                        spawner.SpawnTree(tree);
                    }
                    
                    tree.name = EditorGUILayout.TextField("Name", tree.name);
                    
                    EditorGUILayout.Space();
                    
                    EditorGUILayout.LabelField("Prefabs", EditorStyles.boldLabel);
                    if (tree.prefabs.Count == 0) EditorGUILayout.HelpBox("Add a tree prefab first", MessageType.Info);

                    for (int i = 0; i < tree.prefabs.Count; i++)
                    {
                        SpawnerBase.TreePrefab item = tree.prefabs[i];

                        using (new EditorGUILayout.HorizontalScope())
                        {
                            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                            {
                                EditorGUI.BeginChangeCheck();
                                item.prefab = EditorGUILayout.ObjectField("Prefab", item.prefab, typeof(GameObject), true) as GameObject;

                                if (item.prefab)
                                {
                                    //Update from 1.0.4 to 1.0.5
                                    if (tree.name == "VegetationItem") tree.name = item.prefab.name;
                                        
                                    if (EditorUtility.IsPersistent(item.prefab) == false) EditorGUILayout.HelpBox("Prefab cannot be a scene instance", MessageType.Error);
                                }
                                item.probability = EditorGUILayout.Slider("Spawn chance %", item.probability, 0f, 100f);
                                if (EditorGUI.EndChangeCheck())
                                {
                                    spawner.UpdateTreeItem(tree);
                                    if(autoRespawnTrees.boolValue) spawner.SpawnTree(tree);
                                    EditorUtility.SetDirty(target);
                                }
                            }
                            if (GUILayout.Button(new GUIContent("", TrashIcon, "Remove"), EditorStyles.miniButtonMid))
                            {
                                tree.prefabs.RemoveAt(i);

                                spawner.RefreshTreePrefabs();
                            }
                        }

                        if (item.prefab)
                        {
                            LODGroup lodGroup = item.prefab.GetComponent<LODGroup>();
                            
                            if(!lodGroup) EditorGUILayout.HelpBox("Prefab does not have a LOD Group component, random rotation and scale will not work", MessageType.Warning);
                        }
                    }

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button(new GUIContent("Add", EditorGUIUtility.IconContent(iconPrefix + "Toolbar Plus").image, "Add new item"), EditorStyles.miniButton, GUILayout.Width(60f)))
                        {
                            prefabPickerWindowID = EditorGUIUtility.GetControlID(FocusType.Passive) + 201; 
                            EditorGUIUtility.ShowObjectPicker<GameObject>(null, false, "", prefabPickerWindowID);
                        }
                    }

                    if (tree.prefabs.Count > 0)
                    {
                        if (tree.prefabs[0].prefab != null)
                        {
                            EditorGUILayout.LabelField("Spawn rules", EditorStyles.boldLabel);

                            EditorGUI.BeginChangeCheck();

                            DrawSeedField(ref tree.seed);

                            tree.probability = EditorGUILayout.Slider("Global spawn chance %", tree.probability, 0f, 100f);
                            tree.distance = EditorGUILayout.Slider("Distance between", tree.distance, 0.5f, 50f);
                            VegetationSpawnerEditor.DrawRangeSlider(new GUIContent("Scale", "Scale is randomly selected from this range"), ref tree.scaleRange, 0f, 2f);
                            tree.sinkAmount = EditorGUILayout.Slider(new GUIContent("Sink amount", "Lowers the Y position of the tree"), tree.sinkAmount, 0f, 1f);

                            EditorGUILayout.Space();
                            tree.collisionCheck = EditorGUILayout.Toggle(new GUIContent("Collision check", "Avoid spawning inside colliders, see the settings tab for configuration details"), tree.collisionCheck);
                            tree.rejectUnderwater = EditorGUILayout.Toggle(new GUIContent("Remove underwater", "The water height level can be set in the settings tab"), tree.rejectUnderwater);
                            
                            EditorGUILayout.Space();

                            VegetationSpawnerEditor.DrawRangeSlider(new GUIContent("Height range", "Min/max height this item can spawn at"), ref tree.heightRange, spawner.terrainMinMaxHeight.x, spawner.terrainMinMaxHeight.y);
                            VegetationSpawnerEditor.DrawRangeSlider(new GUIContent("Slope range", "Min/max slope (0-90 degrees) this item can spawn at"), ref tree.slopeRange, 0f, 90f);
                            VegetationSpawnerEditor.DrawRangeSlider(new GUIContent("Curvature range", "0=Concave (bowl), 0.5=flat, 1=convex (edge)"), ref tree.curvatureRange, 0f, 1f);
                           
                            LayerMaskSettings(tree.layerMasks);
                            
                            if (EditorGUI.EndChangeCheck())
                            {
                                if (!autoRespawnTrees.boolValue) return;
                                
                                Stopwatch sw = new Stopwatch();
                                sw.Restart();
                                 spawner.SpawnTree(tree);
                                sw.Stop();

                                VegetationSpawnerEditor.Log.Add("Respawning tree: " + sw.Elapsed.Milliseconds + "ms...");

                                EditorUtility.SetDirty(target);
                            }

                            if(autoRespawnTrees.boolValue == false) EditorGUILayout.HelpBox("Auto respawning is disabled for trees in the settings tab", MessageType.Info);

                            if (autoRespawnTrees.boolValue == false)
                            {
                                if (GUILayout.Button(
                                    new GUIContent(" Respawn", EditorGUIUtility.IconContent("d_Refresh").image),
                                    GUILayout.MaxHeight(30f)))
                                {
                                    Stopwatch sw = new Stopwatch();
                                    sw.Restart();
                                    spawner.SpawnTree(tree);
                                    sw.Stop();

                                    VegetationSpawnerEditor.Log.Add("Respawning tree: " + sw.Elapsed.Milliseconds + "ms...");
                                }
                            }

                        }
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Nothing selected", MessageType.Info);
                }

                if (treeChange.changed)
                {
                    EditorUtility.SetDirty(target);
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }

        private void PrefabPickingActions()
        {
            //New specifics (initial prefab)
            if (Event.current.commandName == "ObjectSelectorClosed" &&
                EditorGUIUtility.GetObjectPickerControlID() == newTreeprefabPickerWindowID)
            {
                GameObject pickedPrefab = (GameObject)EditorGUIUtility.GetObjectPickerObject();
                newTreeprefabPickerWindowID = -1;

                //if (pickedPrefab == null) return;

                VegetationSpawner.TreeType tree = SpawnerBase.TreeType.New(pickedPrefab);

                spawner.treeTypes.Add(tree);
                //Auto select new
                selectedTreeID = spawner.treeTypes.Count - 1;
                
                if(spawner.autoRespawnTrees) spawner.SpawnTree(tree);
                
                EditorUtility.SetDirty(target);
            }
            
            //Specifies prefabs
            if (Event.current.commandName == "ObjectSelectorClosed" &&
                EditorGUIUtility.GetObjectPickerControlID() == prefabPickerWindowID)
            {
                GameObject pickedPrefab = (GameObject)EditorGUIUtility.GetObjectPickerObject();
                prefabPickerWindowID = -1;

                //if (pickedPrefab == null) return;

                TreeType tree = spawner.treeTypes[selectedTreeID];

                TreePrefab treePrefab = new TreePrefab();
                treePrefab.probability = 100;
                treePrefab.prefab = pickedPrefab;
                tree.prefabs.Add(treePrefab);
                
                spawner.RefreshTreePrefabs();
                
                if(spawner.autoRespawnTrees) spawner.SpawnTree(tree);
                
                EditorUtility.SetDirty(target);
            }
        }

        private GrassPrefab GetGrassPrefab(int index)
        {
            if (index < 0 || spawner.grassPrefabs == null) return null;
            
            return spawner.grassPrefabs.Count > 0 ? spawner.grassPrefabs[selectedGrassID] : null;
        }

        private void DrawGrass()
        {
            EditorGUILayout.LabelField("Items", EditorStyles.boldLabel);
            
            //Edge case: Clamp in case there's a switch to scene with less items
            selectedGrassID = Mathf.Min(selectedGrassID, spawner.grassPrefabs.Count - 1);
            
            grassScrollPos = EditorGUILayout.BeginScrollView(grassScrollPos, EditorStyles.helpBox, GUILayout.MaxHeight(thumbSize + 10f), GUILayout.MinHeight(thumbSize + 10f));
            using (new EditorGUILayout.HorizontalScope())
            {
                for (int i = 0; i < spawner.grassPrefabs.Count; i++)
                {
                    if (spawner.grassPrefabs[i] == null) continue;

                    Texture2D thumb = spawner.grassPrefabs[i].billboard;
                    if (spawner.grassPrefabs[i].type == SpawnerBase.GrassType.Mesh) thumb = AssetPreview.GetAssetPreview(spawner.grassPrefabs[i].prefab);
                    if (thumb == null) thumb = EditorGUIUtility.IconContent("d_BuildSettings.Broadcom").image as Texture2D;
                    
                    if (GUILayout.Button(new GUIContent(spawner.grassPrefabs[i].name, thumb), (selectedGrassID == i) ? VegetationSpawnerEditor.PreviewTexSelected : VegetationSpawnerEditor.PreviewTex, GUILayout.Height(thumbSize), GUILayout.Width(thumbSize)))
                    {
                       selectedGrassID = i;
                    }
                }
            }
            EditorGUILayout.EndScrollView();

            VegetationSpawner.GrassPrefab grass = GetGrassPrefab(selectedGrassID);
                
            Undo.RecordObject(spawner, "Modified grass");

            serializedObject.Update();
            using (var grassChange = new EditorGUI.ChangeCheckScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (grass != null) EditorGUILayout.LabelField("Instances: " + grass.instanceCount.ToString("##,#"), EditorStyles.miniLabel);
                    GUILayout.FlexibleSpace();

                    
                    if (GUILayout.Button(new GUIContent("Add", EditorGUIUtility.IconContent(EditorGUIUtility.isProSkin ? "d_Toolbar Plus" : "Toolbar Plus").image, "Add new item"), EditorStyles.miniButtonLeft, GUILayout.Width(60f)))
                    {
                        VegetationSpawner.GrassPrefab newGrass = new SpawnerBase.GrassPrefab();
                        spawner.grassPrefabs.Add(newGrass);

                        newGrass.seed = Random.Range(0, 9999);
                        selectedGrassID = spawner.grassPrefabs.Count - 1;
                        newGrass.index = spawner.grassPrefabs.Count;

                        spawner.RefreshGrassPrototypes();
                    }

                    using (new EditorGUI.DisabledScope(grass == null))
                    {
                        if (GUILayout.Button(new GUIContent("", EditorGUIUtility.IconContent("TreeEditor.Duplicate").image, "Duplicate item"), EditorStyles.miniButtonMid))
                        {
                            SpawnerBase.GrassPrefab newGrass = SpawnerBase.GrassPrefab.Duplicate(spawner.grassPrefabs[selectedGrassID]);

                            spawner.grassPrefabs.Add(newGrass);
                            selectedGrassID = spawner.grassPrefabs.Count - 1;
                            newGrass.index = spawner.grassPrefabs.Count;

                            spawner.RefreshGrassPrototypes();

                            //Spawn it now so changes are visible
                            spawner.SpawnGrass(spawner.grassPrefabs[selectedGrassID]);
                        }

                        if (GUILayout.Button(new GUIContent("", EditorGUIUtility.IconContent("d_TreeEditor.Trash").image, "Remove"), EditorStyles.miniButtonRight))
                        {
                            spawner.grassPrefabs.RemoveAt(selectedGrassID);

                            selectedGrassID = spawner.grassPrefabs.Count - 1;

                            spawner.RefreshGrassPrototypes();
                        }
                    }
                }

                if (grass != null)
                {
                    grass.enabled = EditorGUILayout.Toggle("Enabled", grass.enabled);
                    grass.name = EditorGUILayout.TextField("Name", grass.name);
                    
                    EditorGUILayout.Space();
                    
                    EditorGUILayout.LabelField("Appearance", EditorStyles.boldLabel);

                    grass.type = (SpawnerBase.GrassType)EditorGUILayout.Popup("Render type", (int)grass.type, new[] { "Mesh", "Texture" }, GUILayout.Width(EditorGUIUtility.labelWidth + 80f));

                    if (grass.type == SpawnerBase.GrassType.Mesh)
                    {
                        grass.prefab = EditorGUILayout.ObjectField("Prefab", grass.prefab, typeof(GameObject), true) as GameObject;
                        
                        //Update from 1.0.4 to 1.0.5
                        if (grass.name == "VegetationItem" && grass.prefab) grass.name = grass.prefab.name;
                    }
                    if (grass.type == SpawnerBase.GrassType.Texture)
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField("Texture", GUILayout.MaxWidth(EditorGUIUtility.labelWidth));
                            grass.billboard = (Texture2D)EditorGUILayout.ObjectField(grass.billboard, typeof(Texture2D), false);
                            
                            //Update from 1.0.4 to 1.0.5
                            if (grass.name == "VegetationItem" && grass.billboard) grass.name = grass.billboard.name;
                        }
                        grass.renderAsBillboard = EditorGUILayout.Toggle(new GUIContent("Billboard", "When enabled, orients the grass towards the camera on the Y axis"), grass.renderAsBillboard);
                    }
                    
                    grass.mainColor = EditorGUILayout.ColorField("Primary", grass.mainColor, GUILayout.MaxWidth(EditorGUIUtility.labelWidth + 60f));
                    EditorGUI.indentLevel++;
                    grass.linkColors = EditorGUILayout.Toggle(new GUIContent("Link colors", "Use the primary color as the secondary color, also"), grass.linkColors);
                    EditorGUI.indentLevel--;
                    if (!grass.linkColors) grass.secondaryColor = EditorGUILayout.ColorField("Secondary", grass.secondaryColor, GUILayout.MaxWidth(EditorGUIUtility.labelWidth + 60f));

                    grass.noiseSize = EditorGUILayout.FloatField(new GUIContent("Noise size", "Grass size and color variation is controlled by an internal noise value. This controls the tiling size of the noise"), grass.noiseSize);
                    
                    DrawRangeSlider(new GUIContent("Width", "Min/max width of the mesh"), ref grass.minMaxWidth, 0f, 3f);
                    DrawRangeSlider(new GUIContent("Length", "Min/max length of the mesh"), ref grass.minMaxHeight, 0f, 3f);

                    if (grassChange.changed)
                    {
                        EditorUtility.SetDirty(target);
                        spawner.UpdateProperties(grass);
                    }

                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Spawning rules", EditorStyles.boldLabel);

                    DrawSeedField(ref grass.seed);

                    grass.probability = EditorGUILayout.Slider("Spawn chance %", grass.probability, 0f, 100f);
                    grass.collisionCheck = EditorGUILayout.Toggle(new GUIContent("Collision check", "Take into account the collision cache to avoid spawning inside colliders (see Settings tab)"), grass.collisionCheck);
                    grass.rejectUnderwater = EditorGUILayout.Toggle(new GUIContent("Remove underwater", "The water height level can be set in the settings tab"), grass.rejectUnderwater);
                    
                    EditorGUILayout.Space();
                    
                    DrawRangeSlider(new GUIContent("Height range", "Min/max height this item can spawn at"), ref grass.heightRange, 0f, 1000f);
                    DrawRangeSlider(new GUIContent("Slope range", "Min/max slope (0-90 degrees) this item can spawn at"), ref grass.slopeRange, 0f, 90f);
                    DrawRangeSlider(new GUIContent("Curvature range", "0=Concave (bowl), 0.5=flat, 1=convex (edge)"), ref grass.curvatureRange, 0f, 1f);

                    //EditorGUILayout.Space();
                    LayerMaskSettings(grass.layerMasks);

                    if (grassChange.changed)
                    {
                        serializedObject.ApplyModifiedProperties();
                    }
                    
                    EditorGUILayout.Space();

                    if (GUILayout.Button(new GUIContent(" Respawn", EditorGUIUtility.IconContent("d_Refresh").image), GUILayout.MaxHeight(30f)))
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Restart();

                        spawner.SpawnGrass(grass);

                        sw.Stop();
                        
                        VegetationSpawnerEditor.Log.Add("Respawned " + grass.name + " in " + sw.Elapsed.Seconds + " seconds...");
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Nothing selected", MessageType.Info);
                }
            }

        }

        private void DrawDetailResolutionField()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                serializedObject.Update();
                EditorGUI.BeginChangeCheck();
                detailResolutionIndex.intValue = EditorGUILayout.Popup(new GUIContent("Grass map resolution",
                        "Controls the resolution of the internal detail map. A 512px resolution on a 1024x1024 terrain means grass can be spawned a minimum of 0.5 units apart" +
                        "\n\nHigher resolutions will increase the spawning time, but also allows for denser grass"),
                    detailResolutionIndex.intValue, detailResolutions, GUILayout.Width(EditorGUIUtility.labelWidth + 80f));
                
                grassPatchSizeIndex.intValue = EditorGUILayout.Popup(new GUIContent("Grass patch size",
                        "Grass meshes are divided up into a grid, and meshes are combined (batching).\n\n" +
                        "A higher size means fewer draw calls, but the terrain will thrown an warning if a patch's vertex count exceeds 65K vertices, in which case the size should be lowered"),
                    grassPatchSizeIndex.intValue, patchSizes, GUILayout.Width(EditorGUIUtility.labelWidth + 80f));

                if (EditorGUI.EndChangeCheck())
                {
                    detailResolution.intValue = int.Parse(detailResolutions[detailResolutionIndex.intValue].Substring(0, detailResolutions[detailResolutionIndex.intValue].IndexOf("px")));
                    grassPatchSize.intValue = int.Parse(patchSizes[grassPatchSizeIndex.intValue].Substring(0, patchSizes[grassPatchSizeIndex.intValue].IndexOf("x")));
                    
                    serializedObject.ApplyModifiedProperties();

                    spawner.SetDetailResolution();
                }
                if (spawner.terrains[0])
                {
                    int density = (int)spawner.terrains[0].terrainData.size.x / detailResolution.intValue;

                    if (density > 2)
                    {
                        EditorGUILayout.HelpBox("Resolution is too low relative to the individual terrain size, grass cannot be spawned densely.\n\nIt should be at least half the size of a terrain", MessageType.Warning);
                    }
                }
            EditorGUILayout.HelpBox("Changing these settings will clear all grass, and requires all grass to be respawned!", MessageType.Warning);
            }
        }

        private void DrawSettings()
        {
            EditorGUILayout.LabelField("Spawning", EditorStyles.boldLabel);
            
            EditorGUI.BeginChangeCheck();
            
            DrawSeedField(ref spawner.seed);
            
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);

                serializedObject.ApplyModifiedProperties();
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                //EditorGUILayout.PrefixLabel(" ");
                
                if (GUILayout.Button(new GUIContent(" Respawn everything",
                    EditorGUIUtility.IconContent(iconPrefix + "Refresh").image)))
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Restart();
                    spawner.Respawn();

                    sw.Stop();

                    VegetationSpawnerEditor.Log.Add("Complete respawn: " + sw.Elapsed.Seconds + " seconds...");
                }
                if (GUILayout.Button(new GUIContent(" Respawn grass",
                    EditorGUIUtility.IconContent(iconPrefix + "Refresh").image)))
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Restart();
                    
                    spawner.Respawn(true, false);

                    sw.Stop();

                    VegetationSpawnerEditor.Log.Add("Complete grass respawn: " + sw.Elapsed.Seconds + " seconds...");
                }
                if (GUILayout.Button(new GUIContent(" Respawn trees",
                    EditorGUIUtility.IconContent(iconPrefix + "Refresh").image)))
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Restart();
                    
                    spawner.Respawn(false, true);

                    sw.Stop();

                    VegetationSpawnerEditor.Log.Add("Complete tree respawn: " + sw.Elapsed.Seconds + " seconds...");
                }
            }
            
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            
            DrawDetailResolutionField();
            
            EditorGUILayout.PropertyField(autoRespawnTrees);
            EditorGUILayout.PropertyField(waterHeight);

            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("Collision cache", EditorStyles.boldLabel);

            spawner.RebuildCollisionCacheIfNeeded();
            
            VisualizeCellsPersistent = VegetationSpawner.VisualizeCells;

            EditorGUILayout.PropertyField(cellSize);
            EditorGUILayout.PropertyField(cellDivisions);
            EditorGUILayout.PropertyField(highPrecisionCollision);
            EditorGUILayout.PropertyField(collisionLayerMask);
            EditorGUILayout.PropertyField(tempColliders, true);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(EditorGUIUtility.labelWidth);
                if (GUILayout.Button("Rebuild cache"))
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Restart();
                    spawner.RebuildCollisionCache();
                    sw.Stop();

                    VegetationSpawnerEditor.Log.Add("Rebuilding collision cache: " + sw.Elapsed.Milliseconds + "ms...");
                }
            }
            
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);

                serializedObject.ApplyModifiedProperties();
            }
        }

        private Texture2D GetLayerMainTex(int index)
        {
            if (spawner.terrains == null || spawner.terrains.Count == 0 || !spawner.terrains[0]) {return EditorGUIUtility.IconContent("d_BuildSettings.Broadcom").image as Texture2D;}
            
            TerrainLayer layer = null;

            if (index < spawner.terrains[0].terrainData.terrainLayers.Length) layer = spawner.terrains[0].terrainData.terrainLayers[index];

            if (!layer) return EditorGUIUtility.IconContent("d_BuildSettings.Broadcom").image as Texture2D;

            return layer.diffuseTexture ? layer.diffuseTexture : Texture2D.whiteTexture;
        }

        private Vector2 terrainLayerScrollPos;
        private void LayerMaskSettings(List<SpawnerBase.TerrainLayerMask> masks)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("Layer masks", "Masks can be used to only spawn items on specific terrain layers"), EditorStyles.boldLabel);
            
            selectedLayerID = Mathf.Clamp(selectedLayerID, 0, masks.Count - 1);
            
            terrainLayerScrollPos = EditorGUILayout.BeginScrollView(terrainLayerScrollPos, EditorStyles.textArea, GUILayout.MaxHeight(masks.Count > 0 ? texThumbSize + 17f : 17f));
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    for (int i = 0; i < masks.Count; i++)
                    {
                        //Select preview texture
                        previewTex = GetLayerMainTex(masks[i].layerID);

                        if (GUILayout.Button(new GUIContent(previewTex), (selectedLayerID == i) ? VegetationSpawnerEditor.PreviewTexSelected : VegetationSpawnerEditor.PreviewTex,
                        GUILayout.Width(texThumbSize), GUILayout.MaxHeight(texThumbSize)))
                        {
                            selectedLayerID = i;
                        }
                    }
                }
            }
            EditorGUILayout.EndScrollView();
            
            SpawnerBase.TerrainLayerMask selected = masks.Count > 0 && selectedLayerID >= 0 ? masks[selectedLayerID] : null;
                
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button(new GUIContent("Add", VegetationSpawnerEditor.PlusIcon, "Add new layer mask"), EditorStyles.miniButtonLeft, GUILayout.Width(60f)))
                {
                    LayerDropDown(masks);
                }

                using (new EditorGUI.DisabledScope(selected == null))
                {
                    if (GUILayout.Button(new GUIContent("", VegetationSpawnerEditor.TrashIcon, "Remove"), EditorStyles.miniButtonRight))
                    {
                        masks.Remove(selected);
                        selectedLayerID = masks.Count - 1;
                    }
                }
            }

            if (selected != null)
            {
                EditorGUILayout.LabelField(selected.name + " settings", EditorStyles.boldLabel);

                selected.threshold = EditorGUILayout.Slider(new GUIContent("Minimum strength", "The minimum strength the material must have underneath the item, before it will spawn"), selected.threshold, 0f, 1f);
            }
            
        }

        private List<SpawnerBase.TerrainLayerMask> contextMasks;
        
        private void LayerDropDown(List<SpawnerBase.TerrainLayerMask> masks)
        {
            if (spawner.terrains.Count == 0)
            {
                Debug.LogError("No terrains assigned");
                return;
            }

            contextMasks = masks;

            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < spawner.terrains[0].terrainData.terrainLayers.Length; i++)
            {
                if(spawner.terrains[0].terrainData.terrainLayers[i] == null) continue;
                
                //Check if layer already added
                if (masks.Find(x => x.layerID == i) == null)
                    menu.AddItem(new GUIContent(spawner.terrains[0].terrainData.terrainLayers[i].name), false, AddTerrainLayerMask, i);
            }
            menu.ShowAsContext();
        }

        private void AddTerrainLayerMask(object id)
        {
            SpawnerBase.TerrainLayerMask m = new SpawnerBase.TerrainLayerMask();
            m.layerID = (int)id;
            m.name = spawner.terrains[0].terrainData.terrainLayers[m.layerID].name;

            contextMasks.Add(m);
            selectedLayerID = contextMasks.Count - 1;
        }

        private void OnUndoRedo()
        {
            //Terrain
            if (TabID == 0)
            {
                spawner.CopySettingsToTerrains();
            }
            //Tree
            if (TabID == 1)
            {
                if (selectedTreeID > -1 && selectedTreeID < spawner.treeTypes.Count)
                {
                    //A tad slower, but a tree prefab was possibly removed, then undone
                    spawner.RefreshTreePrefabs();
                    
                    spawner.UpdateTreeItem(spawner.treeTypes[selectedLayerID]);
                    
                    if(autoRespawnTrees.boolValue) spawner.SpawnTree(spawner.treeTypes[selectedTreeID]);
                }
            }
            //Grass
            if (TabID == 2)
            {
                if(selectedGrassID < spawner.grassPrefabs.Count) spawner.UpdateProperties(spawner.grassPrefabs[selectedGrassID]);
            }
        }
    }
}