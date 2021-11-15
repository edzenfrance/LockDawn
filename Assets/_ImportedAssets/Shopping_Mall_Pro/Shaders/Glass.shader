Shader "VIS/Glass" 
{
    Properties 
    {
        _Color          ("Main Color", Color) = (1,1,1,1)
        _ReflectColor   ("Reflection Color", Color) = (1,1,1,0.5)
        _MainTex        ("Base (RGB) RefStrength (A)", 2D) = "white" {} 
        _Cube           ("Cubemap", CUBE) = "" {}
        _FresnelPower   ("_FresnelPower", Range(0.05,5.0)) = 0.75
    }
    
    SubShader 
    {
        Tags { "Queue"="transparent" "RenderType"="Transparent" }
        
        CGPROGRAM
        
        #pragma surface surf BlinnPhong alpha
        #pragma target 3.0

        sampler2D   _MainTex;
        samplerCUBE _RtReflection;
        samplerCUBE _Cube;

        float4 _Color;
        float4 _ReflectColor;
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
            half4  color = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            half4  refl  = texCUBE(_Cube, IN.worldRefl);

            float bias   = 0.20373;
            float facing = saturate(1.0 - max(dot(normalize(IN.viewDir.xyz), normalize(o.Normal)), 0.0));
            
            o.Albedo   = refl.rgb * _ReflectColor.rgb + color.rgb;
            o.Emission = o.Albedo * 0.25;
            o.Alpha    = max(bias + (1.0 - bias) * pow(facing, _FresnelPower), 0.0);
        }

        ENDCG
    }
        
    FallBack "Reflective/VertexLit"
}
