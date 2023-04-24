using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Pineapple/Blur", typeof(UniversalRenderPipeline))]
public class BlurVolume : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enable = new BoolParameter(false);

    public ClampedFloatParameter BlurRadius = new ClampedFloatParameter(1.0f, 0.0f, 20.0f);

    public ClampedIntParameter Iteration = new ClampedIntParameter(1, 1, 45);

    public ClampedFloatParameter DownSampler = new ClampedFloatParameter(1.0f, 1.0f, 20.0f);


    public bool IsActive() => enable.value;

    public bool IsTileCompatible() => false;

}
