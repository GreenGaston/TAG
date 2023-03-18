using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/HueShiftVariables", typeof(UniversalRenderPipeline))]
public class HueShiftVariables : VolumeComponent, IPostProcessComponent
{
	
    public BoolParameter Activation = new BoolParameter(value: false, overrideState: true);
    public ClampedFloatParameter Shift = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
    // Tells when our effect should be rendered
    public bool IsActive() => Activation.value;
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
