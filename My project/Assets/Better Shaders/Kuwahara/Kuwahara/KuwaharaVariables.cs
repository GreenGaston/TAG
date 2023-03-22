using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/Kuwahara/Kuwahara", typeof(UniversalRenderPipeline))]
public class KuwaharaVariables : VolumeComponent, IPostProcessComponent
{
	

    public ClampedIntParameter KernelSize = new ClampedIntParameter(value: 1, min: 1, max: 20);
    public BoolParameter AnimateKernelSize = new BoolParameter(value: false, overrideState: true);
    public ClampedIntParameter MinKernelSize = new ClampedIntParameter(value: 1, min: 1, max: 20);
    public ClampedFloatParameter SizeAnimationSpeed = new ClampedFloatParameter(value: 0.1f, min: 0.1f, max: 5.0f);
    public ClampedFloatParameter NoiseFrequency = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 30.0f);
    public BoolParameter AnimateKernelOrigin = new BoolParameter(value: false, overrideState: true);
    public ClampedIntParameter Passes = new ClampedIntParameter(value: 1, min: 1, max: 4);
    public BoolParameter Activation = new BoolParameter(value: false, overrideState: true);
    // Tells when our effect should be rendered
    public bool IsActive() => Activation.value;
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
