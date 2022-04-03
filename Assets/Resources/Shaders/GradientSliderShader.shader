Shader "Kardamoony/Color Selector/Gradient Slider"
{
    Properties
    {
        _OuterCircle ("Outer Circle", Range(0, 1)) = 1
        _InnerCircle ("Inner Circle", Range(0, 1)) = 0.5
        
        [Space]
        _Color0 ("Color0", Color) = (1, 1, 1, 1)
        _Color1 ("Color1", Color) = (0, 0, 0, 1)
        
        [Space]
        _Rotate ("Rotate", Range(0, 360)) = 0
        _Sector ("Sector", Range(0, 360)) = 0
        
        [Space]
        [Toggle(FLIP_X)] _FlipX ("Flip X", float) = 0
        [Toggle(FLIP_Y)] _FlipY ("Flip Y", float) = 0
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

            #define UV_CENTER float2(0.5, 0.5)

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;
            };

            struct Varyings
            {
                float4 pos : SV_POSITION;
                half4 uv : TEXCOORD0;
                half4 color : COLOR0;
                float2 rotationData : TEXCOORD1; //center x, center y, 
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
            
                float _OuterCircle;
                float _InnerCircle;
            
                half4 _Color0;
                half4 _Color1;
            
                float _Sector;
                float _Rotate;
            
                half _FlipX;
                half _FlipY;
            CBUFFER_END

             Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.pos = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv.xy = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.uv.zw = IN.uv;
                
                OUT.uv.z = abs(_FlipX - OUT.uv.z);
                OUT.uv.w = abs(_FlipY - OUT.uv.w);
                
                OUT.color = IN.color;
                OUT.rotationData = float2(DegToRad(_Sector - 180), DegToRad(_Rotate + 180));
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float sector0 = IN.rotationData.x;
                float rotation = IN.rotationData.y;
                
                float2 uv = Rotate(IN.uv.zw, UV_CENTER, rotation);
                uv = (uv - 0.5) * 2;
                
                float angle = atan2(uv.x, uv.y);
                
                half sectorAlpha = step(angle, sector0);

                half gradient = smoothstep(0, sector0 / TWO_PI + 0.5, angle / TWO_PI + 0.5);

                //end point circles
                float circlesDiff = (_OuterCircle - _InnerCircle) * 0.5;
                float circleDistance = (_InnerCircle + circlesDiff) * 0.5;
                
                float2 topCircleCoords = Rotate(Rotate(float2(0.5, 0.5 + circleDistance), UV_CENTER, -sector0), UV_CENTER, -rotation);
                float2 bttmCircleCoords = Rotate(float2(0.5, 0.5 - circleDistance), UV_CENTER, -rotation);
                
                half topCircle = Circle(IN.uv.zw, circlesDiff * 0.5, topCircleCoords);
                half bttmCircle = Circle(IN.uv.zw, circlesDiff * 0.5, bttmCircleCoords);

                half ring = saturate(Circle(IN.uv.zw, _OuterCircle * 0.5) - Circle(IN.uv.zw, _InnerCircle * 0.5));
 
                half alpha = ring * saturate(sectorAlpha + topCircle + bttmCircle);

                gradient *= 1 - bttmCircle;

                half4 color = lerp(_Color1, _Color0, gradient);
                color.a *= alpha;
                
                return color;
            }
            
            ENDHLSL
        }
    }
}
