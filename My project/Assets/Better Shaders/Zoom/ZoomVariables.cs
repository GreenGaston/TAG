using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/ZoomVariables", typeof(UniversalRenderPipeline))]
public class ZoomVariables : VolumeComponent, IPostProcessComponent
{

    public ClampedIntParameter zoomMode = new ClampedIntParameter(0, 0, 2);

    public ClampedFloatParameter zoom = new ClampedFloatParameter(0.0f, 0.0f, 2.0f);

    public Vector2Parameter offset = new Vector2Parameter(Vector2.zero);

    public ClampedFloatParameter rotation = new ClampedFloatParameter(0.0f, -180.0f, 180.0f);


	
    public BoolParameter Activation = new BoolParameter(value: false, overrideState: true);
    // Tells when our effect should be rendered
    public bool IsActive() => Activation.value;
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
