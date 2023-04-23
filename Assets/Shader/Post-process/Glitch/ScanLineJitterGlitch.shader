Shader "Pineapple/Post-Processing/Glitch/ScanLineJitterGlitch"
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
        
        

        float4 fragShading(Varyings v) : SV_TARGET
        {

            float strength = (0.5 + 0.5 * cos(_Time.y * _GlitchParams.y)) * _GlitchParams.w;

            float jitter = randomSplit(float2(v.uv.y, _Time.x)) * 2 - 1;
            jitter *= step(_GlitchParams.z, abs(jitter))  * strength *  _GlitchParams.x;

            half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, frac(v.uv + float2(jitter, 0)));

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
