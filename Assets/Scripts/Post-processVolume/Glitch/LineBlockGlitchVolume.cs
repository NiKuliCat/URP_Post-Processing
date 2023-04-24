using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;



[Serializable, VolumeComponentMenuForRenderPipeline("Pineapple/Glitch/LineBlockGlitch", typeof(UniversalRenderPipeline))]
public class LineBlockGlitchVolume : VolumeComponent, IPostProcessComponent
{
    public BoolParameter Enable = new BoolParameter(false);


    public ClampedFloatParameter Wave = new ClampedFloatParameter(8, 0, 15);

    public ClampedFloatParameter Multi = new ClampedFloatParameter(4.0f, 0.0f, 10.0f);

    public ClampedFloatParameter LineWidth = new ClampedFloatParameter(2.5f, 0.0f, 10.0f);

    public ClampedFloatParameter Offset = new ClampedFloatParameter(0.5f, 0.0f, 1.0f);

    public ClampedFloatParameter Alpha = new ClampedFloatParameter(1.0f, 0.0f, 1.0f);

    public ClampedFloatParameter Frequency = new ClampedFloatParameter(3.0f, 0.0f, 10.0f);


    public bool IsActive() => Enable.value;

    public bool IsTileCompatible() => false;
}
