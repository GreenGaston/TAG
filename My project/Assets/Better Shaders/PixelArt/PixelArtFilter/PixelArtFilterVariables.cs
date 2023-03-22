using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/PixelArt/PixelArtFilterVariables", typeof(UniversalRenderPipeline))]
public class PixelArtFilterVariables : VolumeComponent, IPostProcessComponent
{

    public ClampedIntParameter downSamples = new ClampedIntParameter(1,1, 8);
	
    public BoolParameter Activation = new BoolParameter(value: false, overrideState: true);
    // Tells when our effect should be rendered
    public bool IsActive() => Activation.value;
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
