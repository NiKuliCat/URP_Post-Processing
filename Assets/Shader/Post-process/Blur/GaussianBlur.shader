Shader "Pineapple/Post-Processing/Blur/GaussianBlur"
{
    Properties
    {
        _MainTex("Texture",2D) = "white"{}
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
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
             float4 uv3 : TEXCOORD3;
         };


         Varyings BlurHorizontal(Attributes i)
         {
             Varyings o;
             o.positionCS = TransformObjectToHClip(i.positionOS);
             o.uv = i.uv;
             o.uv1 = i.uv.xyxy + float4(_BlurRadius.x, 0, _BlurRadius.x, 0) * float4(1, 1, -1, -1) * 1.0f;
             o.uv2 = i.uv.xyxy + float4(_BlurRadius.x, 0, _BlurRadius.x, 0) * float4(1, 1, -1, -1) * 2.0f;
             o.uv3 = i.uv.xyxy + float4(_BlurRadius.x, 0, _BlurRadius.x, 0) * float4(1, 1, -1, -1) * 6.0f;
             return o;
         }

         Varyings BlurVertical(Attributes i)
         {
             Varyings o;
             o.positionCS = TransformObjectToHClip(i.positionOS);
             o.uv = i.uv;
             o.uv1 = i.uv.xyxy + float4(0, _BlurRadius.y, 0, _BlurRadius.y) * float4(1, 1, -1, -1) * 1.0f;
             o.uv2 = i.uv.xyxy + float4(0, _BlurRadius.y, 0, _BlurRadius.y) * float4(1, 1, -1, -1) * 2.0f;
             o.uv3 = i.uv.xyxy + float4(0, _BlurRadius.y, 0, _BlurRadius.y) * float4(1, 1, -1, -1) * 6.0f;
             return o;
         }


         float4 fragShading(Varyings v) : SV_TARGET
         {
             float4 color = float4(0,0,0,0);

             color += 0.40 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv);
             color += 0.15 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv1.xy);
             color += 0.15 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv1.zw);
             color += 0.10 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv2.xy);
             color += 0.10 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv2.zw);
             color += 0.05 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv3.xy);
             color += 0.05 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv3.zw);

             return color;
         }
         
         ENDHLSL

        Pass
        {

            HLSLPROGRAM

            #pragma vertex BlurHorizontal
            #pragma fragment fragShading

            ENDHLSL
        
        }

         Pass
         {

             HLSLPROGRAM

             #pragma vertex BlurVertical
             #pragma fragment fragShading

             ENDHLSL

         }
    }
}
