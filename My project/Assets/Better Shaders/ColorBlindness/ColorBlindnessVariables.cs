using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/ColorBlindnessVariables", typeof(UniversalRenderPipeline))]
public class ColorBlindnessVariables : VolumeComponent, IPostProcessComponent
{

    public ClampedIntParameter BlindType= new ClampedIntParameter(value: 0, min: 0, max: 2);

    public ClampedFloatParameter severity = new ClampedFloatParameter(value: 0.0f, min: 0f, max: 1f);

    public BoolParameter difference = new BoolParameter(value: false, overrideState: true);
	
    public BoolParameter Activation = new BoolParameter(value: false, overrideState: true);
    // Tells when our effect should be rendered
    public bool IsActive() => Activation.value;
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
