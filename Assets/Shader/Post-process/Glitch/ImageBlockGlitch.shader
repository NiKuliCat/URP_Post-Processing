Shader "Pineapple/Post-Processing/Glitch/ImageBlockGlitch"
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
       float _BlockSize;

       
       float4 fragShading(Varyings v) : SV_TARGET
       {
           half block = randomNoise(floor(v.uv * _BlockSize),_GlitchParams.y);
           half blockGlitchFactor = _GlitchParams.x * pow(block.x, 8.0) * pow(block.x, 3.0);

           float glitchSplitFactor = _GlitchParams.x * 0.7 * randomSplit01(_Time.y * _GlitchParams.y * 0.2, _GlitchParams.w) * cos(_GlitchParams.z);

           float finalNoise = blockGlitchFactor - glitchSplitFactor;

           float noiseX = 0.05 * randomNoise(13.0, _GlitchParams.y);
           float noiseY = 0.05 * randomNoise(7.0, _GlitchParams.y);

           float2 noiseOffset = float2(finalNoise * noiseX, finalNoise * noiseY);

           float4 color = 0;

           color.r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv).r;
           color.g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv + noiseOffset).g;
           color.b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv - noiseOffset).b;
           color.a = 1.0;

           return color;
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
