Shader "Pineapple/Post-Processing/Glitch/LineBlockGlitch"
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

        half2 _GlitchParams;
        half4 _LineParams;

        float4 fragShading(Varyings v) : SV_TARGET
        {
            half strength = 0.5 + 0.5 * cos(_GlitchParams.x * _LineParams.x);
            half time = _GlitchParams.x * strength;

            float truncTime = trunc(time,4.0);
            float uv_trunc = randomSplit(trunc(v.uv.yy, float2(8.0, 8.0)) + 100.0 * truncTime);
            float uv_randomTrunc = 6.0 * trunc(time, 24.0 * uv_trunc);

            float blockLine_random = 0.5 * randomSplit(trunc(v.uv.yy + uv_randomTrunc, float2(8 * _LineParams.y, 8 * _LineParams.y)));
            blockLine_random += randomSplit(trunc(v.uv.yy + uv_randomTrunc, float2(7.0, 7.0)));
            blockLine_random = blockLine_random * 2.0 - 1.0;
            blockLine_random = sign(blockLine_random) * saturate((abs(blockLine_random) - _GlitchParams.y) / (0.4));
            blockLine_random = lerp(0, blockLine_random, _LineParams.z);

            float2 uv_blockLine = v.uv;
            uv_blockLine = saturate(uv_blockLine + float2(blockLine_random, 0));
            float4 blockLineColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, abs(uv_blockLine));

            // RGB -> YUV
            float3 blockLineColor_yuv = rgb2yuv(blockLineColor.rgb);
            // adjust Chrominance | É«¶È
            blockLineColor_yuv.y /= 1.0 - 3.0 * abs(blockLine_random) * saturate(0.5 - blockLine_random);
            // adjust Chroma | Å¨¶È
            blockLineColor_yuv.z += 0.125 * blockLine_random * saturate(blockLine_random - 0.5);
            float3 blockLineColor_rgb = yuv2rgb(blockLineColor_yuv);

            float4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, v.uv);
            return lerp(sceneColor, float4(blockLineColor_rgb, blockLineColor.a), _LineParams.w);
            //return float4(blockLine_random, blockLine_random, blockLine_random, 1);
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
