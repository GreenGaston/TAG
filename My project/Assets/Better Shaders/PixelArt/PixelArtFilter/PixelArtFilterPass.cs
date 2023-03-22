using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class PixelArtFilterPass : ScriptableRenderPass
{

    RenderTargetIdentifier source;
    RenderTargetIdentifier destinationA;
    RenderTargetIdentifier destinationB;
    RenderTargetIdentifier latestDest;

    readonly int temporaryRTIdA = Shader.PropertyToID("_TempRT");
    readonly int temporaryRTIdB = Shader.PropertyToID("_TempRTB");

    public PixelArtFilterPass()
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
        var materials = PixelArtFilterMaterials.Instance;
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
        var customEffect = stack.GetComponent<PixelArtFilterVariables>();
        // Only process if the effect is active
        if (customEffect.IsActive())
        {   
            var mat= materials.customEffect;
            RenderTexture[] rt = new RenderTexture[8];
            var current=source;
            int width = renderingData.cameraData.cameraTargetDescriptor.width;
            int height = renderingData.cameraData.cameraTargetDescriptor.height;

            for(int i=0;i<customEffect.downSamples.value;i++){
               
                width/=2;
                height/=2;

                if (height < 2)
                    break;

                RenderTexture currentDest=rt[i]=RenderTexture.GetTemporary(width,height,0,RenderTextureFormat.ARGB32);
                Blit(cmd,current,currentDest,mat);
                current=currentDest;

            }
            Blit(cmd,current,source,mat);
            for(int i=0;i<customEffect.downSamples.value;i++){
                RenderTexture.ReleaseTemporary(rt[i]);
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

