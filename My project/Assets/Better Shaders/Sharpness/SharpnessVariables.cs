using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/SharpnessVariables", typeof(UniversalRenderPipeline))]
public class SharpnessVariables : VolumeComponent, IPostProcessComponent
{
	public ClampedFloatParameter Amount = new ClampedFloatParameter(0.0f, -10.0f, 10.0f);
    public ClampedIntParameter Contrast = new ClampedIntParameter(0, 0,1);
    public BoolParameter Activation = new BoolParameter(value: false, overrideState: true);
    // Tells when our effect should be rendered
    public bool IsActive() => Activation.value;
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
