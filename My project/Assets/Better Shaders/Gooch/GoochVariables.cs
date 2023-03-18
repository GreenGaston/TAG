using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/Gooch", typeof(UniversalRenderPipeline))]
public class GoochVariables : VolumeComponent, IPostProcessComponent
{
	
    //gaussianKernelSize is the size of the gaussian kernel
    public ColorParameter Albedo = new ColorParameter(Color.white, overrideState: true);
    public ClampedFloatParameter Smoothness = new ClampedFloatParameter(value:0.01f, min:0.01f, max:1, overrideState: true);
    public ColorParameter Warm = new ColorParameter(Color.white, overrideState: true);
    public ColorParameter Cool = new ColorParameter(Color.white, overrideState: true);
    public ClampedFloatParameter Alpha = new ClampedFloatParameter(value:0.01f, min:0.01f, max:1, overrideState: true);
    public ClampedFloatParameter Beta = new ClampedFloatParameter(value:0.01f, min:0.01f, max:1, overrideState: true);
    public BoolParameter Activation = new BoolParameter(false, overrideState: true);

    //private lightVector lightVector = new lightVector();
    // Tells when our effect should be rendered
    public bool IsActive() => Activation.value;
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
