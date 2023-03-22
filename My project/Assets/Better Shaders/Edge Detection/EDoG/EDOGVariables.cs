using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/EdgeDetection/EDOG", typeof(UniversalRenderPipeline))]
public class EDOGVariables : VolumeComponent, IPostProcessComponent
{
    
  
    public ClampedIntParameter superSample = new ClampedIntParameter(value: 1, min: 1, max: 4, overrideState: true);

    public BoolParameter useFlow = new BoolParameter(value: false , overrideState: true);

    public ClampedFloatParameter structureTensorDeviation = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 5.0f, overrideState: true);

    public ClampedFloatParameter lineIntegralDeviation = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 20.0f, overrideState: true);

    public Vector2Parameter lineConvolutionStepSizes = new Vector2Parameter(value: new Vector2(1.0f, 1.0f), overrideState: true);

    public BoolParameter calcDiffBeforeConvolution = new BoolParameter(value: false, overrideState: true);

    public ClampedFloatParameter differenceOfGaussiansDeviation = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 10.0f, overrideState: true);

    public ClampedFloatParameter stdevScale = new ClampedFloatParameter(value: 0.0f, min: 0.1f, max: 5.0f, overrideState: true);

    public ClampedFloatParameter Sharpness = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 100.0f, overrideState: true);

    public ClampedIntParameter thresholdMode = new ClampedIntParameter(value: 0, min: 0, max: 3, overrideState: true);

    public ClampedIntParameter quantizerStep = new ClampedIntParameter(value: 1, min: 1, max: 16, overrideState: true);

    public ClampedFloatParameter whitePoint = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 100.0f, overrideState: true);

    public ClampedFloatParameter softThreshold = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 10.0f, overrideState: true);

    public BoolParameter invert = new BoolParameter(value: false, overrideState: true);

    public BoolParameter smoothEdges = new BoolParameter(value: false, overrideState: true);

    public ClampedFloatParameter edgeSmoothDeviation = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 10.0f, overrideState: true);

    public Vector2Parameter edgeSmoothStepSizes = new Vector2Parameter(value: new Vector2(1.0f, 1.0f), overrideState: true);

    public BoolParameter enableHatching = new BoolParameter(value: false, overrideState: true);
    public TextureParameter hatchTexture = new TextureParameter(value: null, overrideState: true);

    public ClampedFloatParameter hatchResolution = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 8.0f, overrideState: true);

    public ClampedFloatParameter hatchRotation = new ClampedFloatParameter(value: 0.0f, min: -180.0f, max: 180.0f, overrideState: true);
    
    public BoolParameter enableSecondLayer = new BoolParameter(value: false, overrideState: true);

    public ClampedFloatParameter secondWhitePoint = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 100.0f, overrideState: true);

    public ClampedFloatParameter hatchResolution2 = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 6.0f, overrideState: true);

    public ClampedFloatParameter secondHatchRotation = new ClampedFloatParameter(value: 0.0f, min: -180.0f, max: 180.0f, overrideState: true);

    public BoolParameter enableThirdLayer = new BoolParameter(value: false, overrideState: true);

    public ClampedFloatParameter thirdWhitePoint = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 100.0f, overrideState: true);

    public ClampedFloatParameter hatchResolution3 = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 8.0f, overrideState: true);
 
    public ClampedFloatParameter thirdHatchRotation = new ClampedFloatParameter(value: 0.0f, min: -180.0f, max: 180.0f, overrideState: true);

    public BoolParameter enableFourthLayer = new BoolParameter(value: false, overrideState: true);

    public ClampedFloatParameter fourthWhitePoint = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 100.0f, overrideState: true);

    public ClampedFloatParameter hatchResolution4 = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 8.0f, overrideState: true);

    public ClampedFloatParameter fourthHatchRotation = new ClampedFloatParameter(value: 0.0f, min: -180.0f, max: 180.0f, overrideState: true);

    public BoolParameter enableColoredPencil = new BoolParameter(value: false, overrideState: true);

    public ClampedFloatParameter brightnessOffset = new ClampedFloatParameter(value: 0.0f, min: -1.0f, max: 1.0f, overrideState: true);

    public ClampedFloatParameter saturation = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 5.0f, overrideState: true);

    public ClampedFloatParameter termStrength = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 5.0f, overrideState: true);

    public ClampedIntParameter blendMode = new ClampedIntParameter(value: 0, min: 0, max: 2, overrideState: true);

    public ColorParameter minColor = new ColorParameter(value: new Color(0.0f, 0.0f, 0.0f), overrideState: true);

    public ColorParameter maxColor = new ColorParameter(value: new Color(1.0f, 1.0f, 1.0f), overrideState: true);

    public ClampedFloatParameter blendStrength = new ClampedFloatParameter(value: 0.0f, min: 0.0f, max: 2.0f, overrideState: true);

    public BoolParameter Activation = new BoolParameter(value: false, overrideState: true);



    public bool IsActive() {
        //Debug.Log("being called");
        return Activation.value;
    }
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
