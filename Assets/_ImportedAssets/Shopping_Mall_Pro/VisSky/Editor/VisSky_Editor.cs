using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(VisSky))]

public class VisSky_Editor : Editor  {
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//
//                        VisSky - Editor Script 
//
//                by Andre "AEG" Bürger / VIS-Games 2011 
//
//                       http://www.vis-games.de
//    
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------





//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
public void Start()
{
   

    VisSky vissky = target as VisSky;

    vissky.using_unity_pro = true;

    vissky.time_hour = 13;
    vissky.time_minutes = 0;

    vissky.time_speed = 0.0f;

    vissky.dynamic_weather_change = false;

    vissky.MAX_CLOUDS = 500;

    vissky.wind_xspeed = 15.0f;
    vissky.wind_zspeed = 50.0f;

    vissky.viewport_center_obj = null;
    
    vissky.stars_active = false;
    vissky.MAX_STARS = 100;

    vissky.contrails_active = false;
    vissky.MAX_CONTRAIL_CREATORS = 1;

    vissky.MAX_CONTRAIL_PLANES = 1000;

    vissky.moon_shadows = false;
}
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
public override void OnInspectorGUI()
{
	EditorGUIUtility.LookLikeControls(200, 50);

    VisSky vissky = target as VisSky;

   	if(!vissky.editor_image)
    {
		vissky.editor_image = (Texture2D)UnityEngine.Resources.LoadAssetAtPath("Assets/VisSky/Textures/editor_logo.png", typeof(Texture2D));
	}
    EditorGUILayout.Separator();
    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // draw image
    //
	Rect imageRect = EditorGUILayout.BeginHorizontal();
	imageRect.x = imageRect.width / 2 - 160;
	if (imageRect.x < 0) {
		imageRect.x = 0;
	}
	imageRect.width = 320;
	imageRect.height = 140;
	GUI.DrawTexture(imageRect, vissky.editor_image);
	EditorGUILayout.EndHorizontal();

    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();


    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // Running under unityPro yes/no
    //
    vissky.using_unity_pro = EditorGUILayout.Toggle("Running under UnityPro", vissky.using_unity_pro);

    EditorGUILayout.Separator();
    EditorGUILayout.Separator();

    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // Viewport Center Object/Camera
    //
    if(vissky.using_unity_pro == true)
    {
        vissky.viewport_center_obj = (GameObject)EditorGUILayout.ObjectField("Viewport Camera or Object", vissky.viewport_center_obj, typeof(GameObject), true);
        
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
    }
    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // Time configuration
    //
    EditorGUILayout.PrefixLabel("Time hours");
    vissky.time_hour = EditorGUILayout.IntSlider(vissky.time_hour, 0, 23);
        
    EditorGUILayout.PrefixLabel("Time minutes");
    vissky.time_minutes = EditorGUILayout.IntSlider(vissky.time_minutes, 0, 59);
    
    EditorGUILayout.PrefixLabel("Time speed");
    vissky.time_speed = EditorGUILayout.Slider(vissky.time_speed, 0.0f, 3600.0f);

    EditorGUILayout.Separator();
    EditorGUILayout.Separator();

    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // Wind speed and direction
    //
    EditorGUILayout.PrefixLabel("Wind X-Speed");
    vissky.wind_xspeed = EditorGUILayout.Slider(vissky.wind_xspeed, -100.0f, 100.0f);
        
    EditorGUILayout.PrefixLabel("Wind Z-Speed");
    vissky.wind_zspeed = EditorGUILayout.Slider(vissky.wind_zspeed, -100.0f, 100.0f);
   
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();

    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // Comment for parameters that can only be changed when application is not running
    //
    EditorGUILayout.PrefixLabel("Change the following paramters");
    EditorGUILayout.PrefixLabel("only when application is stopped.");

    EditorGUILayout.Separator();
    EditorGUILayout.Separator();

    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // Cast Moon Shadows
    //
    vissky.moon_shadows = EditorGUILayout.Toggle("Moon cast shadows", vissky.moon_shadows);

    EditorGUILayout.Separator();
    EditorGUILayout.Separator();

    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // Max Clouds
    //
    vissky.MAX_CLOUDS = EditorGUILayout.IntField("Max Clouds",   vissky.MAX_CLOUDS);

    EditorGUILayout.Separator();
    EditorGUILayout.Separator();

    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // Dynamic cloud-density-change
    //
    vissky.dynamic_weather_change = EditorGUILayout.Toggle("Dynamic Cloud-Density-Change", vissky.dynamic_weather_change);

    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    
    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // Stars configuration
    //
    vissky.stars_active = EditorGUILayout.Toggle("Stars at Nightsky", vissky.stars_active);
    EditorGUILayout.PrefixLabel("Max Stars at Nightsky");
    vissky.MAX_STARS = EditorGUILayout.IntSlider(vissky.MAX_STARS, 1, 500);

    EditorGUILayout.Separator();
    EditorGUILayout.Separator();

    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // Airplane-contrails configuration
    //
    vissky.contrails_active = EditorGUILayout.Toggle("Show Airplane Contrails", vissky.contrails_active);
    EditorGUILayout.PrefixLabel("Max Airplanes");
    vissky.MAX_CONTRAIL_CREATORS = EditorGUILayout.IntSlider(vissky.MAX_CONTRAIL_CREATORS, 1, 10);

    EditorGUILayout.Separator();
    EditorGUILayout.Separator();

}
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------















//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
}