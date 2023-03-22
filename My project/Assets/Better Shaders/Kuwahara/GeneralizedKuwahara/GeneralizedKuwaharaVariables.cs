using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/Kuwahara/GeneralizedKuwahara", typeof(UniversalRenderPipeline))]
public class GeneralizedKuwaharaVariables : VolumeComponent, IPostProcessComponent
{

    public ClampedIntParameter KernelSize = new ClampedIntParameter(value: 2, min: 2, max: 20);
    public ClampedFloatParameter Sharpness = new ClampedFloatParameter(value: 1.0f, min: 1.0f, max: 18.0f);
    public ClampedFloatParameter Hardness = new ClampedFloatParameter(value: 1.0f, min: 1.0f, max: 100.0f);
    public ClampedFloatParameter Alpha = new ClampedFloatParameter(value: 1.0f, min: 0.01f, max: 2.0f);
    public ClampedFloatParameter ZeroCrossing = new ClampedFloatParameter(value: 0.58f, min: 0.01f, max: 2.0f);
    public BoolParameter UseZeta = new BoolParameter(value: false);
    public ClampedFloatParameter Zeta = new ClampedFloatParameter(value: 1.0f, min: 0.01f, max: 3.0f);

    public ClampedIntParameter Passes = new ClampedIntParameter(value: 1, min: 1, max: 4);


	
    public BoolParameter Activation = new BoolParameter(value: false, overrideState: true);
    // Tells when our effect should be rendered
    public bool IsActive() => Activation.value;
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
