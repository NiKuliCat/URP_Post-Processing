using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;



[Serializable, VolumeComponentMenuForRenderPipeline("Pineapple/Glitch/ImageBlockGlitch_Double", typeof(UniversalRenderPipeline))]

public class ImageBlockGlitch_DoubleVolume : VolumeComponent, IPostProcessComponent
{

    public BoolParameter Enable = new BoolParameter(false);

    public ClampedFloatParameter Intensity = new ClampedFloatParameter(0.8f, 0.0f, 1.0f);

    public ClampedFloatParameter Wave = new ClampedFloatParameter(8, 0, 15);

    public ClampedFloatParameter Multi = new ClampedFloatParameter(4.0f, 0.0f, 10.0f);

    public BlockSettings BlockLayer_1 = new BlockSettings();

    public BlockSettings BlockLayer_2 = new BlockSettings();

    public bool IsActive() => Enable.value;

    public bool IsTileCompatible() => false;

    [Serializable]
    public class BlockSettings
    {
        public ClampedFloatParameter BlockSize_X = new ClampedFloatParameter(0.0f,0.0f,30.0f);
        public ClampedFloatParameter BlockSize_Y = new ClampedFloatParameter(0.0f, 0.0f, 30.0f);
        public ClampedFloatParameter BlockIntensity = new ClampedFloatParameter(0.0f,0.0f,10.0f);
    }
}
