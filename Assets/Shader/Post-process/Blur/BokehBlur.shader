Shader "Pineapple/Post-Processing/Blur/BokehBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {

        HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"


        half4 _BlurRadius;
        half4 _GoldenRot;


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
        };

        Varyings vertexCompute(Attributes i)
        {
            Varyings o;
            o.positionCS = TransformObjectToHClip(i.positionOS);
            o.uv = i.uv;

            return o;
        }

        float4 frag_BokehBlur(Varyings v) : SV_TARGET
        {
            half2x2 rot = half2x2(_GoldenRot);
            half4 accumulator = 0.0;
            half4 divisor = 0.0;

            half r = 1.0;
            half2 angle = half2(0.0, _BlurRadius.y);

            for (int j = 0; j < _BlurRadius.x; j++)
            {
                r += 1.0 / r;
                angle = mul(rot, angle);
                half4 bokeh = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(v.uv + _BlurRadius.zw * (r - 1.0) * angle));
                accumulator += bokeh * bokeh;
                divisor += bokeh;
            }

            return accumulator / divisor;
        }

        ENDHLSL

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vertexCompute
            #pragma fragment frag_BokehBlur

            ENDHLSL

        }


    }
}
