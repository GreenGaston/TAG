using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/BlendModeVariables", typeof(UniversalRenderPipeline))]
public class BlendModeVariables : VolumeComponent, IPostProcessComponent
{

    public ClampedIntParameter BlendMode = new ClampedIntParameter(value: 0, min: 0, max: 9, overrideState: true);
    //texture
    public TextureParameter BlendTexture = new TextureParameter(value: null, overrideState: true);

    public ColorParameter BlendColor = new ColorParameter(value: Color.white, overrideState: true);

    public ClampedIntParameter BlendType = new ClampedIntParameter(value: 0, min: 0, max: 2, overrideState: true);

    public ClampedFloatParameter strength = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 1.0f, overrideState: true);
	
    public BoolParameter Activation = new BoolParameter(value: false, overrideState: true);


    // Tells when our effect should be rendered
    public bool IsActive() => Activation.value;
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
