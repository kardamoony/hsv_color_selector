Shader "Kardamoony/URP/Simple Toon"
{
    Properties
    {
        _BaseMap ("Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _AmbientColor ("Ambient Color", Color) = (0.2, 0.2, 0.2, 1)
        _ShadowStrength ("Shadow Strength", Range(0, 1)) = 1
        _ShadowSmoothness ("Shadow Smoothness", float) = 0.5
    }
    SubShader
    {
        Tags 
        {
            "Queue" = "Geometry"
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            ZWrite On
            Cull Back
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct Varyings
            {
                float4 pos : SV_POSITION;
                half2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
                half4 _AmbientColor;
                half _ShadowStrength;
                half _ShadowSmoothness;
            CBUFFER_END

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                
                OUT.pos = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.worldNormal = TransformObjectToWorldNormal(IN.normal);
                
                return OUT;
            }

            half4 toon_lighting_diffuse(Varyings IN)
            {
                float NdotL = dot(_MainLightPosition.xyz, IN.worldNormal);
                half diff = step(0, NdotL);
                //half diff = smoothstep(0, 1, NdotL * _ShadowSmoothness);
                half4 lighting = lerp(_AmbientColor, _MainLightColor, diff);
                
                return lighting;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor * toon_lighting_diffuse(IN);
                return col;
            }
            
            ENDHLSL
        }
    }
}
