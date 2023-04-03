Shader "Pineapple/Post-Processing/Blur/GrainyBlur"
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

        float Rand(float2 uv)
        {
            return sin(dot(uv, half2(1233.224, 1743.335)));
        }


        Varyings vertexCompute(Attributes i)
        {
            Varyings o;
            o.positionCS = TransformObjectToHClip(i.positionOS);
            o.uv = i.uv;

            return o;
        }

        float4 frag_GrainyBlur(Varyings v) : SV_TARGET
        {
            float2 random = float2(0,0);
            float4 color = float4(0,0,0,0);
            float offset = Rand(v.uv);

            for (int i = 0; i < _BlurRadius.x; i++)
            {
                offset = frac(43758.5453 * offset + 0.61432);
                random.x = (offset - 0.5) * 2;
                offset = frac(43758.5453 * offset + 0.61432);
                random.y = (offset - 0.5) * 2;

                color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv + random * _BlurRadius.y);

            }

            color *= 1 / _BlurRadius.x;

            return color;
        }

        ENDHLSL


        Pass
        {

            HLSLPROGRAM

            #pragma vertex  vertexCompute
            #pragma fragment frag_GrainyBlur


            ENDHLSL

        }
    }
}
