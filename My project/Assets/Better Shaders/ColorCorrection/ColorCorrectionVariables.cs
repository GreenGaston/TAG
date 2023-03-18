using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/ColorCorrectionVariables", typeof(UniversalRenderPipeline))]
public class ColorCorrectionVariables : VolumeComponent, IPostProcessComponent
{

   

    public  Vector3Parameter exposure = new Vector3Parameter(new Vector3(1.0f, 1.0f, 1.0f));
    public  ClampedFloatParameter temperature = new ClampedFloatParameter(value: -100.0f, min: -100.0f, max: 100.0f, overrideState: true);
    public  ClampedFloatParameter tint = new ClampedFloatParameter(value: -100.0f, min: -100.0f, max: 100.0f, overrideState: true);
    public Vector3Parameter contrast = new Vector3Parameter(new Vector3(1.0f, 1.0f, 1.0f));
    public Vector3Parameter linearMidPoint = new Vector3Parameter(new Vector3(0.5f, 0.5f, 0.5f));
    public Vector3Parameter brightness = new Vector3Parameter(new Vector3(0.0f, 0.0f, 0.0f));
    public ColorParameter colorFilter = new ColorParameter(value: Color.white, overrideState: true);
    public Vector3Parameter saturation = new Vector3Parameter(new Vector3(1.0f, 1.0f, 1.0f));
	
    public BoolParameter Activation = new BoolParameter(value: false, overrideState: true);
    // Tells when our effect should be rendered
    public bool IsActive() => Activation.value;
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
