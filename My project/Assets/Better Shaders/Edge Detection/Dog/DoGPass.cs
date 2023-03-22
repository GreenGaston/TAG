using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class DoGPass : ScriptableRenderPass
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

    public DoGPass()
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
        var materials = DoGMaterial.Instance;
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
        var customEffect = stack.GetComponent<DoGVariables>();
        // Only process if the effect is active
        if (customEffect.IsActive())
        {
            //get the source rendertexture from the rendertexture identifier
            
            RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
            var texture = RenderTexture.GetTemporary(desc);
            cmd.Blit(source, texture);

            
            var dogMat = materials.customEffect;
            //cmd.Blit(texture ,source);
 
            //dogMat.SetTexture("_MainTex",texture);
            
            //set the shader properties
            dogMat.SetInt("_GaussianKernelSize", customEffect.gaussianKernelSize.value);
            //Debug.Log(customEffect.gaussianKernelSize.value);
            dogMat.SetFloat("_Sigma", customEffect.stdev.value);
            dogMat.SetFloat("_K", customEffect.stdevScale.value);
            dogMat.SetFloat("_Tau", customEffect.tau.value);
            dogMat.SetFloat("_Phi", customEffect.phi.value);
            dogMat.SetFloat("_Threshold", customEffect.threshold.value);
            dogMat.SetInt("_Thresholding", customEffect.thresholding.value?1:0);
            dogMat.SetInt("_Invert", customEffect.invert.value?1:0);
            dogMat.SetInt("_Tanh",customEffect.tanh.value?1:0);


            //first pass
            Blit(cmd, source, destinationA, dogMat, 0);
            //second pass
            Blit(cmd, destinationA, destinationB, dogMat, 1);

            var texture2 = RenderTexture.GetTemporary(desc);
            cmd.Blit(destinationB, texture2);

            dogMat.SetTexture("_GaussianTex",texture2);

            //blit from the source to the destination using the third pass of the material
            Blit(cmd, source, destinationA, dogMat, 2);
            Blit(cmd, destinationA, source);
            latestDest = source;
            RenderTexture.ReleaseTemporary(texture);
            RenderTexture.ReleaseTemporary(texture2);

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

