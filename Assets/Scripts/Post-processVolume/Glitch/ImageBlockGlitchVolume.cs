using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;



[Serializable, VolumeComponentMenuForRenderPipeline("Pineapple/Glitch/ImageBlockGlitch", typeof(UniversalRenderPipeline))]

public class ImageBlockGlitchVolume : VolumeComponent, IPostProcessComponent
{

    public BoolParameter Enable = new BoolParameter(false);

    public ClampedFloatParameter Intensity = new ClampedFloatParameter(1.0f, 0.0f, 1.0f);

    public ClampedFloatParameter BlockSize = new ClampedFloatParameter(12.0f, 0.0f, 20.0f);

    public ClampedFloatParameter Wave = new ClampedFloatParameter(8, 0, 15);

    public ClampedFloatParameter Multi = new ClampedFloatParameter(4.0f, 0.0f, 10.0f);

    public bool IsActive() => Enable.value;

    public bool IsTileCompatible() => false;
}
