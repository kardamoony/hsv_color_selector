Shader "Unlit/Color Picker"
{
    Properties
    {
        [HideInInspector] _OuterCircle ("Outer Circle", Range(0, 1)) = 1
        [HideInInspector] _InnerCircle ("Inner Circle", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags 
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "ForceNoShadowCasting" = "True"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/Resources/Shaders/ShaderHelpers.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 pos : SV_POSITION;
                half2 uv : TEXCOORD0;
            };
            
            CBUFFER_START(UnityPerMaterial)
                float _OuterCircle;
                float _InnerCircle;
            CBUFFER_END

             Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.pos = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float aAngle = radians(120);
                
                float cosine = cos(aAngle);
                
                float2 dirRed = float2(0, 0.5);
                float2 dirGreen = Rotate(dirRed, aAngle);
                float2 dirBlue = Rotate(dirGreen, aAngle);
                
                //\a\ = sqrt(x^2 + y^2)
                //cos A = dot(a, b) / \a\ * \b\

                float2 centerToUvDir = IN.uv - float2(0.5, 0.5);

                float cosR = GetCosine(dirRed, centerToUvDir);
                half r = Remap01(cosR, float2(cosine, 1));

                float cosG = GetCosine(dirGreen, centerToUvDir);
                half g = Remap01(cosG, float2(cosine, 1));

                float cosB = GetCosine(dirBlue, centerToUvDir);
                half b = Remap01(cosB, float2(cosine, 1));

                half alpha = Circle(IN.uv, _OuterCircle * 0.5) - Circle(IN.uv, _InnerCircle * 0.5);
                
                return half4(r, g, b, alpha);
            }
            
            ENDHLSL
        }
    }
}
