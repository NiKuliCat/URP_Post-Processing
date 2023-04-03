Shader "Pineapple/Post-Processing/Blur/BoxBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
       HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/core.hlsl"

        half4 _BlurRadius;

        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);


        struct Attributes
        {
            float4 positionOS: POSITION;
            float2 uv: TEXCOORD0;
        };

        struct Varyings
        {
            float4 positionCS: SV_POSITION;
            float2 uv: TEXCOORD0;
            float4 uv1: TEXCOORD1;
        };


        Varyings vertexCompute(Attributes i)
        {
            Varyings o;
            o.positionCS = TransformObjectToHClip(i.positionOS);
            o.uv = i.uv;
            o.uv1 = _BlurRadius.xyxy * float4(1, 1, -1, -1);
            return o;
        }

        float4 fragShading_BoxBlur(Varyings v) : SV_TARGET
        {
            float4 color = float4(0,0,0,0);

            //========================================================================//
            // 1/4   1/4
            // 1/4   1/4
            //========================================================================//

            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv + v.uv1.zy) * 0.25f;
            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv + v.uv1.xy) * 0.25f;
            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv + v.uv1.zw) * 0.25f;
            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv + v.uv1.xw) * 0.25f;

            return color;

        }

        ENDHLSL


        Pass
        {

            HLSLPROGRAM

            #pragma vertex vertexCompute
            #pragma fragment fragShading_BoxBlur

            ENDHLSL

        }

        Pass
        {

            HLSLPROGRAM

            #pragma vertex vertexCompute
            #pragma fragment fragShading_BoxBlur

            ENDHLSL

        }
    }
}
