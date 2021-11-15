// Vegetation Spawner by Staggart Creations http://staggart.xyz
// Copyright protected under Unity Asset Store EULA

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace sc.terrain.vegetationspawner
{
    public class VegetationSpawnerEditor
    {
        private const string TreeIconData = "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";
        private const string GrassIconData = "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";
        
        public const float thumbSize = 100f;
        public const float texThumbSize = 50f;
        
        public static string iconPrefix => EditorGUIUtility.isProSkin ? "d_" : "";
        
        public static void DrawRangeSlider(GUIContent label, ref Vector2 input, float min, float max)
        {
            float minBrightness = input.x;
            float maxBrightness = input.y;

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(label, GUILayout.MaxWidth(EditorGUIUtility.labelWidth));

                minBrightness = EditorGUILayout.FloatField(minBrightness, GUILayout.MaxWidth(40f));
                EditorGUILayout.MinMaxSlider(ref minBrightness, ref maxBrightness, min, max);
                maxBrightness = EditorGUILayout.FloatField(maxBrightness, GUILayout.MaxWidth(40f));
            }

            input.x = minBrightness;
            input.y = maxBrightness;
        }

        public static void DrawSeedField(ref int seed)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                seed = EditorGUILayout.IntField("Seed", seed, GUILayout.MaxWidth(EditorGUIUtility.labelWidth + 50f));
                if (GUILayout.Button("Randomize", GUILayout.MaxWidth(100f)))
                {
                    seed = UnityEngine.Random.Range(0, 99999);
                }
            }
        }
        
        private static Texture CreateIcon(string data)
        {
            byte[] bytes = System.Convert.FromBase64String(data);

            Texture2D icon = new Texture2D(16, 16, TextureFormat.RGBA32, false, false);
            icon.LoadImage(bytes, true);
            
            return icon;
        }

        private static GUIStyle _PreviewTex;
        public static GUIStyle PreviewTex
        {
            get
            {
                if (_PreviewTex == null)
                {
                    _PreviewTex = new GUIStyle(EditorStyles.label)
                    {
                        clipping = TextClipping.Clip,
                        alignment = TextAnchor.MiddleCenter,
                        imagePosition = ImagePosition.ImageAbove
                    };
                }
                return _PreviewTex;
            }
        }

        private static GUIStyle _PreviewTexSelected;
        public static GUIStyle PreviewTexSelected
        {
            get
            {
                if (_PreviewTexSelected == null)
                {
                    _PreviewTexSelected = new GUIStyle(EditorStyles.objectFieldThumb)
                    {
                        clipping = TextClipping.Clip,
                        alignment = TextAnchor.MiddleCenter,
                        imagePosition = ImagePosition.ImageAbove
                    };
                }
                return _PreviewTexSelected;
            }
        }

        private static Texture _TerrainIcon;
        public static Texture TerrainIcon
        {
            get
            {
                if (_TerrainIcon == null)
                {
#if UNITY_2019_3_OR_NEWER
                    _TerrainIcon = EditorGUIUtility.IconContent(iconPrefix + "Terrain Icon").image;
#else
                    _TerrainIcon = EditorGUIUtility.IconContent("Terrain Icon").image;
#endif
                }
                return _TerrainIcon;
            }
        }

        private static Texture _TreeIcon;
        public static Texture TreeIcon
        {
            get
            {
                if (_TreeIcon == null)
                {
                    _TreeIcon = CreateIcon(TreeIconData);
                }
                return _TreeIcon;
            }
        }

        private static Texture _DetailIcon;
        public static Texture DetailIcon
        {
            get
            {
                if (_DetailIcon == null)
                {
                    _DetailIcon = CreateIcon(GrassIconData);
                }
                return _DetailIcon;
            }
        }

        private static Texture _PlusIcon;
        public static Texture PlusIcon
        {
            get
            {
                if (_PlusIcon == null)
                {
                    _PlusIcon = EditorGUIUtility.IconContent(iconPrefix + "Toolbar Plus").image;
                }
                return _PlusIcon;
            }
        }

        private static Texture _TrashIcon;
        public static Texture TrashIcon
        {
            get
            {
                if (_TrashIcon == null)
                {
                    _TrashIcon = EditorGUIUtility.IconContent(iconPrefix + "TreeEditor.Trash").image;
                }
                return _TrashIcon;
            }
        }

        public class Log
        {
            private static int MaxItems = 9;

            public static List<string> items = new List<string>();

            public static void Add(string text)
            {
                if (items.Count >= MaxItems) items.RemoveAt(items.Count - 1);

                string hourString = ((DateTime.Now.Hour <= 9) ? "0" : "") + DateTime.Now.Hour;
                string minuteString = ((DateTime.Now.Minute <= 9) ? "0" : "") + DateTime.Now.Minute;
                string secString = ((DateTime.Now.Second <= 9) ? "0" : "") + DateTime.Now.Second;
                string timeString = "[" + hourString + ":" + minuteString + ":" + secString + "] ";

                items.Insert(0, timeString + text);
            }
        }

    }
}