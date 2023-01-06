using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/DitheringVariables", typeof(UniversalRenderPipeline))]
public class DitheringVariables: VolumeComponent, IPostProcessComponent
{
	
    
    public ClampedFloatParameter spread= new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 1.0f, overrideState: true);

    public ClampedIntParameter redColorCount = new ClampedIntParameter(value: 2, min: 2, max: 16, overrideState: true);

    public ClampedIntParameter greenColorCount = new ClampedIntParameter(value: 2, min: 2, max: 16, overrideState: true);

    public ClampedIntParameter blueColorCount = new ClampedIntParameter(value: 2, min: 2, max: 16, overrideState: true);

    public ClampedIntParameter bayerLevel = new ClampedIntParameter(value: 0, min: 0, max: 2, overrideState: true);

    public ClampedIntParameter downSamples = new ClampedIntParameter(value: 0, min: 0, max: 8, overrideState: true);

    public BoolParameter pointFilterDown = new BoolParameter(value: false, overrideState: true);

    public BoolParameter Activation = new BoolParameter(value: false, overrideState: true);

    
    // Tells when our effect should be rendered
    public bool IsActive() => Activation.value;
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
