using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/PixelArt/PaletteSwapVariables", typeof(UniversalRenderPipeline))]
public class PaletteSwapVariables : VolumeComponent, IPostProcessComponent
{

    public TextureParameter colorPalette = new TextureParameter(null);
    public BoolParameter invert = new BoolParameter(value: false);
	
    public BoolParameter Activation = new BoolParameter(value: false, overrideState: true);
    // Tells when our effect should be rendered

    public bool IsActive() => Activation.value;
   
   	// I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
