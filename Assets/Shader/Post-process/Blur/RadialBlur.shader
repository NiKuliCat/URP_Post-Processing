Shader "Pineapple/Post-Processing/Blur/RadialBlur"
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

        float4 frag_RadialBlur(Varyings v) : SV_TARGET
        {
            float2 center = _BlurRadius.zw;
            float2 uv = v.uv - center;

            float4 color = float4(0, 0, 0, 0);
            half scale = 1;

            for (int i = 0; i < int(_BlurRadius.x); i++)
            {
                scale = i * _BlurRadius.y + 1;
                color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + center);
            }
           
            color *= 1/_BlurRadius.x;

            return color;
        }

        ENDHLSL

        Pass
        {

            HLSLPROGRAM

            #pragma vertex vertexCompute
            #pragma fragment frag_RadialBlur


            ENDHLSL


        }
        
    }
}
