using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class DitheringPass : ScriptableRenderPass
{
	// Used to render from camera to post processings
	// back and forth, until we render the final image to
	// the camera
    RenderTargetIdentifier source;
    RenderTargetIdentifier destinationA;
    RenderTargetIdentifier destinationB;
    RenderTargetIdentifier latestDest;

    readonly int temporaryRTIdA = Shader.PropertyToID("_TempRT");
    readonly int temporaryRTIdB = Shader.PropertyToID("_TempRTB");

    public DitheringPass()
    {
        // Set the render pass event
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        // Grab the camera target descriptor. We will use this when creating a temporary render texture.
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        descriptor.depthBufferBits = 0;

        var renderer = renderingData.cameraData.renderer;
        source = renderer.cameraColorTarget;

        // Create a temporary render texture using the descriptor from above.
        cmd.GetTemporaryRT(temporaryRTIdA , descriptor, FilterMode.Bilinear);
        destinationA = new RenderTargetIdentifier(temporaryRTIdA);
        cmd.GetTemporaryRT(temporaryRTIdB , descriptor, FilterMode.Bilinear);
        destinationB = new RenderTargetIdentifier(temporaryRTIdB);
    }
    
    // The actual execution of the pass. This is where custom rendering occurs.
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
    	// Skipping post processing rendering inside the scene View
        if(renderingData.cameraData.isSceneViewCamera)
            return;
        
        // Here you get your materials from your custom class
        // (It's up to you! But here is how I did it)
        var materials = DitheringMaterial.Instance;
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


		// Starts with the camera source
        latestDest = source;

        //---Custom effect here---
        var customEffect = stack.GetComponent<DitheringVariables>();
        // Only process if the effect is active
        if (customEffect.IsActive())
        {

            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            var mat = materials.customEffect;
            mat.SetFloat("_Spread", customEffect.spread.value);
            mat.SetInt("_RedColorCount", customEffect.redColorCount.value);
            mat.SetInt("_GreenColorCount", customEffect.greenColorCount.value);
            mat.SetInt("_BlueColorCount", customEffect.blueColorCount.value);
            mat.SetInt("_BayerLevel", customEffect.bayerLevel.value);
            //get the width and height of the textu
            int width = descriptor.width;
            int height = descriptor.height;
            RenderTexture[] rt=new RenderTexture[customEffect.downSamples.value];
            var currentSource=rt[0]=RenderTexture.GetTemporary(width,height,0);
            Blit(cmd, source, currentSource, mat, 0);
            

            for(int i=1;i<customEffect.downSamples.value;i++)
            {
                width/=2;
                height/=2;
                if(height<2)
                {
                    break;
                }
                
                RenderTexture currentDest=rt[i]=RenderTexture.GetTemporary(width,height,0);
                if(customEffect.pointFilterDown.value)
                {
                    Blit(cmd, currentSource, currentDest, mat, 1);
                }
                else
                {
                    Blit(cmd, currentSource, currentDest);
                }
                currentSource=currentDest;
            }

            RenderTexture Dither=RenderTexture.GetTemporary(width,height,0);
            Blit(cmd, currentSource, Dither, mat, 0);
            Blit(cmd,Dither,source,mat,1);
            RenderTexture.ReleaseTemporary(Dither);
            for(int i=customEffect.downSamples.value-1;i>=0;i--)
            {
                RenderTexture.ReleaseTemporary(rt[i]);
            }

                

        }
        
        // Add any other custom effect/component you want, in your preferred order
        // Custom effect 2, 3 , ...

		
		// DONE! Now that we have processed all our custom effects, applies the final result to camera
        //Blit(cmd, latestDest, source);
        
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

