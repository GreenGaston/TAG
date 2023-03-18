using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class GeneralizedKuwaharaPass : ScriptableRenderPass
{

    RenderTargetIdentifier source;
    RenderTargetIdentifier destinationA;
    RenderTargetIdentifier destinationB;
    RenderTargetIdentifier latestDest;

    readonly int temporaryRTIdA = Shader.PropertyToID("_TempRT");
    readonly int temporaryRTIdB = Shader.PropertyToID("_TempRTB");

    public GeneralizedKuwaharaPass()
    {

        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {

        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        descriptor.depthBufferBits = 0;

        var renderer = renderingData.cameraData.renderer;
        source = renderer.cameraColorTarget;

        cmd.GetTemporaryRT(temporaryRTIdA , descriptor, FilterMode.Bilinear);
        destinationA = new RenderTargetIdentifier(temporaryRTIdA);
        cmd.GetTemporaryRT(temporaryRTIdB , descriptor, FilterMode.Bilinear);
        destinationB = new RenderTargetIdentifier(temporaryRTIdB);
    }
    

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
    	// Skipping post processing rendering inside the scene View
        if(renderingData.cameraData.isSceneViewCamera)
            return;
        
        // Here you get your materials from your custom class
        // (It's up to you! But here is how I did it)
        var materials = GeneralizedKuwaharaMaterials.Instance;
        if (materials == null)
        {
            Debug.LogError("Custom Post Processing Materials instance is null");
            return;
        }
        
        CommandBuffer cmd = CommandBufferPool.Get("Custom Post Processing");
        cmd.Clear();

		// This holds all the current Volumes information
		// which we will need later
        var stack = VolumeManager.instance.stack;


        //---Custom effect here---
        var customEffect = stack.GetComponent<GeneralizedKuwaharaVariables>();
        // Only process if the effect is active
        if (customEffect.IsActive())
        {

            var mat= materials.customEffect;

            mat.SetInt("_KernelSize", customEffect.KernelSize.value);
            mat.SetInt("_N", 8);
            mat.SetFloat("_Q", customEffect.Sharpness.value);
            mat.SetFloat("_Hardness", customEffect.Hardness.value);
            mat.SetFloat("_ZeroCrossing", customEffect.ZeroCrossing.value);
            mat.SetFloat("_Zeta", customEffect.UseZeta.value ? customEffect.Zeta.value : 2.0f / (customEffect.KernelSize.value / 2.0f));

            RenderTexture[] passes= new RenderTexture[customEffect.Passes.value];

            for(int i=0;i<passes.Length;i++)
            {
                passes[i]= RenderTexture.GetTemporary(renderingData.cameraData.cameraTargetDescriptor);
            }
            Blit(cmd, source, passes[0], mat);
            for(int i=1;i<passes.Length;i++)
            {
                Blit(cmd, passes[i-1], passes[i], mat);
            }

            Blit(cmd, passes[passes.Length-1], source, mat);
            for(int i=0;i<passes.Length;i++)
            {
                RenderTexture.ReleaseTemporary(passes[i]);
            }
        }
        
       
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

	//Cleans the temporary RTs when we don't need them anymore
    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(temporaryRTIdA);
        cmd.ReleaseTemporaryRT(temporaryRTIdB);
    }
}

