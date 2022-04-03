Shader "Kardamoony/Color Selector/Color Wheel"
{
    Properties
    {
        _BackgroundCircle ("Background Circle", Range(0, 1)) = 1
        _SamplerCircle ("Sampler Circle", Range(0, 1)) = 1
        
        _OuterCircle ("Outer Circle", Range(0, 1)) = 1
        _InnerCircle ("Inner Circle", Range(0, 1)) = 0.5
        
        [Space(10)]
        _FrameColor ("Frame Color", Color) = (1, 1, 1, 1)
        _SamplerColor ("Sampler Color", Color) = (1, 0, 0, 1)
        
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
            
            #include "Assets/Resources/Shaders/ShaderHelpers.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;
            };

            struct Varyings
            {
                float4 pos : SV_POSITION;
                half2 uv : TEXCOORD0;
                half4 color : COLOR0;
            };
            
            CBUFFER_START(UnityPerMaterial)
                float _OuterCircle;
                float _InnerCircle;
            
                float _BackgroundCircle;
                float _SamplerCircle;
            
                half4 _FrameColor;
                half4 _SamplerColor;
            CBUFFER_END

             Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.pos = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.color = IN.color;
                return OUT;
            }

            half3 hue2rgb(float hue)
            {
                hue = frac(hue);
                half r = abs(hue * 6 - 3) - 1;
                half g = 2 - abs(hue * 6 - 2);
                half b = 2 - abs(hue * 6 - 4);
                return saturate(half3(r, g, b));
            }

            half GetCircle(float2 uv, float size)
            {
                return Circle(uv, size * 0.5);
            }
            
            half4 frag (Varyings IN) : SV_Target
            {
                float2 uv = (IN.uv - 0.5) * 2;
                float angle = atan2(uv.x, uv.y) / TWO_PI;

                half outerCircle = GetCircle(IN.uv, _OuterCircle);
                half innerCircle = GetCircle(IN.uv, _InnerCircle);
                half frameCircle = GetCircle(IN.uv, _BackgroundCircle);
                half samplerCircle = GetCircle(IN.uv, _SamplerCircle);

                half wheelAlpha = outerCircle - innerCircle;
                half backgroundAlpha = frameCircle - wheelAlpha - samplerCircle;

                half4 samplerColor = _SamplerColor * samplerCircle;
                half4 frameColor = _FrameColor * IN.color * backgroundAlpha;
                half4 rgbWheelColor = half4(hue2rgb(angle), wheelAlpha) * wheelAlpha;
                
                return saturate(frameColor + rgbWheelColor + samplerColor);
            }
            
            ENDHLSL
        }
    }
}
