Shader "Emerald AI/Standard Outline" {
    Properties {
		_MainTexColor ("Base Color", Color) = (1.0,1.0,1.0,1.0)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _MainBump ("MainBump", 2D) = "bump" {}
		_Smoothness ("Smoothness", Range (0, 1) ) = 0.25
		_Occlusion("Occlusion", 2D)= "white" {}

		_OutlineColor("Outline color", Color) = (0,0,0,1)
		_OutlineWidth("Outlines width", Range(0.0, 2.0)) = 0.15

		//_DamageFlash("Flash", Range(0.0, 0.5)) = 0.0
    }

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
	};

	struct v2f
	{
		float4 pos : POSITION;
	};

	uniform float _OutlineWidth;
	uniform float4 _OutlineColor;
	uniform int _OutlineEnabled;
	uniform float _DamageFlash;

	ENDCG

    SubShader 
	{
        Tags { "RenderType"="Opaque"  "Queue" = "AlphaTest" }

		Pass //Outline
		{
			ZWrite Off
			Cull Back
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			v2f vert(appdata v)
			{
				appdata original = v;

				if (_OutlineEnabled == 1)
				{
					v.vertex.xyz += _OutlineWidth * normalize(v.vertex.xyz);
				}

				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;

			}

			half4 frag(v2f i) : COLOR
			{
				return _OutlineColor;
			}

			ENDCG
		}

		CGINCLUDE
		#define _GLOSSYENV 1
		#define UNITY_SETUP_BRDF_INPUT MetallicSetup
		ENDCG

		Tags{ "Queue" = "Geometry"}
       
        CGPROGRAM
        #pragma target 4.0
		#include "UnityPBSLighting.cginc"
        #pragma surface surf Standard
 
		fixed4 _MainTexColor;
        sampler2D _MainTex;
        sampler2D _MainBump;
		sampler2D _Occlusion;
		float _Smoothness;
		float _Scale;
 
        struct Input {
            float2 uv_MainTex;
            float2 uv_MainBump;
			float2 uv_Occlusion;
            float3 worldNormal;
			float4 screenPos;
			float3 worldPos;
			half4 color : COLOR;
            INTERNAL_DATA
        };
 
        void surf (Input IN, inout SurfaceOutputStandard o) 
		{
            half4 MainDiffuse = tex2D(_MainTex, IN.uv_MainTex) * _MainTexColor + _DamageFlash;
			half3 Occlusion = tex2D(_Occlusion, IN.uv_Occlusion);

			o.Albedo = MainDiffuse;
			o.Normal = UnpackNormal(tex2D(_MainBump, IN.uv_MainBump));			
			o.Smoothness = _Smoothness;
			o.Occlusion = Occlusion.rgb;
            o.Alpha = MainDiffuse.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}