using UnityEngine;
using System;
using System.Collections;

public class VisSky : MonoBehaviour {
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
//
//
//                                           VisSky by Andre "AEG" Bürger of VIS-Games 2011 
//                                                  
//                                                            Version 1.0
//
// -Static or dynamic time-system
// -Day/Night-change
// -Simple cloud-density change (weather change) 
// -Stars and moon at nightsky
// -Contrails from airplanes
// -controlable windspeed for clouds
// -changing light/shadow intensities by cloud-sun-occlusion
// -Changing Sky/Cloud-Colors by daytime and cloud-density
//
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------

    //---------------------------------------------------
    //---------------------------------------------------
    //---------------------------------------------------
    //
    // Public Vars
    //
    public bool using_unity_pro = true;

    public float time_speed = 1.0f;

    public int time_hour = 13;
    public int time_minutes = 0;

    public bool dynamic_weather_change = false;

    public int MAX_CLOUDS = 1000;

    public float wind_xspeed = 10.0f;
    public float wind_zspeed = 10.0f;

    public Texture2D cloud_texture2d = null;

    public Texture2D gradient_source = null;
    public Texture2D gradient = null;

    public GameObject viewport_center_obj = null;
    
    public bool stars_active = false;
    public int MAX_STARS = 100;

    public bool contrails_active = false;
    public int MAX_CONTRAIL_CREATORS = 1;
    public int MAX_CONTRAIL_PLANES = 1000;

    public bool moon_shadows = false;

    public Texture2D editor_image;
    //---------------------------------------------------
    //---------------------------------------------------
    //---------------------------------------------------
    //
    // Internal Vars
    //
    GameObject cloud_01;
    GameObject cloud_02;
    GameObject cloud_03;
    GameObject cloud_04;
    GameObject cloud_05;
    GameObject cloud_06;
    GameObject cloud_07;
    GameObject cloud_08;
    GameObject cloud_09;
    GameObject cloud_10;
    GameObject cloud_11;
    GameObject cloud_12;
    GameObject cloud_13;
    GameObject cloud_14;
    GameObject cloud_15;
    GameObject cloud_16;
    GameObject cloud_17;
    GameObject cloud_18;
    GameObject cloud_19;
    GameObject cloud_20;
    GameObject cloud_21;
    GameObject cloud_22;
    
    GameObject sky;

    Renderer sky_renderer;
    Renderer sunflare_renderer;
    Renderer moonplane_renderer;


    Material cloud_material;

    float[] clouds_xpos;
    float[] clouds_ypos;
    float[] clouds_zpos;
    float[] clouds_scale;
    float[] clouds_yrot;
    GameObject[] clouds_obj;

    float cloud_scale_factor;
    float cloud_dest_scale_factor;

    int   weather_change_mode;
    float weather_change_counter;

    bool[]  contrails_creator_active;
    float[] contrails_creator_xpos;
    float[] contrails_creator_ypos;
    float[] contrails_creator_zpos;
    float[] contrails_creator_xadd;
    float[] contrails_creator_zadd;
    float[] contrails_creator_yrot;
    float[] contrails_creator_counter;

    float[] contrails_lifetime;
    GameObject[] contrails_object;

    int contrail_planes_active;

    GameObject contrail_obj;
    Material contrail_material;

    GameObject star_01;
    GameObject star_02;
    GameObject star_03;
    GameObject star_04;
    GameObject star_05;
    GameObject star_06;
    GameObject star_07;
    GameObject star_08;

    Material stars_material;

    GameObject sun;
    Light sun_light;
    GameObject sun_flare;

    GameObject moon;
    Light moon_light;
    GameObject moon_plane;

    float time_msecs;
    int   time_seconds;

    Camera cloudcam;

    int last_frame_sky_gradient;
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
//
//  Color and Intensity Lists
//
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
float[] sun_intensity_table =
{
    0.0f,           //-- 00:00
    0.0f,           //-- 00:30
    0.0f,           //-- 01:00
    0.0f,           //-- 01:30
    0.0f,           //-- 02:00
    0.0f,           //-- 02:30
    0.0f,           //-- 03:00
    0.0f,           //-- 03:30
    0.0f,           //-- 04:00
    0.0f,           //-- 04:30
    0.0f,           //-- 05:00
    0.1f,           //-- 05:30
    0.2f,           //-- 06:00
    0.3f,           //-- 06:30
    0.4f,           //-- 07:00
    0.5f,           //-- 07:30
    0.6f,           //-- 08:00
    0.7f,           //-- 08:30
    0.8f,           //-- 09:00
    0.9f,           //-- 09:30
    1.0f,           //-- 10:00
    1.0f,           //-- 10:30
    1.0f,           //-- 11:00
    1.0f,           //-- 11:30
    1.0f,           //-- 12:00
    1.0f,           //-- 12:30
    1.0f,           //-- 13:00
    1.0f,           //-- 13:30
    1.0f,           //-- 14:00
    1.0f,           //-- 14:30
    1.0f,           //-- 15:00
    1.0f,           //-- 15:30
    1.0f,           //-- 16:00
    1.0f,           //-- 16:30
    1.0f,           //-- 17:00
    1.0f,           //-- 17:30
    1.0f,           //-- 18:00
    1.0f,           //-- 18:30
    1.0f,           //-- 19:00
    1.0f,           //-- 19:30
    1.0f,           //-- 20:00
    1.0f,           //-- 20:30
    1.1f,           //-- 21:00
    1.5f,           //-- 21:30
    1.3f,           //-- 22:00
    1.0f,           //-- 22:30
    0.5f,           //-- 23:00
    0.0f,           //-- 23:30
};

float[] ambient_intensity_table =
{
    0.35f,          //-- 00:00
    0.35f,          //-- 00:30
    0.35f,          //-- 01:00
    0.35f,          //-- 01:30
    0.35f,          //-- 02:00
    0.35f,          //-- 02:30
    0.35f,          //-- 03:00
    0.35f,          //-- 03:30
    0.35f,          //-- 04:00
    0.35f,          //-- 04:30
    0.35f,          //-- 05:00
    0.35f,          //-- 05:30
    0.35f,          //-- 06:00
    0.4f,           //-- 06:30
    0.45f,          //-- 07:00
    0.5f,           //-- 07:30
    0.5f,           //-- 08:00
    0.5f,           //-- 08:30
    0.5f,           //-- 09:00
    0.6f,           //-- 09:30
    0.6f,           //-- 10:00
    0.6f,           //-- 10:30
    0.6f,           //-- 11:00
    0.6f,           //-- 11:30
    0.6f,           //-- 12:00
    0.6f,           //-- 12:30
    0.6f,           //-- 13:00
    0.6f,           //-- 13:30
    0.6f,           //-- 14:00
    0.6f,           //-- 14:30
    0.6f,           //-- 15:00
    0.6f,           //-- 15:30
    0.6f,           //-- 16:00
    0.6f,           //-- 16:30
    0.6f,           //-- 17:00
    0.6f,           //-- 17:30
    0.6f,           //-- 18:00
    0.6f,           //-- 18:30
    0.6f,           //-- 19:00
    0.6f,           //-- 19:30
    0.55f,          //-- 20:00
    0.5f,           //-- 20:30
    0.45f,          //-- 21:00
    0.4f,           //-- 21:30
    0.35f,          //-- 22:00
    0.35f,          //-- 22:30
    0.35f,          //-- 23:00
    0.35f,          //-- 23:30
};


float[] sun_shadow_intensity_table =
{
    0.0f,           //-- 00:00
    0.0f,           //-- 00:30
    0.0f,           //-- 01:00
    0.0f,           //-- 01:30
    0.0f,           //-- 02:00
    0.0f,           //-- 02:30
    0.0f,           //-- 03:00
    0.0f,           //-- 03:30
    0.0f,           //-- 04:00
    0.0f,           //-- 04:30
    0.0f,           //-- 05:00
    0.0f,           //-- 05:30
    0.1f,           //-- 06:00
    0.2f,           //-- 06:30
    0.3f,           //-- 07:00
    0.4f,           //-- 07:30
    0.5f,           //-- 08:00
    0.6f,           //-- 08:30
    0.7f,           //-- 09:00
    0.8f,           //-- 09:30
    0.9f,           //-- 10:00
    1.0f,           //-- 10:30
    1.0f,           //-- 11:00
    1.0f,           //-- 11:30
    1.0f,           //-- 12:00
    1.0f,           //-- 12:30
    1.0f,           //-- 13:00
    1.0f,           //-- 13:30
    1.0f,           //-- 14:00
    1.0f,           //-- 14:30
    1.0f,           //-- 15:00
    1.0f,           //-- 15:30
    1.0f,           //-- 16:00
    1.0f,           //-- 16:30
    1.0f,           //-- 17:00
    1.0f,           //-- 17:30
    1.0f,           //-- 18:00
    1.0f,           //-- 18:30
    1.0f,           //-- 19:00
    1.0f,           //-- 19:30
    0.8f,           //-- 20:00
    0.6f,           //-- 20:30
    0.4f,           //-- 21:00
    0.2f,           //-- 21:30
    0.0f,           //-- 22:00
    0.1f,           //-- 22:30
    0.0f,           //-- 23:00
    0.0f,           //-- 23:30
};

float[] sun_flare_intensity_table =
{
    0.0f,           //-- 00:00
    0.0f,           //-- 00:30
    0.0f,           //-- 01:00
    0.0f,           //-- 01:30
    0.0f,           //-- 02:00
    0.0f,           //-- 02:30
    0.0f,           //-- 03:00
    0.0f,           //-- 03:30
    0.0f,           //-- 04:00
    0.0f,           //-- 04:30
    0.8f,           //-- 05:00
    0.8f,           //-- 05:30
    0.8f,           //-- 06:00
    0.8f,           //-- 06:30
    0.8f,           //-- 07:00
    0.8f,           //-- 07:30
    0.8f,           //-- 08:00
    0.8f,           //-- 08:30
    0.8f,           //-- 09:00
    0.8f,           //-- 09:30
    0.8f,           //-- 10:00
    0.8f,           //-- 10:30
    0.8f,           //-- 11:00
    0.8f,           //-- 11:30
    0.8f,           //-- 12:00
    0.8f,           //-- 12:30
    0.8f,           //-- 13:00
    0.8f,           //-- 13:30
    0.8f,           //-- 14:00
    0.8f,           //-- 14:30
    0.8f,           //-- 15:00
    0.8f,           //-- 15:30
    0.8f,           //-- 16:00
    0.8f,           //-- 16:30
    0.8f,           //-- 17:00
    0.8f,           //-- 17:30
    0.8f,           //-- 18:00
    0.8f,           //-- 18:30
    1.0f,           //-- 19:00
    1.2f,           //-- 19:30
    1.4f,           //-- 20:00
    1.6f,           //-- 20:30
    1.8f,           //-- 21:00
    2.0f,           //-- 21:30
    2.5f,           //-- 22:00
    0.0f,           //-- 22:30
    0.0f,           //-- 23:00
    0.0f,           //-- 23:30
};

float[] sun_flare_scale_table =
{
    0.0f,           //-- 00:00
    0.0f,           //-- 00:30
    0.0f,           //-- 01:00
    0.0f,           //-- 01:30
    0.0f,           //-- 02:00
    0.0f,           //-- 02:30
    0.0f,           //-- 03:00
    0.0f,           //-- 03:30
    0.0f,           //-- 04:00
    0.0f,           //-- 04:30
    1.5f,           //-- 05:00
    1.4f,           //-- 05:30
    1.3f,           //-- 06:00
    1.2f,           //-- 06:30
    1.1f,           //-- 07:00
    1.0f,           //-- 07:30
    1.0f,           //-- 08:00
    1.0f,           //-- 08:30
    1.0f,           //-- 09:00
    1.0f,           //-- 09:30
    1.0f,           //-- 10:00
    1.0f,           //-- 10:30
    1.0f,           //-- 11:00
    1.0f,           //-- 11:30
    1.0f,           //-- 12:00
    1.0f,           //-- 12:30
    1.0f,           //-- 13:00
    1.0f,           //-- 13:30
    1.0f,           //-- 14:00
    1.0f,           //-- 14:30
    1.0f,           //-- 15:00
    1.0f,           //-- 15:30
    1.0f,           //-- 16:00
    1.0f,           //-- 16:30
    1.0f,           //-- 17:00
    1.0f,           //-- 17:30
    1.0f,           //-- 18:00
    1.0f,           //-- 18:30
    1.0f,           //-- 19:00
    1.0f,           //-- 19:30
    1.0f,           //-- 20:00
    1.0f,           //-- 20:30
    1.2f,           //-- 21:00
    1.6f,           //-- 21:30
    2.0f,           //-- 22:00
    0.0f,           //-- 22:30
    0.0f,           //-- 23:00
    0.0f,           //-- 23:30
};


int[] sun_color_table =
{
    0,0,0,          //-- 00:00    
    0,0,0,          //-- 00:30    
    0,0,0,          //-- 01:00    
    0,0,0,          //-- 01:30    
    0,0,0,          //-- 02:00    
    0,0,0,          //-- 02:30    
    0,0,0,          //-- 03:00    
    0,0,0,          //-- 03:30    
    0,0,0,          //-- 04:00    
    0,0,0,          //-- 04:30    
    0,0,0,          //-- 05:00    
    22,35,39,       //-- 05:30    
    64,73,65,       //-- 06:00    
    109,111,92,     //-- 06:30    
    153,149,119,    //-- 07:00    
    212,200,125,    //-- 07:30    
    235,226,169,    //-- 08:00    
    251,248,224,    //-- 08:30    
    251,248,224,    //-- 09:00    
    251,248,224,    //-- 09:30    
    251,248,224,    //-- 10:00    
    251,248,224,    //-- 10:30    
    251,248,224,    //-- 11:00    
    251,248,224,    //-- 11:30    
    251,248,224,    //-- 12:00    
    251,248,224,    //-- 12:30    
    251,248,224,    //-- 13:00    
    251,248,224,    //-- 13:30    
    251,248,224,    //-- 14:00    
    251,248,224,    //-- 14:30    
    251,248,224,    //-- 15:00    
    251,248,224,    //-- 15:30    
    251,248,224,    //-- 16:00    
    251,248,224,    //-- 16:30    
    251,248,224,    //-- 17:00    
    251,248,224,    //-- 17:30    
    240,222,191,    //-- 18:00    
    228,197,158,    //-- 18:30    
    218,171,127,    //-- 19:00    
    207,146,95,     //-- 19:30    
    196,121,63,     //-- 20:00    
    152,99,57,      //-- 20:30    
    110,50,28,      //-- 21:00    
    68,22,0,        //-- 21:30    
    32,11,0,        //-- 22:00    
    0,0,0,          //-- 22:30    
    0,0,0,          //-- 23:00    
    0,0,0,          //-- 23:30    
};

int[] sun_flare_color_table =
{
    0,0,0,          //-- 00:00    
    0,0,0,          //-- 00:30    
    0,0,0,          //-- 01:00    
    0,0,0,          //-- 01:30    
    0,0,0,          //-- 02:00    
    0,0,0,          //-- 02:30    
    0,0,0,          //-- 03:00    
    0,0,0,          //-- 03:30    
    0,0,0,          //-- 04:00    
    0,0,0,          //-- 04:30    
    0,0,0,          //-- 05:00    
    22,35,39,       //-- 05:30    
    64,73,65,       //-- 06:00    
    109,111,92,     //-- 06:30    
    153,149,119,    //-- 07:00    
    212,200,125,    //-- 07:30    
    235,226,169,    //-- 08:00    
    251,248,224,    //-- 08:30    
    251,248,224,    //-- 09:00    
    251,248,224,    //-- 09:30    
    251,248,224,    //-- 10:00    
    251,248,224,    //-- 10:30    
    251,248,224,    //-- 11:00    
    251,248,224,    //-- 11:30    
    251,248,224,    //-- 12:00    
    251,248,224,    //-- 12:30    
    251,248,224,    //-- 13:00    
    251,248,224,    //-- 13:30    
    251,248,224,    //-- 14:00    
    251,248,224,    //-- 14:30    
    251,248,224,    //-- 15:00    
    251,248,224,    //-- 15:30    
    251,248,224,    //-- 16:00    
    251,248,224,    //-- 16:30    
    251,248,224,    //-- 17:00    
    251,248,224,    //-- 17:30    
    251,248,224,    //-- 18:00    
    251,248,224,    //-- 18:30    
    251,248,224,    //-- 19:00    
    233,203,110,    //-- 19:30    
    242,225,167,    //-- 20:00    
    233,183,80,    //-- 20:30    
    180,86,32,      //-- 21:00    
    120,28,0,       //-- 21:30    
    120,28,0,       //-- 22:00    
    0,0,0,          //-- 22:30    
    0,0,0,          //-- 23:00    
    0,0,0,          //-- 23:30    
};

int[] sky_color_table =
{
    8,9,10,         //-- 00:00    
    8,9,10,         //-- 00:30    
    8,9,10,         //-- 01:00    
    8,9,10,         //-- 01:30    
    8,9,10,         //-- 02:00    
    8,9,10,         //-- 02:30    
    8,9,10,         //-- 03:00    
    8,9,10,         //-- 03:30    
    8,9,10,         //-- 04:00    
    8,9,10,         //-- 04:30    
    8,9,10,         //-- 05:00    
    30,35,37,       //-- 05:30    
    53,61,64,       //-- 06:00    
    76,87,91,       //-- 06:30    
    99,113,118,     //-- 07:00    
    121,139,146,    //-- 07:30    
    144,165,173,    //-- 08:00    
    167,191,200,    //-- 08:30    
    190,217,227,    //-- 09:00    
    213,244,255,    //-- 09:30    
    213,244,255,    //-- 10:00    
    213,244,255,    //-- 10:30    
    213,244,255,    //-- 11:00    
    213,244,255,    //-- 11:30    
    213,244,255,    //-- 12:00    
    213,244,255,    //-- 12:30    
    213,244,255,    //-- 13:00    
    213,244,255,    //-- 13:30    
    213,244,255,    //-- 14:00    
    213,244,255,    //-- 14:30    
    213,244,255,    //-- 15:00    
    213,244,255,    //-- 15:30    
    213,244,255,    //-- 16:00    
    213,244,255,    //-- 16:30    
    213,244,255,    //-- 17:00    
    213,244,255,    //-- 17:30    
    213,244,255,    //-- 18:00    
    190,217,227,    //-- 18:30    
    167,191,200,    //-- 19:00    
    144,165,173,    //-- 19:30    
    168,158,150,    //-- 20:00    
    192,151,127,    //-- 20:30    
    77,54,41,       //-- 21:00    
    55,31,25,       //-- 21:30    
    12,9,10,         //-- 22:00    
    8,9,10,         //-- 22:30    
    8,9,10,         //-- 23:00    
    8,9,10,         //-- 23:30    
};

   
int[] ambient_color_table =
{
    25,35,42,    //-- 00:00    
    25,35,42,    //-- 00:30    
    25,35,42,    //-- 01:00    
    25,35,42,    //-- 01:30    
    25,35,42,    //-- 02:00    
    25,35,42,    //-- 02:30    
    25,35,42,    //-- 03:00    
    25,35,42,    //-- 03:30    
    25,35,42,    //-- 04:00    
    25,35,42,    //-- 04:30    
    25,35,42,    //-- 05:00    
    29,38,45,    //-- 05:30    
    32,41,47,    //-- 06:00    
    35,44,50,    //-- 06:30    
    38,46,52,    //-- 07:00    
    41,49,55,    //-- 07:30    
    44,52,57,    //-- 08:00    
    47,54,59,    //-- 08:30    
    57,62,72,    //-- 09:00    
    67,70,85,    //-- 09:30    
    67,70,85,    //-- 10:00    
    67,70,85,    //-- 10:30    
    67,70,85,    //-- 11:00    
    67,70,85,    //-- 11:30    
    67,70,85,    //-- 12:00    
    67,70,85,    //-- 12:30    
    67,70,85,    //-- 13:00    
    67,70,85,    //-- 13:30    
    67,70,85,    //-- 14:00    
    67,70,85,    //-- 14:30    
    67,70,85,    //-- 15:00    
    67,70,85,    //-- 15:30    
    67,70,85,    //-- 16:00    
    67,70,85,    //-- 16:30    
    67,70,85,    //-- 17:00    
    67,70,85,    //-- 17:30    
    57,62,72,    //-- 18:00    
    47,54,59,    //-- 18:30    
    44,52,57,    //-- 19:00    
    41,49,55,    //-- 19:30    
    38,46,52,    //-- 20:00    
    35,44,50,    //-- 20:30    
    32,41,47,    //-- 21:00    
    29,38,45,    //-- 21:30    
    25,35,42,    //-- 22:00    
    25,35,42,    //-- 22:30    
    25,35,42,    //-- 23:00    
    25,35,42,    //-- 23:30    
};

float[] moon_intensity_table =
{
    0.5f,           //-- 00:00
    0.5f,           //-- 00:30
    0.5f,           //-- 01:00
    0.5f,           //-- 01:30
    0.5f,           //-- 02:00
    0.5f,           //-- 02:30
    0.5f,           //-- 03:00
    0.5f,           //-- 03:30
    0.5f,           //-- 04:00
    0.5f,           //-- 04:30
    0.5f,           //-- 05:00
    0.4f,           //-- 05:30
    0.2f,           //-- 06:00
    0.0f,           //-- 06:30
    0.0f,           //-- 07:00
    0.0f,           //-- 07:30
    0.0f,           //-- 08:00
    0.0f,           //-- 08:30
    0.0f,           //-- 09:00
    0.0f,           //-- 09:30
    0.0f,           //-- 10:00
    0.0f,           //-- 10:30
    0.0f,           //-- 11:00
    0.0f,           //-- 11:30
    0.0f,           //-- 12:00
    0.0f,           //-- 12:30
    0.0f,           //-- 13:00
    0.0f,           //-- 13:30
    0.0f,           //-- 14:00
    0.0f,           //-- 14:30
    0.0f,           //-- 15:00
    0.0f,           //-- 15:30
    0.0f,           //-- 16:00
    0.0f,           //-- 16:30
    0.0f,           //-- 17:00
    0.0f,           //-- 17:30
    0.0f,           //-- 18:00
    0.0f,           //-- 18:30
    0.0f,           //-- 19:00
    0.0f,           //-- 19:30
    0.0f,           //-- 20:00
    0.0f,           //-- 20:30
    0.0f,           //-- 21:00
    0.0f,           //-- 21:30
    0.2f,           //-- 22:00
    0.3f,           //-- 22:30
    0.4f,           //-- 23:00
    0.5f,           //-- 23:30
};

float[] stars_intensity_table =
{
    1.1f,           //-- 00:00
    1.1f,           //-- 00:30
    1.0f,           //-- 01:00
    1.0f,           //-- 01:30
    1.0f,           //-- 02:00
    1.0f,           //-- 02:30
    1.0f,           //-- 03:00
    1.0f,           //-- 03:30
    1.0f,           //-- 04:00
    0.5f,           //-- 04:30
    0.0f,           //-- 05:00
    0.0f,           //-- 05:30
    0.0f,           //-- 06:00
    0.0f,           //-- 06:30
    0.0f,           //-- 07:00
    0.0f,           //-- 07:30
    0.0f,           //-- 08:00
    0.0f,           //-- 08:30
    0.0f,           //-- 09:00
    0.0f,           //-- 09:30
    0.0f,           //-- 10:00
    0.0f,           //-- 10:30
    0.0f,           //-- 11:00
    0.0f,           //-- 11:30
    0.0f,           //-- 12:00
    0.0f,           //-- 12:30
    0.0f,           //-- 13:00
    0.0f,           //-- 13:30
    0.0f,           //-- 14:00
    0.0f,           //-- 14:30
    0.0f,           //-- 15:00
    0.0f,           //-- 15:30
    0.0f,           //-- 16:00
    0.0f,           //-- 16:30
    0.0f,           //-- 17:00
    0.0f,           //-- 17:30
    0.0f,           //-- 18:00
    0.0f,           //-- 18:30
    0.0f,           //-- 19:00
    0.0f,           //-- 19:30
    0.0f,           //-- 20:00
    0.0f,           //-- 20:30
    0.0f,           //-- 21:00
    0.0f,           //-- 21:30
    0.2f,           //-- 22:00
    0.4f,           //-- 22:30
    0.6f,           //-- 23:00
    0.8f,           //-- 23:30
};

float[] moon_shadow_intensity_table =
{
    0.6f,           //-- 00:00
    0.8f,           //-- 00:30
    1.0f,           //-- 01:00
    1.0f,           //-- 01:30
    1.0f,           //-- 02:00
    1.0f,           //-- 02:30
    0.8f,           //-- 03:00
    0.6f,           //-- 03:30
    0.4f,           //-- 04:00
    0.3f,           //-- 04:30
    0.2f,           //-- 05:00
    0.1f,           //-- 05:30
    0.0f,           //-- 06:00
    0.0f,           //-- 06:30
    0.0f,           //-- 07:00
    0.0f,           //-- 07:30
    0.0f,           //-- 08:00
    0.0f,           //-- 08:30
    0.0f,           //-- 09:00
    0.0f,           //-- 09:30
    0.0f,           //-- 10:00
    0.0f,           //-- 10:30
    0.0f,           //-- 11:00
    0.0f,           //-- 11:30
    0.0f,           //-- 12:00
    0.0f,           //-- 12:30
    0.0f,           //-- 13:00
    0.0f,           //-- 13:30
    0.0f,           //-- 14:00
    0.0f,           //-- 14:30
    0.0f,           //-- 15:00
    0.0f,           //-- 15:30
    0.0f,           //-- 16:00
    0.0f,           //-- 16:30
    0.0f,           //-- 17:00
    0.0f,           //-- 17:30
    0.0f,           //-- 18:00
    0.0f,           //-- 18:30
    0.0f,           //-- 19:00
    0.0f,           //-- 19:30
    0.0f,           //-- 20:00
    0.0f,           //-- 20:30
    0.0f,           //-- 21:00
    0.0f,           //-- 21:30
    0.0f,           //-- 22:00
    0.0f,           //-- 22:30
    0.2f,           //-- 23:00
    0.4f,           //-- 23:30
};

float[] moon_plane_intensity_table =
{
    1.0f,           //-- 00:00
    1.0f,           //-- 00:30
    1.0f,           //-- 01:00
    1.0f,           //-- 01:30
    1.0f,           //-- 02:00
    0.8f,           //-- 02:30
    0.6f,           //-- 03:00
    0.5f,           //-- 03:30
    0.4f,           //-- 04:00
    0.3f,           //-- 04:30
    0.2f,           //-- 05:00
    0.1f,           //-- 05:30
    0.0f,           //-- 06:00
    0.0f,           //-- 06:30
    0.0f,           //-- 07:00
    0.0f,           //-- 07:30
    0.0f,           //-- 08:00
    0.0f,           //-- 08:30
    0.0f,           //-- 09:00
    0.0f,           //-- 09:30
    0.0f,           //-- 10:00
    0.0f,           //-- 10:30
    0.0f,           //-- 11:00
    0.0f,           //-- 11:30
    0.0f,           //-- 12:00
    0.0f,           //-- 12:30
    0.0f,           //-- 13:00
    0.0f,           //-- 13:30
    0.0f,           //-- 14:00
    0.0f,           //-- 14:30
    0.0f,           //-- 15:00
    0.0f,           //-- 15:30
    0.0f,           //-- 16:00
    0.0f,           //-- 16:30
    0.0f,           //-- 17:00
    0.0f,           //-- 17:30
    0.0f,           //-- 18:00
    0.05f,          //-- 18:30
    0.1f,           //-- 19:00
    0.15f,          //-- 19:30
    0.2f,           //-- 20:00
    0.25f,          //-- 20:30
    0.3f,           //-- 21:00
    0.35f,          //-- 21:30
    0.4f,           //-- 22:00
    0.5f,           //-- 22:30
    0.6f,           //-- 23:00
    0.8f,           //-- 23:30
};


//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
//
//  Awake
//
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
void Awake()
{
    //--------------------------------------------------------------
    //--------------------------------------------------------------
    //--------------------------------------------------------------
    //
    // Check for missing parameters
    //
    if((viewport_center_obj == null) && (using_unity_pro == true))
    {
        Debug.LogError("Missing Viewport-Center Camera or Object. Please add the camera or a GameObject that represents the Viewport-Center-Position (Player-Position) in the the Inspector. Script will be terminated.");
        Destroy(gameObject);    
        return;
    }
    else if(using_unity_pro == false)
        viewport_center_obj = null;
    //--------------------------------------------------------------
    //--------------------------------------------------------------
    //--------------------------------------------------------------
    //
    //
    //
    cloud_01 = transform.Find("Clouds/cloud_01").gameObject;    
    cloud_02 = transform.Find("Clouds/cloud_02").gameObject;    
    cloud_03 = transform.Find("Clouds/cloud_03").gameObject;    
    cloud_04 = transform.Find("Clouds/cloud_04").gameObject;    
    cloud_05 = transform.Find("Clouds/cloud_05").gameObject;    
    cloud_06 = transform.Find("Clouds/cloud_06").gameObject;    
    cloud_07 = transform.Find("Clouds/cloud_07").gameObject;    
    cloud_08 = transform.Find("Clouds/cloud_08").gameObject;    
    cloud_09 = transform.Find("Clouds/cloud_09").gameObject;    
    cloud_10 = transform.Find("Clouds/cloud_10").gameObject;    
    cloud_11 = transform.Find("Clouds/cloud_11").gameObject;    
    cloud_12 = transform.Find("Clouds/cloud_12").gameObject;    
    cloud_13 = transform.Find("Clouds/cloud_13").gameObject;    
    cloud_14 = transform.Find("Clouds/cloud_14").gameObject;    
    cloud_15 = transform.Find("Clouds/cloud_15").gameObject;    
    cloud_16 = transform.Find("Clouds/cloud_16").gameObject;    
    cloud_17 = transform.Find("Clouds/cloud_17").gameObject;    
    cloud_18 = transform.Find("Clouds/cloud_18").gameObject;    
    cloud_19 = transform.Find("Clouds/cloud_19").gameObject;    
    cloud_20 = transform.Find("Clouds/cloud_20").gameObject;    
    cloud_21 = transform.Find("Clouds/cloud_21").gameObject;    
    cloud_22 = transform.Find("Clouds/cloud_22").gameObject;    
    //--------------------------------------------------------------
    clouds_xpos         = new float[MAX_CLOUDS];
    clouds_ypos         = new float[MAX_CLOUDS];
    clouds_zpos         = new float[MAX_CLOUDS];
    clouds_scale        = new float[MAX_CLOUDS];
    clouds_yrot         = new float[MAX_CLOUDS];
    clouds_obj          = new GameObject[MAX_CLOUDS];
    //--------------------------------------------------------------
    time_msecs      = 0.0f;
    time_seconds    = 0;
    //--------------------------------------------------------------
    sun = transform.Find("Sun").gameObject;    
    sun_light = transform.Find("Sun/SunLight").light;

    if(using_unity_pro == false)
        sun_light.shadows = LightShadows.None;    
    //--------------------------------------------------------------
    moon = transform.Find("Moon").gameObject;    
    moon_light = transform.Find("Moon/MoonPlane/MoonLight").light;
    moon_plane = transform.Find("Moon/MoonPlane").gameObject;
    
    if(moon_shadows == false)
        moon_light.shadows = LightShadows.None;    
    //--------------------------------------------------------------
    sky = transform.Find("Skydome").gameObject;
    //--------------------------------------------------------------
    sun_flare = transform.Find("Sun/SunLight/SunFlare").gameObject;
    //--------------------------------------------------------------
    sky_renderer       = sky.GetComponent<Renderer>();
    sunflare_renderer  = sun_flare.GetComponent<Renderer>();
    moonplane_renderer = moon_plane.GetComponent<Renderer>();

    cloud_material = cloud_01.renderer.sharedMaterial;

    cloudcam = transform.Find("CloudCam").camera;
    cloudcam.enabled = using_unity_pro; // only active when running under unityPro

    //-- init sky gradient
    int i;
    for(i=0;i<16;i++)
        gradient.SetPixels(i,0,1,512, gradient_source.GetPixels(0, 0, 1, 512));
    gradient.Apply();
    last_frame_sky_gradient = 1;    

    //-- init contrails
    contrails_creator_active = new bool [MAX_CONTRAIL_CREATORS];
    contrails_creator_xpos   = new float[MAX_CONTRAIL_CREATORS];
    contrails_creator_ypos   = new float[MAX_CONTRAIL_CREATORS];
    contrails_creator_zpos   = new float[MAX_CONTRAIL_CREATORS];
    contrails_creator_xadd   = new float[MAX_CONTRAIL_CREATORS];
    contrails_creator_zadd   = new float[MAX_CONTRAIL_CREATORS];
    contrails_creator_yrot   = new float[MAX_CONTRAIL_CREATORS];
    contrails_creator_counter= new float[MAX_CONTRAIL_CREATORS];

    contrails_lifetime       = new float[MAX_CONTRAIL_PLANES];
    contrails_object         = new GameObject[MAX_CONTRAIL_PLANES];    

    contrail_obj = transform.Find("Clouds/contrail").gameObject;
    contrail_material = contrail_obj.renderer.sharedMaterial;
        
    star_01 = transform.Find("Stars/star_01").gameObject;
    star_02 = transform.Find("Stars/star_02").gameObject;
    star_03 = transform.Find("Stars/star_03").gameObject;
    star_04 = transform.Find("Stars/star_04").gameObject;
    star_05 = transform.Find("Stars/star_05").gameObject;
    star_06 = transform.Find("Stars/star_06").gameObject;
    star_07 = transform.Find("Stars/star_07").gameObject;
    star_08 = transform.Find("Stars/star_08").gameObject;

    stars_material = star_01.renderer.sharedMaterial;
}
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
//
//  Start
//
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
void Start()
{
    int i;
    //-------------------------------------------------------------------
    //-------------------------------------------------------------------
    //-------------------------------------------------------------------
    //
    // Init Clouds
    //
    cloud_scale_factor = 1.0f;

    for(i=0;i<MAX_CLOUDS;i++)
    {
        clouds_xpos[i]  = (float)((UnityEngine.Random.value) * 15000.0f);
        clouds_ypos[i]  = (float)(((UnityEngine.Random.value) * 300.0f) + 300.0f);
        clouds_zpos[i]  = (float)((UnityEngine.Random.value) * 15000.0f);
        clouds_scale[i] = (float)(((UnityEngine.Random.value) * 30.0f) + 25.0f);
        clouds_yrot[i]  = (float)((UnityEngine.Random.value) * 360.0f);

        switch( (int)((UnityEngine.Random.value) * 22) )
        {
            case 0: {clouds_obj[i] = (GameObject)Instantiate(cloud_01);};break;
            case 1: {clouds_obj[i] = (GameObject)Instantiate(cloud_02);};break;
            case 2: {clouds_obj[i] = (GameObject)Instantiate(cloud_03);};break;
            case 3: {clouds_obj[i] = (GameObject)Instantiate(cloud_04);};break;
            case 4: {clouds_obj[i] = (GameObject)Instantiate(cloud_05);};break;
            case 5: {clouds_obj[i] = (GameObject)Instantiate(cloud_06);};break;
            case 6: {clouds_obj[i] = (GameObject)Instantiate(cloud_07);};break;
            case 7: {clouds_obj[i] = (GameObject)Instantiate(cloud_08);};break;
            case 8: {clouds_obj[i] = (GameObject)Instantiate(cloud_09);};break;
            case 9: {clouds_obj[i] = (GameObject)Instantiate(cloud_10);};break;
            case 10:{clouds_obj[i] = (GameObject)Instantiate(cloud_11);};break;
            case 11:{clouds_obj[i] = (GameObject)Instantiate(cloud_12);};break;
            case 12:{clouds_obj[i] = (GameObject)Instantiate(cloud_13);};break;
            case 13:{clouds_obj[i] = (GameObject)Instantiate(cloud_14);};break;
            case 14:{clouds_obj[i] = (GameObject)Instantiate(cloud_15);};break;
            case 15:{clouds_obj[i] = (GameObject)Instantiate(cloud_16);};break;
            case 16:{clouds_obj[i] = (GameObject)Instantiate(cloud_17);};break;
            case 17:{clouds_obj[i] = (GameObject)Instantiate(cloud_18);};break;
            case 18:{clouds_obj[i] = (GameObject)Instantiate(cloud_19);};break;
            case 19:{clouds_obj[i] = (GameObject)Instantiate(cloud_20);};break;
            case 20:{clouds_obj[i] = (GameObject)Instantiate(cloud_21);};break;
            default:{clouds_obj[i] = (GameObject)Instantiate(cloud_22);};break;
        }
        clouds_obj[i].transform.parent = gameObject.transform;
        clouds_obj[i].transform.eulerAngles = new Vector3(0.0f, clouds_yrot[i], 0.0f);
        clouds_obj[i].transform.localScale  = new Vector3(clouds_scale[i] * cloud_scale_factor, 1.0f, clouds_scale[i] * cloud_scale_factor);
    }    
    //-------------------------------------------------------------------
    //-------------------------------------------------------------------
    //-------------------------------------------------------------------
    //
    // Init weather change
    //
    weather_change_mode = 0;
    weather_change_counter = 0.0f;

    //-------------------------------------------------------------------
    //-------------------------------------------------------------------
    //-------------------------------------------------------------------
    //
    // Init contrails
    //
    for(i=0;i<MAX_CONTRAIL_CREATORS;i++)
        contrails_creator_active[i] = false;    

    contrail_planes_active = 0;

    //-------------------------------------------------------------------
    //-------------------------------------------------------------------
    //-------------------------------------------------------------------
    //
    // Init stars
    //
    if(stars_active == true)
    {
        for(i=0;i<MAX_STARS;i++)
        {
            GameObject star;
            switch( (int)((UnityEngine.Random.value) * 8) )
            {
                case 0:  {star = (GameObject)Instantiate(star_01);};break;
                case 1:  {star = (GameObject)Instantiate(star_02);};break;
                case 2:  {star = (GameObject)Instantiate(star_03);};break;
                case 3:  {star = (GameObject)Instantiate(star_04);};break;
                case 4:  {star = (GameObject)Instantiate(star_05);};break;
                case 5:  {star = (GameObject)Instantiate(star_06);};break;
                case 6:  {star = (GameObject)Instantiate(star_07);};break;
                default: {star = (GameObject)Instantiate(star_08);};break;
            }
            float xpos = (float)(((UnityEngine.Random.value) * 14000.0f) - 7000.0f);
            float zpos = (float)(((UnityEngine.Random.value) * 14000.0f) - 7000.0f);
            star.transform.parent      = gameObject.transform;
            star.transform.position    = new Vector3 (xpos, 800.0f, zpos);
            star.transform.localScale  = new Vector3 (10.0f, 10.0f, 10.0f);
            star.transform.eulerAngles = new Vector3 (0.0f, 0.0f, 0.0f); 
            star.transform.LookAt(gameObject.transform);
        }
    }

}
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
//
//  Update
//
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
void Update()
{
    int i;
    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //
    // Update Time
    //
    
    if(time_minutes >= 60)
        time_minutes = 59;
    if(time_minutes < 0)
        time_minutes = 0;

    if(time_hour >= 24)
        time_hour = 23;
    if(time_hour < 0)
        time_hour = 0;

    time_msecs += Time.deltaTime * time_speed;
    if(time_msecs >= 1.0f)
    {
        while(time_msecs >= 1.0f)
        {
            time_msecs -= 1.0f;
            time_seconds++;
        }
        if(time_seconds >= 60)
        {
            while(time_seconds >= 60)
            {
                time_seconds -= 60;
                time_minutes++;
            }

            if(time_minutes >= 60)
            {
                while(time_minutes >= 60)
                {
                    time_minutes -= 60;
                    time_hour++;
                }
                if(time_hour >= 24)
                {
                    while(time_hour >= 24)
                        time_hour -= 24;
               }
            }
        }
    }
    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //
    // Update Contrails
    //
    if(contrails_active == true)
    { 
        //-- move active contrail creators
        for(i=0;i<MAX_CONTRAIL_CREATORS;i++)
        {
            if(contrails_creator_active[i] == true)
            {    
                contrails_creator_xpos[i] += (contrails_creator_xadd[i] * Time.deltaTime);        
                contrails_creator_zpos[i] += (contrails_creator_zadd[i] * Time.deltaTime);
        
                if( (contrails_creator_xpos[i] <= -7500.0f) | (contrails_creator_xpos[i] >= 7500.0f) |
                    (contrails_creator_zpos[i] <= -7500.0f) | (contrails_creator_zpos[i] >= 7500.0f) )
                {    
                    contrails_creator_active[i] = false; 
                }
                else
                {
                    contrails_creator_counter[i] += Time.deltaTime * 4.0f;
                    if(contrails_creator_counter[i] >= 1.0f)
                    {
                        contrails_creator_counter[i] = 0.0f;

                        if(contrail_planes_active < MAX_CONTRAIL_PLANES)
                        {
                            contrails_object[contrail_planes_active] = (GameObject)Instantiate(contrail_obj);
                            contrails_object[contrail_planes_active].transform.parent = gameObject.transform;
                            
                            contrails_object[contrail_planes_active].transform.position         = new Vector3 (contrails_creator_xpos[i], contrails_creator_ypos[i], contrails_creator_zpos[i]);
                            contrails_object[contrail_planes_active].transform.eulerAngles = new Vector3 (0.0f, contrails_creator_yrot[i] - 90.0f, 0.0f);
                            
                            float contrail_size = (float)(((UnityEngine.Random.value) * 3.0f) + 5.0f);
                            contrails_object[contrail_planes_active].transform.localScale = new Vector3 (contrail_size, contrail_size, contrail_size);

                            contrails_lifetime[contrail_planes_active] = (float)(((UnityEngine.Random.value) * 80.0f) + 120.0f);

                            contrail_planes_active++;
                        }
                    }
                }
            }
            else //-- create new one 
            {
                if( (time_hour >= 6) && (time_hour < 19) )
                {
                    if( ((UnityEngine.Random.value) * 10.0f) <= 5.0f )
                    {
                        contrails_creator_active[i] = true;
                        contrails_creator_ypos[i] = (float)(((UnityEngine.Random.value) * 400.0f) + 800.0f);
                        
                        int direction = (int)((UnityEngine.Random.value) * 4);
                        switch(direction)
                        {
                            //-- spawn at z min
                            case 0:
                            {
                                contrails_creator_zpos[i] = -7450.0f;        
                                contrails_creator_xpos[i] = (float)(((UnityEngine.Random.value) * 8000.0f) - 4000.0f);

                                contrails_creator_yrot[i] = (float)((UnityEngine.Random.value) * 360.0f);
                                
                                contrails_creator_xadd[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * contrails_creator_yrot[i])) * 120.0f;
                                contrails_creator_zadd[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * contrails_creator_yrot[i])) * 120.0f;

                            };break;

                            //-- spawn at z max
                            case 1:
                            {
                                contrails_creator_zpos[i] = 7450.0f;        
                                contrails_creator_xpos[i] = (float)(((UnityEngine.Random.value) * 8000.0f) - 4000.0f);

                                contrails_creator_yrot[i] = (float)((UnityEngine.Random.value) * 360.0f);

                                contrails_creator_xadd[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * contrails_creator_yrot[i])) * 120.0f;
                                contrails_creator_zadd[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * contrails_creator_yrot[i])) * 120.0f;

                            };break;

                            //-- spawn at x min
                            case 2:
                            {
                                contrails_creator_xpos[i] = -7450.0f;        
                                contrails_creator_zpos[i] = (float)(((UnityEngine.Random.value) * 8000.0f) - 4000.0f);

                                contrails_creator_yrot[i] = (float)((UnityEngine.Random.value) * 360.0f);

                                contrails_creator_xadd[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * contrails_creator_yrot[i])) * 120.0f;
                                contrails_creator_zadd[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * contrails_creator_yrot[i])) * 120.0f;

                            };break;

                            //-- spawn at x max
                            default:
                            {
                                contrails_creator_xpos[i] = 7450.0f;        
                                contrails_creator_zpos[i] = (float)(((UnityEngine.Random.value) * 8000.0f) - 4000.0f);

                                contrails_creator_yrot[i] = (float)((UnityEngine.Random.value) * 360.0f);

                                contrails_creator_xadd[i] = (float)(Math.Sin( ((2 * Math.PI) / 360.0f) * contrails_creator_yrot[i])) * 120.0f;
                                contrails_creator_zadd[i] = (float)(Math.Cos( ((2 * Math.PI) / 360.0f) * contrails_creator_yrot[i])) * 120.0f;

                            };break;
                        }
                    }
                }
            }
        }
        //-- decrease contrail planes lifetime and delete destroyed planes
        if(contrail_planes_active >= 1)      
        {
            i = 0;
            bool done = false;
            while(done == false)
            {
                float contrail_size = contrails_object[i].transform.localScale.x;
                contrail_size += Time.deltaTime * ((UnityEngine.Random.value) * 4.0f);
                contrails_object[i].transform.localScale = new Vector3 (contrail_size, contrail_size, contrail_size);

                contrails_lifetime[i] -= Time.deltaTime * 5.0f;
                if(contrails_lifetime[i] < 0.0f)
                {
                    Destroy(contrails_object[i]);

                    if(i != (contrail_planes_active - 1) )
                    {
                        contrails_lifetime[i] = contrails_lifetime[contrail_planes_active - 1];  
                        contrails_object[i]   = contrails_object[contrail_planes_active - 1];  
                        contrail_planes_active--;
                    }
                    else
                    {
                        contrail_planes_active--;
                        done = true;
                    }
                }
                else
                {
                    i++;
                    if(i >= contrail_planes_active)
                    {
                        done = true;
                    }
                }
            }
        }
    }
    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //
    // Update Clouds
    //
    if(dynamic_weather_change == true)
    {    
        for(i=0;i<MAX_CLOUDS;i++)
        {
            clouds_xpos[i] = (clouds_xpos[i] + wind_xspeed * Time.deltaTime) % 15000.0f;
            clouds_zpos[i] = (clouds_zpos[i] + wind_zspeed * Time.deltaTime) % 15000.0f;

            clouds_obj[i].transform.position    = new Vector3(clouds_xpos[i] - 7500.0f, clouds_ypos[i], clouds_zpos[i] - 7500.0f);
            clouds_obj[i].transform.localScale = new Vector3(clouds_scale[i] * cloud_scale_factor, 1.0f, clouds_scale[i] * cloud_scale_factor); 
        }
    }
    else
    {    
        for(i=0;i<MAX_CLOUDS;i++)
        {
            clouds_xpos[i] = (clouds_xpos[i] + wind_xspeed * Time.deltaTime) % 15000.0f;
            clouds_zpos[i] = (clouds_zpos[i] + wind_zspeed * Time.deltaTime) % 15000.0f;

            clouds_obj[i].transform.position    = new Vector3(clouds_xpos[i] - 7500.0f, clouds_ypos[i], clouds_zpos[i] - 7500.0f);
        }
    }

    if(dynamic_weather_change == true)
    {
        if(weather_change_mode == 0)           
        {
            weather_change_counter -= Time.deltaTime * 1.0f;
            if(weather_change_counter < 0.0f)
            {
                weather_change_mode = 1;
                weather_change_counter = 0.0f;
                cloud_dest_scale_factor = (float)(((UnityEngine.Random.value) * 0.8f) + 0.2f);
            }
        }
        if(weather_change_mode == 1)
        {
            weather_change_counter += Time.deltaTime * 1.0f;
            if(weather_change_counter >= 2.0f)
            {
                if( (Math.Abs(cloud_dest_scale_factor - cloud_scale_factor) <= 0.05f) )
                {
                    weather_change_mode = 0;
                    weather_change_counter = (float)(((UnityEngine.Random.value) * 20.0f) + 10.0f);
                }
                //-- 
                else if(cloud_scale_factor < cloud_dest_scale_factor)
                    cloud_scale_factor += Time.deltaTime * 0.01f;               
                //-- delete clouds
                else 
                    cloud_scale_factor -= Time.deltaTime * 0.01f;               
            }
        }
    }

    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //
    // Set Cloud Camera and calculate Cloud Density
    //
    float cloud_density;
    if(using_unity_pro == true)
    {
        cloudcam.transform.position = viewport_center_obj.transform.position;

        if((time_hour <= 6) | (time_hour >= 22))
            cloudcam.transform.LookAt(moon_light.transform);
        else
            cloudcam.transform.LookAt(sun_light.transform);
        
        cloud_material.SetColor("_TintColor", new Color(0.83f , 0.83f, 0.83f, 0.25f));

        cloudcam.Render();
        
        cloud_density = (cloud_texture2d.GetPixel(6, 6).grayscale + 
                         cloud_texture2d.GetPixel(7, 6).grayscale + 
                         cloud_texture2d.GetPixel(8, 6).grayscale + 
                         cloud_texture2d.GetPixel(9, 6).grayscale + 
                         cloud_texture2d.GetPixel(6, 7).grayscale + 
                         cloud_texture2d.GetPixel(7, 7).grayscale + 
                         cloud_texture2d.GetPixel(8, 7).grayscale + 
                         cloud_texture2d.GetPixel(9, 7).grayscale + 
                         cloud_texture2d.GetPixel(6, 8).grayscale + 
                         cloud_texture2d.GetPixel(7, 8).grayscale + 
                         cloud_texture2d.GetPixel(8, 8).grayscale + 
                         cloud_texture2d.GetPixel(9, 8).grayscale + 
                         cloud_texture2d.GetPixel(6, 9).grayscale + 
                         cloud_texture2d.GetPixel(7, 9).grayscale + 
                         cloud_texture2d.GetPixel(8, 9).grayscale + 
                         cloud_texture2d.GetPixel(9, 9).grayscale) / 16.0f;
    }
    else
    {
        if(MAX_CLOUDS <= 300)
            cloud_density = 0.0f;
        else if(MAX_CLOUDS >= 2300)
            cloud_density = 0.99999f;
        else
            cloud_density = (float)((MAX_CLOUDS - 300.0f) / 2000.0f);
    }
    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //
    // Update Sky Gradient
    //

    int sky_gradient = (int)(cloud_density * 512);
    if(sky_gradient != last_frame_sky_gradient)
    {
        for(i=0;i<16;i++)
            gradient.SetPixels(i,0,1,512, gradient_source.GetPixels(sky_gradient, 0, 1, 512));
        gradient.Apply();
    }
  
    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //
    // Update Sky
    //

    //-- day only
    if((time_hour >= 5) && (time_hour < 23))
    {
        //-- set sun position and angle
        float sun_angle = (float) (-5.0f + ( ((180.0f + 10.0f) / ((22-6)*60*60)) * (((time_hour - 6)*60*60) + (time_minutes * 60) + time_seconds) ));
        sun.transform.localEulerAngles = new Vector3(sun_angle, 0.0f, 0.0f);
    }

    //-- night only
    if((time_hour >= 18) | (time_hour < 6))
    {
        //-- set moon position and angle   
        int moon_hour;
        if(time_hour >= 18)
            moon_hour = time_hour - 18;
        else
            moon_hour = time_hour + 6;

        float moon_angle = (float) (0.0f - ( ((180.0f) / ((12)*60*60)) * ((moon_hour*60*60) + (time_minutes * 60) + time_seconds) ));
        moon.transform.localEulerAngles = new Vector3(0.0f, moon_angle, 0.0f);
        
        float moon_height = (float) (Mathf.Sin(((2 * Mathf.PI) / 360) * moon_angle) * -1500.0f);
        moon.transform.localPosition = new Vector3(0.0f, moon_height + 1500.0f, 0.0f);
    }

    //-- sun brightness
    int table_index1;
    int table_index2;
    float scale;

    table_index1 = (time_hour * 2);
    if(time_minutes >= 30)
        table_index1++;
    table_index2 = (table_index1 + 1) % 48;
    
    scale = (float) (((time_minutes % 30) * 60) + time_seconds) / (30*60);

    //-- set sunlight intensity
    sun_light.intensity = ((sun_intensity_table[table_index1] * (1.0f - scale)) + (sun_intensity_table[table_index2] * scale) ) * (1.1f - (cloud_density * 1.1f));

    //-- enabled / disble light
    if(sun_light.intensity == 0.0f)
    {
        if(sun_light.light.enabled == true)
            sun_light.light.enabled = false;
    }
    else
    {
        if(sun_light.light.enabled == false)
            sun_light.light.enabled = true;
    }

    //-- set sunlight shadow intensity
    sun_light.shadowStrength = ((sun_shadow_intensity_table[table_index1] * (1.0f - scale)) + (sun_shadow_intensity_table[table_index2] * scale) ) * (1.0f - cloud_density) * 0.86f;

    //-- sun color
    float color_r1 = ((float)sun_color_table[table_index1 * 3 + 0] / 256.0f);
    float color_g1 = ((float)sun_color_table[table_index1 * 3 + 1] / 256.0f);
    float color_b1 = ((float)sun_color_table[table_index1 * 3 + 2] / 256.0f);

    float color_r2 = ((float)sun_color_table[table_index2 * 3 + 0] / 256.0f);
    float color_g2 = ((float)sun_color_table[table_index2 * 3 + 1] / 256.0f);
    float color_b2 = ((float)sun_color_table[table_index2 * 3 + 2] / 256.0f);

    float color_r = ((1.0f - scale) * color_r1) + (color_r2 * scale);
    float color_g = ((1.0f - scale) * color_g1) + (color_g2 * scale);
    float color_b = ((1.0f - scale) * color_b1) + (color_b2 * scale);

    sun_light.color = new Color (color_r, color_g, color_b);

    //-- sun flare color and intensity
    color_r1 = ((float)sun_flare_color_table[table_index1 * 3 + 0] / 256.0f);
    color_g1 = ((float)sun_flare_color_table[table_index1 * 3 + 1] / 256.0f);
    color_b1 = ((float)sun_flare_color_table[table_index1 * 3 + 2] / 256.0f);

    color_r2 = ((float)sun_flare_color_table[table_index2 * 3 + 0] / 256.0f);
    color_g2 = ((float)sun_flare_color_table[table_index2 * 3 + 1] / 256.0f);
    color_b2 = ((float)sun_flare_color_table[table_index2 * 3 + 2] / 256.0f);

    color_r = ((1.0f - scale) * color_r1) + (color_r2 * scale);
    color_g = ((1.0f - scale) * color_g1) + (color_g2 * scale);
    color_b = ((1.0f - scale) * color_b1) + (color_b2 * scale);
    
    float flare_intensity = ((sun_flare_intensity_table[table_index1] * (1.0f - scale)) + (sun_flare_intensity_table[table_index2] * scale) ) * 0.45f;
    
    sunflare_renderer.material.SetColor("_TintColor", new Color(color_r, color_g, color_b, flare_intensity * (1.0f - cloud_density)));

    float size = ((sun_flare_scale_table[table_index1] * (1.0f - scale)) + (sun_flare_scale_table[table_index2] * scale) ) * 1500.0f * (1.0f - (cloud_density / 2.0f) );
    sun_flare.transform.localScale = new Vector3 (size, size, size);

    //-- change sky color
    color_r1 = ((float)sky_color_table[table_index1 * 3 + 0] / 256.0f);
    color_g1 = ((float)sky_color_table[table_index1 * 3 + 1] / 256.0f);
    color_b1 = ((float)sky_color_table[table_index1 * 3 + 2] / 256.0f);

    color_r2 = ((float)sky_color_table[table_index2 * 3 + 0] / 256.0f);
    color_g2 = ((float)sky_color_table[table_index2 * 3 + 1] / 256.0f);
    color_b2 = ((float)sky_color_table[table_index2 * 3 + 2] / 256.0f);

    color_r = ((1.0f - scale) * color_r1) + (color_r2 * scale);
    color_g = ((1.0f - scale) * color_g1) + (color_g2 * scale);
    color_b = ((1.0f - scale) * color_b1) + (color_b2 * scale);
    
    sky_renderer.material.SetColor("_Color", new Color(color_r , color_g, color_b));

    //-- change cloud color
    cloud_material.SetColor("_TintColor", new Color(color_r * (1.0f - (cloud_density / 2.0f)  ), color_g * (1.0f - (cloud_density / 2.0f)), color_b * (1.0f - (cloud_density / 2.0f)), 0.25f * (1.0f - (cloud_density / 2.0f))));

    //-- chaneg contrail material
    contrail_material.SetColor("_TintColor", new Color(color_r * (0.25f - (cloud_density / 8.0f)  ), color_g * (0.25f - (cloud_density / 8.0f)), color_b * (0.25f - (cloud_density / 8.0f)), 0.25f * (0.25f - (cloud_density / 8.0f))));

    //-- set ambient color and intensity
    color_r1 = ((float)ambient_color_table[table_index1 * 3 + 0] / 256.0f);
    color_g1 = ((float)ambient_color_table[table_index1 * 3 + 1] / 256.0f);
    color_b1 = ((float)ambient_color_table[table_index1 * 3 + 2] / 256.0f);

    color_r2 = ((float)ambient_color_table[table_index2 * 3 + 0] / 256.0f);
    color_g2 = ((float)ambient_color_table[table_index2 * 3 + 1] / 256.0f);
    color_b2 = ((float)ambient_color_table[table_index2 * 3 + 2] / 256.0f);

    color_r = ((1.0f - scale) * color_r1) + (color_r2 * scale);
    color_g = ((1.0f - scale) * color_g1) + (color_g2 * scale);
    color_b = ((1.0f - scale) * color_b1) + (color_b2 * scale);
    
    float ambient_intensity = ((ambient_intensity_table[table_index1] * (1.0f - scale)) + (ambient_intensity_table[table_index2] * scale) );
    RenderSettings.ambientLight = new Color(color_r * ((cloud_density / 2.0f) + ambient_intensity), color_g * ((cloud_density / 2.0f) + ambient_intensity), color_b * ((cloud_density / 2.0f) + ambient_intensity));

    //-- set sunlight intensity
    moon_light.intensity = ((moon_intensity_table[table_index1] * (1.0f - scale)) + (moon_intensity_table[table_index2] * scale) ) * 0.95f * (1.0f - (cloud_density * 1.0f));

    //-- set sunlight shadow intensity
    moon_light.shadowStrength = ((moon_shadow_intensity_table[table_index1] * (1.0f - scale)) + (moon_shadow_intensity_table[table_index2] * scale) ) * 0.4f * (1.0f - cloud_density);

    //-- set moon plane brightness
    float moon_intensity = ((moon_plane_intensity_table[table_index1] * (1.0f - scale)) + (moon_plane_intensity_table[table_index2] * scale) ) * 0.4f * (1.0f - (cloud_density * 1.0f));
    moonplane_renderer.material.SetColor("_TintColor", new Color(0.8f, 0.8f, 0.8f, moon_intensity));

    //-- chaneg contrail material
    float stars_intensity = ((stars_intensity_table[table_index1] * (1.0f - scale)) + (stars_intensity_table[table_index2] * scale) ) * (1.0f - (cloud_density * 1.0f));
    stars_material.SetColor("_TintColor", new Color(1.0f, 1.0f, 1.0f, stars_intensity));

    //-- enabled / disble light
    if(moon_light.intensity == 0.0f)
    {
        if(moon_light.light.enabled == true)
            moon_light.light.enabled = false;
    }
    else
    {
        if(moon_light.light.enabled == false)
            moon_light.light.enabled = true;
    }
    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //------------------------------------------------------------------

}
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------
}
