using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/AnisotropicKuwaharaVariables", typeof(UniversalRenderPipeline))]
public class AnisotropicKuwaharaVariables : VolumeComponent, IPostProcessComponent
{
	
    //     [Range(2, 20)]
    // public int kernelSize = 2;
    // [Range(1.0f, 18.0f)]
    // public float sharpness = 8;
    // [Range(1.0f, 100.0f)]
    // public float hardness = 8;
    // [Range(0.01f, 2.0f)]
    // public float alpha = 1.0f;
    // [Range(0.01f, 2.0f)]
    // public float zeroCrossing = 0.58f;

    // public bool useZeta = false;
    // [Range(0.01f, 3.0f)]
    // public float zeta = 1.0f;

    // [Range(1, 4)]
    // public int passes = 1;

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
