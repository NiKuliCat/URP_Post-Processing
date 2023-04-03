Shader "Pineapple/Post-Processing/Blur/TentBlur"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
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
             float4 uv1: TEXCORD1;
         };


         Varyings vertexCompute(Attributes i)
         {
             Varyings o;
             o.positionCS = TransformObjectToHClip(i.positionOS);
             o.uv = i.uv;
             o.uv1 = _BlurRadius.xyxy * float4(1, 1, -1, 0);
             return o;
         }


         float4 FragShading_TentBlur(Varyings v) : SV_TARGET
         {
             float4 color = float4(0,0,0,0);

             //===================================//
             // 1/16    1/8   1/16
             // 1/8     1/4   1/8
             // 1/16    1/8   1/16  
             //===================================//

             color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv + v.uv1.zy);
             color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv + v.uv1.wy) * 2.0f;
             color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv + v.uv1.xy);

             color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv + v.uv1.zw) * 2.0f;
             color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv) * 4.0f;
             color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv - v.uv1.zw) * 2.0f;

             color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv - v.uv1.xy);
             color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv - v.uv1.wy) * 2.0f;
             color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv - v.uv1.zy);

             color *= 1.0f / 16.0f;
             return color;
         }

         ENDHLSL

         Pass
         {
             HLSLPROGRAM

            #pragma vertex vertexCompute
            #pragma fragment FragShading_TentBlur

            ENDHLSL

         }

         Pass
         {

            HLSLPROGRAM

            #pragma vertex vertexCompute
            #pragma fragment FragShading_TentBlur

            ENDHLSL

         }
    }
}
