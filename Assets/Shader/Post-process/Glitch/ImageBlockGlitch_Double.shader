Shader "Pineapple/Post-Processing/Glitch/ImageBlockGlitch_Double"
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
       half4 _BlockSize;
       half2 _BlockIntensity;

       float4 fragShading(Varyings v) : SV_TARGET
       {
           float blockLayer1 = pow(randomNoise(floor(v.uv * _BlockSize.xy),_GlitchParams.y),_BlockIntensity.x);
           float blockLayer2 = pow(randomNoise(floor(v.uv * _BlockSize.zw), _GlitchParams.y), _BlockIntensity.y);

           float glitchSplitFactor = _GlitchParams.x  * randomSplit01(_Time.y * _GlitchParams.y * 0.2, _GlitchParams.w) * cos(_GlitchParams.z);

           float finalNoise = blockLayer1 * blockLayer2 - glitchSplitFactor;

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
