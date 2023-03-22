using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/EdgeDetection/EdgeDetect", typeof(UniversalRenderPipeline))]
public class EdgeDetectVariables : VolumeComponent, IPostProcessComponent
{

    public ColorParameter borderColor = new ColorParameter(Color.black, overrideState: true);
    public ClampedFloatParameter ThreshHold = new ClampedFloatParameter(0.0f, 0f, 1f, overrideState: true);
    public ClampedFloatParameter DepthThreshold = new ClampedFloatParameter(0.0f, 0f, 1f, overrideState: true);
	
    public BoolParameter Activation = new BoolParameter(value: false, overrideState: true);

    // Tells when our effect should be rendered
    public bool IsActive() => Activation.value;
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
