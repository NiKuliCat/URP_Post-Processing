using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;



[Serializable, VolumeComponentMenuForRenderPipeline("Pineapple/Glitch/RGBSplitGlitch", typeof(UniversalRenderPipeline))]
public class RGBSplitGlitchVolume : VolumeComponent, IPostProcessComponent
{

    public BoolParameter Enable = new BoolParameter(false);

    public ClampedFloatParameter Intensity = new ClampedFloatParameter(1.0f, 0.0f, 1.0f);

    public ClampedFloatParameter Wave = new ClampedFloatParameter(1, 0, 3);

    public ClampedFloatParameter Multi = new ClampedFloatParameter(3.0f, 0.0f, 10.0f);

    public bool IsActive() => Enable.value;

    public bool IsTileCompatible() => false;
}
