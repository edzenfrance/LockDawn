Shader "VIS/Enamel" 
{
    Properties 
    {
        _Color          ("Main Color", Color) = (1,1,1,1)
        _MainTex        ("Base Texture (RGB)", 2D) = "white" {} 
        _CubeTex        ("Reflection Cubemap", CUBE) = "" {}
        _BlendFactor    ("Blend Factor", Range(0.0, 1.0)) = 0.5
        _FresnelPower   ("Fresnel Power", Range(0.05, 5.0)) = 0.75
        _EmissionFactor ("Emission Factor", Range(0.0, 1.0)) = 0.5
    }
    
    SubShader 
    {
        Tags { "Queue"="geometry" "RenderType"="opaque" }    
        
        CGPROGRAM
        
        #pragma surface surf BlinnPhong
        #pragma target 3.0

        sampler2D   _MainTex;
        samplerCUBE _CubeTex;

        float4 _Color;
        float  _BlendFactor;
        float  _FresnelPower;
        float  _EmissionFactor;

        struct Input 
        {
            float2 uv_MainTex;
            float3 worldRefl;
            float3 viewDir;
            
            INTERNAL_DATA
        };

        void surf(Input IN, inout SurfaceOutput o) 
        {
            float4 color = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            float4 refl  = texCUBE(_CubeTex, IN.worldRefl);
	        
            float bias   = 0.20373;
            float facing = saturate(1.0 - max(dot(normalize(IN.viewDir.xyz), normalize(o.Normal)), 0.0));
            float factor = max(bias + (1.0 - bias) * pow(facing, _FresnelPower), 0.0);
            
            o.Albedo   = _BlendFactor * refl.rgb * factor + (1.0 - _BlendFactor) * color.rgb;
            o.Emission = _EmissionFactor * o.Albedo;
            o.Alpha    = 1.0;
        }

        ENDCG
    }
        
    FallBack "Reflective/VertexLit"
}
