using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/GammaVariables", typeof(UniversalRenderPipeline))]
public class GammaVariables : VolumeComponent, IPostProcessComponent
{

    public ClampedFloatParameter gamma = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 10.0f, overrideState: true);
	
    public BoolParameter Activation = new BoolParameter(value: false, overrideState: true);
    // Tells when our effect should be rendered
    public bool IsActive() => Activation.value;
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
