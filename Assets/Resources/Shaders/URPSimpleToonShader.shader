Shader "Kardamoony/URP/Simple Toon"
{
    Properties
    {
        _BaseMap ("Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _RimColor ("Rim Color", Color) = (1, 1, 1, 1)
        _RimPower ("Rim Power", float) = 3
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
            
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma shader_feature _ALPHATEST_ON
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
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
                float3 worldPos : TEXCOORD2;
                float4 shadowCoord : TEXCOORD3;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
                half4 _AmbientColor;
                half4 _RimColor;
                half _RimPower;
                half _ShadowStrength;
                half _ShadowSmoothness;
            CBUFFER_END

            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                OUT.pos = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.worldNormal = TransformObjectToWorldNormal(IN.normal);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.shadowCoord = GetShadowCoord(vertexInput);
                OUT.worldPos = vertexInput.positionWS;
    
                return OUT;
            }

            half4 toon_lighting_diffuse(Varyings IN)
            {
                Light mainLight = GetMainLight(IN.shadowCoord);

                half shadowAttenuation = smoothstep(0, 1, mainLight.shadowAttenuation);
                
                float NdotL = dot(_MainLightPosition.xyz, IN.worldNormal);
                half diff = saturate(NdotL);//smoothstep(0, 1, NdotL);
        
                float3 viewDir = 1 - normalize(_WorldSpaceCameraPos - IN.worldPos.xyz);
                half fresnel = pow(saturate(dot(viewDir, IN.worldNormal)), _RimPower);
                //fresnel = smoothstep(0, 0.05,  fresnel);

                half4 lighting = lerp(_MainLightColor, _RimColor, fresnel);
                lighting = lerp(_AmbientColor, lighting, (diff + fresnel) * shadowAttenuation);
                
                return lighting;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor * toon_lighting_diffuse(IN);
                return col;
            }
            
            ENDHLSL
        }
        
        Pass
    {
        Name "ShadowCaster"

        Tags {"LightMode" = "ShadowCaster"}

            Cull Back

            HLSLPROGRAM

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
           
            #pragma shader_feature _ALPHATEST_ON

           // GPU Instancing
            #pragma multi_compile_instancing
          
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"


             CBUFFER_START(UnityPerMaterial)
                half4 _TintColor;
                sampler2D _MainTex;
                float4 _MainTex_ST;
                float   _Alpha;
             CBUFFER_END

            struct VertexInput
            {         
                float4 vertex : POSITION;
                float4 normal : NORMAL;
           
                #if _ALPHATEST_ON
                float2 uv     : TEXCOORD0;
                #endif

                UNITY_VERTEX_INPUT_INSTANCE_ID 
            };
         
            struct VertexOutput
            {         
                float4 vertex : SV_POSITION;
                #if _ALPHATEST_ON
                float2 uv     : TEXCOORD0;
                #endif
                UNITY_VERTEX_INPUT_INSTANCE_ID         
                UNITY_VERTEX_OUTPUT_STEREO
            };

            VertexOutput ShadowPassVertex(VertexInput v)
            {
                VertexOutput o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);                            
          
                float3 positionWS = TransformObjectToWorld(v.vertex.xyz);
                float3 normalWS   = TransformObjectToWorldNormal(v.normal.xyz);
        
                float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _MainLightPosition.xyz));
             
                o.vertex = positionCS;
                #if _ALPHATEST_ON
                o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw; ;
                #endif

                return o;
            }

            half4 ShadowPassFragment(VertexOutput i) : SV_TARGET
            {  
                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
              
                #if _ALPHATEST_ON
                float4 col = tex2D(_MainTex, i.uv);
                clip(col.a - _Alpha);
                #endif

                return 0;
            }

            ENDHLSL
        }
   
        Pass
        {
            Name "DepthOnly"
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0

            Cull Back

            HLSLPROGRAM
           
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
   
            // GPU Instancing
            #pragma multi_compile_instancing

            #pragma vertex vert
            #pragma fragment frag
               
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
               
            CBUFFER_START(UnityPerMaterial)
            CBUFFER_END
               
            struct VertexInput
            {
                float4 vertex : POSITION;                   
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct VertexOutput
            {           
                float4 vertex : SV_POSITION;
                 
                UNITY_VERTEX_INPUT_INSTANCE_ID           
                UNITY_VERTEX_OUTPUT_STEREO                 
            };

            VertexOutput vert(VertexInput v)
            {
                VertexOutput o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = TransformObjectToHClip(v.vertex.xyz);

                return o;
            }

            half4 frag(VertexOutput IN) : SV_TARGET
            {       
                return 0;
            }
            
            ENDHLSL
        }
    }
}
