



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

inline float randomSplit(float2 speed)
{
    return frac(sin(dot(speed, float2(12.9898, 78.233))) * 43758.5453);
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

float trunc(float x, float num_levels)
{
	return floor(x * num_levels) / num_levels;
}

float2 trunc(float2 x, float2 num_levels)
{
	return floor(x * num_levels) / num_levels;
}

float3 rgb2yuv(float3 rgb)
{
	float3 yuv;
	yuv.x = dot(rgb, float3(0.299, 0.587, 0.114));
	yuv.y = dot(rgb, float3(-0.14713, -0.28886, 0.436));
	yuv.z = dot(rgb, float3(0.615, -0.51499, -0.10001));
	return yuv;
}

float3 yuv2rgb(float3 yuv)
{
	float3 rgb;
	rgb.r = yuv.x + yuv.z * 1.13983;
	rgb.g = yuv.x + dot(float2(-0.39465, -0.58060), yuv.yz);
	rgb.b = yuv.x + yuv.y * 2.03211;
	return rgb;
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