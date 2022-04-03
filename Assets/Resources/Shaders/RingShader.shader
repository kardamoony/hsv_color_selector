Shader "Kardamoont/Ring"
{
    Properties
    {
        _OuterCircle ("Outer Circle", Range(0, 1)) = 1
        _InnerCircle ("Inner Circle", Range(0, 1)) = 0.5
        _Color ("Color", Color) = (1, 1, 1, 1)
        
        //Stencil
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Range(0, 255)) = 255
        _StencilReadMask ("Stencil Read Mask", Range(0, 255)) = 255
        
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    }
    SubShader
    {
        ColorMask RGB
        
        Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}
        
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
                half4 uv : TEXCOORD0;
                half4 color : COLOR0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float _OuterCircle;
                float _InnerCircle;
                half4 _Color;
            CBUFFER_END

             Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.pos = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv.xy = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.uv.zw = IN.uv;
                OUT.color = IN.color;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv.xy) * _Color * IN.color;
                col.a *= Circle(IN.uv.zw, _OuterCircle * 0.5) - Circle(IN.uv.zw, _InnerCircle * 0.5);
                return col;
            }
            
            ENDHLSL
        }
    }
}
