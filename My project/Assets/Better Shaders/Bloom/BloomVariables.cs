using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/Bloom", typeof(UniversalRenderPipeline))]
public class BloomVariables : VolumeComponent, IPostProcessComponent
{
    public ClampedFloatParameter Threshold = new ClampedFloatParameter(value: 0.0f, min: 0f, max: 10f);

    public ClampedFloatParameter softThreshold = new ClampedFloatParameter(value: 0.0f, min: 0f, max: 1f);

    public ClampedIntParameter downSamples = new ClampedIntParameter(value: 0, min: 0, max: 16);
	
    public ClampedFloatParameter downSampleDelta = new ClampedFloatParameter(value: 0.0f, min: 0f, max: 2f);

    public ClampedFloatParameter upSampleDelta = new ClampedFloatParameter(value: 0.0f, min: 0f, max: 2f);

    public ClampedFloatParameter intensity = new ClampedFloatParameter(value: 0.0f, min: 0f, max: 10f);
    public BoolParameter Activation = new BoolParameter(value: false, overrideState: true);
    // Tells when our effect should be rendered
    public bool IsActive() => Activation.value;
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
