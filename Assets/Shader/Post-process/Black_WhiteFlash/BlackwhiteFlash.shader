Shader "Pineapple/Post-Processing/BlackwhiteFlash"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex1("Noise_1" ,2D) = "white" {}
        _NoiseTex2("Noise_2" ,2D) = "white" {}

        _Threshold("Threshold",Range(0,0.3)) = 0.1
        _Smooth("Smooth",Range(0,0.15)) = 0

        [HDR]_FlashColor_1("FlashColor 1",Color) = (1,1,1,1)
        [HDR]_FlashColor_2("FlashColor 2",Color) = (0,0,0,0)



    }
    SubShader
    {

        HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);

        TEXTURE2D(_NoiseTex1);
        SAMPLER(sampler_NoiseTex1);

        TEXTURE2D(_NoiseTex2);
        SAMPLER(sampler_NoiseTex2);


        CBUFFER_START(UnityPerMaterial)

        float4 _NoiseTex1_ST;

        float4 _NoiseTex2_ST;

        half _Threshold;
        half _Smooth;

        float4 _FlashColor_1;
        float4 _FlashColor_2;



        float4 _Params1;
        float4 _Params2;
        CBUFFER_END


        //去色
        float3 grayColor(float3 color)
        {
            float gray = color.r * 0.264 + color.g * 0.617 + color.b * 0.149;
            return float3(gray, gray, gray);
        }

        //极坐标转换
        float2 polarCoordinates(float2 UV, float2 Center, float RadialScale, float LengthScale)
        {
            float2 delta = UV - Center;
            float radius = length(delta) * 2 * RadialScale;
            float angle = atan2(delta.x, delta.y) * 1.0 / 6.28 * LengthScale;
            return float2(radius, angle);
        }

        struct Attributes
        {
            float4 positionOS : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float2 uv : TEXCOORD0;
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
            //float uv = v.uv + float2(_Time.y,0);
            float2 polarUV = polarCoordinates(v.uv,_Params1.xy,_Params1.z,_Params1.w);

            half noise_1 = SAMPLE_TEXTURE2D(_NoiseTex1, sampler_NoiseTex1, polarUV + _Time.y * float2(_Params2.z,  _Params2.w));
            half noise_2 = SAMPLE_TEXTURE2D(_NoiseTex2, sampler_NoiseTex2, polarUV +  _Time.y * float2(_Params2.z, _Params2.w));

            float4 sceneColor = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,v.uv);

            half gray = grayColor(sceneColor.rgb).r;

            half flash = lerp(gray, noise_1 + noise_2, _Params2.y);

            half flashFactor = smoothstep(_Threshold - _Smooth, _Threshold + _Smooth, flash);

            float3 flashColor = lerp(_FlashColor_1, _FlashColor_2, flashFactor);

            float3 finalColor = lerp(sceneColor, flashColor, _Params2.x);



            return float4(finalColor, 1.0);

        }

        ENDHLSL


        Pass
        {

            HLSLPROGRAM
                
            #pragma vertex vertexCompute
            #pragma fragment fragShading

            ENDHLSL


        }





    }
}
