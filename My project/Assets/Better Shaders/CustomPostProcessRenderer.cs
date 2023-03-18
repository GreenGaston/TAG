using UnityEngine.Rendering.Universal;
//import to use debug.log
using UnityEngine;

[System.Serializable]
public class CustomPostProcessRenderer : ScriptableRendererFeature
{
    CustomPostProcessPass pass;
    EDOGPass pass2;
    GoochPass pass3;
    DitheringPass pass4;
    BlendModePass pass5;
    BloomPass pass6;
    ColorBlindnessPass pass7;
    ColorCorrectionPass pass8;
    GammaPass pass9;
    HueShiftPass pass10;
    EdgeDetectPass pass11;
    FogPass pass12;
    BlitterPass pass13;
    AnisotropicKuwaharaPass pass14;
    KuwaharaPass pass15;
    GeneralizedKuwaharaPass pass16;
    PaletteSwapPass pass17;
    PixelArtFilterPass pass18;
    SharpnessPass pass19;
    ZoomPass pass20;
    ToneMappingPass pass21;
    public override void Create()
    {
        pass = new CustomPostProcessPass();
        pass2 = new EDOGPass();
        pass3 = new GoochPass();
        pass4 = new DitheringPass();
        pass5 = new BlendModePass();
        pass6 = new BloomPass();
        pass7 = new ColorBlindnessPass();
        pass8 = new ColorCorrectionPass();
        pass9 = new GammaPass();
        pass10 = new HueShiftPass();
        pass11 = new EdgeDetectPass();
        pass12 = new FogPass();
        pass13 = new BlitterPass();
        pass14 = new AnisotropicKuwaharaPass();
        pass15 = new KuwaharaPass();
        pass16 = new GeneralizedKuwaharaPass();
        pass17 = new PaletteSwapPass();
        pass18 = new PixelArtFilterPass();
        pass19 = new SharpnessPass();
        pass20 = new ZoomPass();
        pass21 = new ToneMappingPass();

        
    }
    
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        //Debug.Log("Adding Render Passes");	
        renderer.EnqueuePass(pass);
        renderer.EnqueuePass(pass2);
        renderer.EnqueuePass(pass3);
        renderer.EnqueuePass(pass4);
        renderer.EnqueuePass(pass5);
        renderer.EnqueuePass(pass6);
        renderer.EnqueuePass(pass7);
        renderer.EnqueuePass(pass8);
        renderer.EnqueuePass(pass9);
        renderer.EnqueuePass(pass10);
        renderer.EnqueuePass(pass11);
        renderer.EnqueuePass(pass12);
        renderer.EnqueuePass(pass13);
        renderer.EnqueuePass(pass14);
        renderer.EnqueuePass(pass15);
        renderer.EnqueuePass(pass16);
        renderer.EnqueuePass(pass17);
        renderer.EnqueuePass(pass18);
        renderer.EnqueuePass(pass19);
        renderer.EnqueuePass(pass20);
        renderer.EnqueuePass(pass21);
    }
}
