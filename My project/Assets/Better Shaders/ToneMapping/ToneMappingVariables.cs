using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/ToneMappingVariables", typeof(UniversalRenderPipeline))]
public class ToneMappingVariables : VolumeComponent, IPostProcessComponent
{
	public ClampedIntParameter mode = new ClampedIntParameter(1, 1, 11);

    public ClampedFloatParameter Ldmax = new ClampedFloatParameter(1.0f, 1.0f, 300.0f);
    public ClampedFloatParameter Cmax = new ClampedFloatParameter(1.0f, 1.0f, 100.0f);
    public ClampedFloatParameter p = new ClampedFloatParameter(1.0f, 1.0f, 100.0f);
    public ClampedFloatParameter hiVal = new ClampedFloatParameter(1.0f, 1.0f, 150.0f);
    public ClampedFloatParameter Cwhite = new ClampedFloatParameter(1.0f, 1.0f, 60.0f);
    public ClampedFloatParameter shoulderStrength = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
    public ClampedFloatParameter linearStrength = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
    public ClampedFloatParameter linearAngle = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
    public ClampedFloatParameter toeStrength = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
    public ClampedFloatParameter toeNumerator = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
    public ClampedFloatParameter toeDenominator = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
    public ClampedFloatParameter linearWhitePoint = new ClampedFloatParameter(0.0f, 0.0f, 60.0f);
    public ClampedFloatParameter maxBrightness = new ClampedFloatParameter(1.0f, 1.0f, 100.0f);
    public ClampedFloatParameter contrast = new ClampedFloatParameter(0.0f, 0.0f, 5.0f);
    public ClampedFloatParameter linearStart = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
    public ClampedFloatParameter linearLength = new ClampedFloatParameter(0.01f, 0.01f, 0.99f);
    public ClampedFloatParameter blackTightnessShape = new ClampedFloatParameter(1.0f, 1.0f, 3.0f);
    public ClampedFloatParameter blackTightnessOffset = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);


    public BoolParameter Activation = new BoolParameter(value: false, overrideState: true);
    // Tells when our effect should be rendered
    public bool IsActive() => Activation.value;
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
