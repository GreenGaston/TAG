using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/Fog", typeof(UniversalRenderPipeline))]
public class FogVariables : VolumeComponent, IPostProcessComponent
{
	

    public ColorParameter fogColor = new ColorParameter(Color.white);
    public ClampedFloatParameter fogDensity = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
    public ClampedFloatParameter fogOffset = new ClampedFloatParameter(0.0f, 0.0f, 100.0f);
    public BoolParameter Activation = new BoolParameter(value: false, overrideState: true);
    // Tells when our effect should be rendered
    public bool IsActive() => Activation.value;
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
