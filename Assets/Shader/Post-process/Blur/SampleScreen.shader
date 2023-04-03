Shader "Pineapple/SampleScreen"
{
    Properties
    {
    }
        SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
        }

        Pass
        {
           HLSLPROGRAM

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma vertex vertexCompute
            #pragma fragment fragShading


           TEXTURE2D(_BlurBuffer1);
           SAMPLER(sampler_BlurBuffer1);

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

           float4 fragShading(Varyings v) : SV_TARGET
           {
               //float4 color = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,v.uv);
               float4 color = SAMPLE_TEXTURE2D(_BlurBuffer1,sampler_BlurBuffer1,v.uv);
               return color;


           }


           ENDHLSL
        }
    }
}
