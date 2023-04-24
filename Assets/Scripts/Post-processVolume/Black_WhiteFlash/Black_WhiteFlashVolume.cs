using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


[Serializable, VolumeComponentMenuForRenderPipeline("Pineapple/Black_WhiteFlash", typeof(UniversalRenderPipeline))]
public class Black_WhiteFlashVolume : VolumeComponent, IPostProcessComponent
{
    public BoolParameter Enable = new BoolParameter(false);

    public ClampedFloatParameter Intensity = new ClampedFloatParameter(0.8f, 0.0f, 1.0f);

    public ClampedFloatParameter NoiseIntensity = new ClampedFloatParameter(0.4f, 0.0f, 1.0f);

    public ColorParameter BlackTintColor = new ColorParameter(Color.black,true,false,true);

    public ColorParameter WhiteTintColor = new ColorParameter(Color.white, true, false, true);

    public ClampedFloatParameter Center_X_SS = new ClampedFloatParameter(0.5f, 0.0f, 1.0f);
    public ClampedFloatParameter Center_Y_SS = new ClampedFloatParameter(0.5f, 0.0f, 1.0f);

    public ClampedFloatParameter NoiseWave_U = new ClampedFloatParameter(0.4f, 0.0f, 1.0f);
    public ClampedFloatParameter NoiseWave_V = new ClampedFloatParameter(0.4f, 0.0f, 1.0f);

    public ClampedFloatParameter RadialTilling = new ClampedFloatParameter(0f, 0.0f, 1.0f);
    public ClampedFloatParameter LengthTilling = new ClampedFloatParameter(10.0f, 0.0f, 30.0f);

    public bool IsActive() => Enable.value;

    public bool IsTileCompatible() => false;

}
