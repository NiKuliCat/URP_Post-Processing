Shader "Pineapple/Post-Processing/Blur/KawaseBlur"
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

        float4  _MainTex_TexelSize;

        struct Attributes
        {
            float4 positionOS: POSITION;
            float2 uv: TEXCOORD0;
        };

        struct Varyings
        {
            float4 positionCS: SV_POSITION;
            float2 uv: TEXCOORD0;
        };


        Varyings vertexCompute(Attributes i)
        {
            Varyings o;
            o.positionCS = TransformObjectToHClip(i.positionOS);
            o.uv = i.uv;
            return o;
        }

        float4 fragShading_KawaseBlur(Varyings v) : SV_TARGET
        {
            float4 color = float4(0,0,0,0);

            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv + float2(_BlurRadius.x + 0.5, _BlurRadius.x + 0.5) * _MainTex_TexelSize.xy);
            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv + float2(-_BlurRadius.x - 0.5, _BlurRadius.x + 0.5) * _MainTex_TexelSize.xy);
            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv + float2(-_BlurRadius.x - 0.5, -_BlurRadius.x - 0.5) * _MainTex_TexelSize.xy);
            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv + float2(_BlurRadius.x + 0.5,  -_BlurRadius.x - 0.5) * _MainTex_TexelSize.xy);

            color *= 0.25f;

            return color;
        }

        ENDHLSL

        Pass
        {

            HLSLPROGRAM

            #pragma vertex vertexCompute
            #pragma fragment fragShading_KawaseBlur

            ENDHLSL

        }

    }
}
