



TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);


inline float randomNoise(float2 seed, float speed)
{
    return frac(sin(dot(seed * floor(_Time.y * speed), float2(17.13, 3.71))) * 43758.5453123);
}

inline float randomNoise(float seed, float speed)
{
    return randomNoise(float2(seed, 1.0), speed);
}

inline float randomSplit(float speed)
{
    return frac(sin(dot(float2(speed, 2.0), float2(12.9898, 78.233))) * 43758.5453);
}

inline float randomSplit01(float speed,float multi)
{
    float splitAmout = (1.0 + sin(speed * 6.0)) * 0.5;
    splitAmout *= 1.0 + sin(speed * 16.0) * 0.5;
    splitAmout *= 1.0 + sin(speed * 19.0) * 0.5;
    splitAmout *= 1.0 + sin(speed * 27.0) * 0.5;
    splitAmout = pow(splitAmout, multi);
    splitAmout *= 0.05;
    return splitAmout;
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