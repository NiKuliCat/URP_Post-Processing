using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


[Serializable, VolumeComponentMenuForRenderPipeline("Pineapple/Glitch/ScanLineJitterGlitch", typeof(UniversalRenderPipeline))]

public class ScanLineJitterGlitchVolume : VolumeComponent, IPostProcessComponent
{
    public BoolParameter Enable = new BoolParameter(false);

    public ClampedFloatParameter Intensity = new ClampedFloatParameter(0.8f, 0.0f, 1.0f);

    public ClampedFloatParameter Threshold = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);

    public ClampedFloatParameter Wave = new ClampedFloatParameter(1.2f, 0.0f, 3.0f);

    public ClampedFloatParameter Multi = new ClampedFloatParameter(0.03f, 0.0f, 0.15f);

    public bool IsActive() => Enable.value;

    public bool IsTileCompatible() => false;
}
