using UnityEngine.Rendering.Universal;
//import to use debug.log
using UnityEngine;

[System.Serializable]
public class CustomPostProcessRenderer : ScriptableRendererFeature
{
    CustomPostProcessPass pass;
    EDOGPass pass2;

    public override void Create()
    {
        pass = new CustomPostProcessPass();
    }
    
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        //Debug.Log("Adding Render Passes");	
        renderer.EnqueuePass(pass);
        renderer.EnqueuePass(pass2);
    }
}
