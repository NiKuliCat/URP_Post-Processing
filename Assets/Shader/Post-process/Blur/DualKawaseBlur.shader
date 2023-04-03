Shader "Pineapple/Post-Processing/Blur/DualKawaseBlur"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }


    SubShader
    {
        HLSLINCLUDE


        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"


        half4 _BlurRadius;

        float4 _MainTex_ST;

        float4  _MainTex_TexelSize;

        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);

        

        struct Attributes
        {
            float4 positionOS: POSITION;
            float2 uv: TEXCOORD0;
        };

        struct Varyings_DownSampler
        {
            float4 positionCS : SV_POSITION;
            float2 uv : TEXCOORD0;
            float4 uv1 : TEXCOORD1;
            float4 uv2 : TEXCOORD2;
        };

        struct Varyings_UpSampler
        {
            float4 positionCS : SV_POSITION;
            float4 uv1 : TEXCOORD0;
            float4 uv2 : TEXCOORD1;
            float4 uv3 : TEXOOCRD2;
            float4 uv4 : TEXOOCRD3;
        };

        Varyings_DownSampler vertex_DownSampler(Attributes i)
        {
            Varyings_DownSampler o;

            o.positionCS = TransformObjectToHClip(i.positionOS);
            float2 uv = TRANSFORM_TEX(i.uv, _MainTex);

            float2 TexelSize = float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * 0.5;
            float2 offset = float2(1 + _BlurRadius.x, 1 + _BlurRadius.x);

            o.uv = uv;

            o.uv1.xy = uv + TexelSize * offset;
            o.uv1.zw = uv - TexelSize * offset;

            o.uv2.xy = uv + float2(TexelSize.x, -TexelSize.y) * offset;
            o.uv2.zw = uv - float2(TexelSize.x, -TexelSize.y) * offset;

            return o;
        }

        float4 frag_DownSampler(Varyings_DownSampler v) : SV_TARGET
        {
            float4 color = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,v.uv) * 4;

            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv1.xy);
            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv1.zw);
            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv2.xy);
            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv2.zw);

            color *= 0.125;


            return color;
        }

        Varyings_UpSampler vertex_UpSampler(Attributes i)
        {
            Varyings_UpSampler o;

            o.positionCS = TransformObjectToHClip(i.positionOS);
            float2 uv = TRANSFORM_TEX(i.uv, _MainTex);

            float2 TexelSize = float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * 0.5;
            float2 offset = float2(1 + _BlurRadius.x, 1 + _BlurRadius.x);

            o.uv1.xy = uv + float2(-2, 0) * TexelSize * offset;
            o.uv1.zw = uv + float2(-1, 1) * TexelSize * offset;

            o.uv2.xy = uv + float2(0, 2) * TexelSize * offset;
            o.uv2.zw = uv + float2(1, 1) * TexelSize * offset;

            o.uv3.xy = uv + float2(2, 0) * TexelSize * offset;
            o.uv3.zw = uv + float2(1, -1) * TexelSize * offset;

            o.uv4.xy = uv + float2(0, -2) * TexelSize * offset;
            o.uv4.zw = uv + float2(-1, -1) * TexelSize * offset;


            return o;
        }


        float4 frag_UpSampler(Varyings_UpSampler v) : SV_TARGET
        {
            float4 color = float4(0,0,0,0);

            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv1.xy);
            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv1.zw) * 2;

            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv2.xy);
            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv2.zw) * 2;

            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv3.xy);
            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv3.zw) * 2;

            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv4.xy);
            color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv4.zw) * 2;

            color *= 0.0833;

            return color;
        }

        ENDHLSL

        Pass
        {
           
            HLSLPROGRAM

            #pragma vertex vertex_DownSampler
            #pragma fragment frag_DownSampler

            ENDHLSL

        }

        Pass
        {

            HLSLPROGRAM

            #pragma vertex vertex_UpSampler
            #pragma fragment frag_UpSampler

            ENDHLSL

        }

    }
}
