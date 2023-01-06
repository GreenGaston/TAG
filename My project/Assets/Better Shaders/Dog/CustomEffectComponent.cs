using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/CustomEffectComponent", typeof(UniversalRenderPipeline))]
public class CustomEffectComponent : VolumeComponent, IPostProcessComponent
{
	
    //gaussianKernelSize is the size of the gaussian kernel
    public ClampedIntParameter gaussianKernelSize = new ClampedIntParameter(value: 1, min: 1, max: 10, overrideState: true);
    //stdev is the standard deviation of the gaussian kernel
    public ClampedFloatParameter stdev = new ClampedFloatParameter(value: 0.1f, min: 0.1f, max: 5.0f, overrideState: true);
    //stdevScale is the scale of the standard deviation of the gaussian kernel
    public ClampedFloatParameter stdevScale = new ClampedFloatParameter(value: 0.1f, min: 0.1f, max: 5.0f, overrideState: true);
    //tau is the tau value of the difference of gaussians
    public ClampedFloatParameter tau = new ClampedFloatParameter(value: 0.01f, min: 0.01f, max: 5.0f, overrideState: true);
    //boolean for thresholding
    public BoolParameter thresholding = new BoolParameter(value: false, overrideState: true);
    //boolean for tanh
    public BoolParameter tanh = new BoolParameter(value: false, overrideState: true);
    //phi is value for tanh
    public ClampedFloatParameter phi = new ClampedFloatParameter(value: 0.01f, min: 0.01f, max: 100.0f, overrideState: true);
    //threshold is the threshold value for thresholding
    public ClampedFloatParameter threshold = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 0.5f, overrideState: true);
    //invert is the boolean for inverting the image
    public BoolParameter invert = new BoolParameter(value: false, overrideState: true);

    public BoolParameter Activation = new BoolParameter(value: false, overrideState: true);
    // Other 'Parameter' variables you might have
    
    // Tells when our effect should be rendered
    public bool IsActive() => Activation.value;
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
