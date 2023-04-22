Shader "Pineapple/Post-Processing/Glitch/RGBSplitGlitch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        #include "Common.hlsl"

        half4 _GlitchParams;

        TEXTURE2D(_NoiseTex);
        SAMPLER(sampler_NoiseTex);

        float4 randomGlitchColor(float2 uv, float split, float random)
        {
            float4 horizontal_color = 0;
            float4 vertical_color = 0;

            horizontal_color.r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv).r;
            horizontal_color.g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x + split, uv.y)).g;
            horizontal_color.b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x - split, uv.y)).b;

            vertical_color.r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv).r;
            vertical_color.g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x , uv.y + split)).g;
            vertical_color.b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x , uv.y - split)).b;

            float4 finalColor = lerp(horizontal_color, vertical_color, random);

            finalColor *= (1.0 - split * 0.5);
            finalColor.a = 1.0;

            return finalColor;
        }

        float4  fragShading(Varyings v) : SV_TARGET
        {
            float glitchSplit = _GlitchParams.x * randomSplit01(_GlitchParams.y,_GlitchParams.w) * cos(_GlitchParams.z);
            float random = _GlitchParams.z * 0.5 + 0.5;
            return randomGlitchColor(v.uv,glitchSplit, random);
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
