using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class EDOGPass : ScriptableRenderPass
{
	// Used to render from camera to post processings
	// back and forth, until we render the final image to
	// the camera
    RenderTargetIdentifier source;
    RenderTargetIdentifier destinationA;
    RenderTargetIdentifier destinationB;
    RenderTargetIdentifier destinationC;
    RenderTargetIdentifier destinationD;
    RenderTargetIdentifier latestDest;

    readonly int temporaryRTIdA = Shader.PropertyToID("_TempRT");
    readonly int temporaryRTIdB = Shader.PropertyToID("_TempRTB");
    readonly int temporaryRTIdC = Shader.PropertyToID("_TempRTC");
    readonly int temporaryRTIdD = Shader.PropertyToID("_TempRTD");

    public EDOGPass()
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
        cmd.GetTemporaryRT(temporaryRTIdC , descriptor, FilterMode.Bilinear);
        destinationC = new RenderTargetIdentifier(temporaryRTIdC);
        cmd.GetTemporaryRT(temporaryRTIdD , descriptor, FilterMode.Bilinear);
        destinationD = new RenderTargetIdentifier(temporaryRTIdD);

    }
    
    // The actual execution of the pass. This is where custom rendering occurs.
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {

       
    	// Skipping post processing rendering inside the scene View
        if(renderingData.cameraData.isSceneViewCamera)
            return;
        
        // Here you get your materials from your custom class
        // (It's up to you! But here is how I did it)
        var materials = EDOGMaterial.Instance;
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
        var customEffect = stack.GetComponent<EDOGVariables>();
        // Only process if the effect is active
        if (customEffect.IsActive())
        {
            //Debug.Log("Custom Effect is active");
            RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
            var dogMat= materials.customEffect;
            dogMat.SetFloat("_SigmaC", customEffect.structureTensorDeviation.value);
            dogMat.SetFloat("_SigmaE", customEffect.differenceOfGaussiansDeviation.value);
            dogMat.SetFloat("_SigmaM", customEffect.lineIntegralDeviation.value);
            dogMat.SetFloat("_SigmaA", customEffect.edgeSmoothDeviation.value);
            dogMat.SetFloat("_K", customEffect.stdevScale.value);
            dogMat.SetFloat("_Tau", customEffect.Sharpness.value);
            dogMat.SetFloat("_Phi", customEffect.softThreshold.value);
            dogMat.SetFloat("_Threshold", customEffect.whitePoint.value);
            dogMat.SetFloat("_Threshold2", customEffect.secondWhitePoint.value);
            dogMat.SetFloat("_Threshold3", customEffect.thirdWhitePoint.value);
            dogMat.SetFloat("_Threshold4", customEffect.fourthWhitePoint.value);
            dogMat.SetFloat("_Thresholds", customEffect.quantizerStep.value);
            dogMat.SetFloat("_BlendStrength", customEffect.blendStrength.value);
            dogMat.SetFloat("_DoGStrength", customEffect.termStrength.value);
            dogMat.SetFloat("_HatchTexRotation", customEffect.hatchRotation.value);
            dogMat.SetFloat("_HatchTexRotation1", customEffect.secondHatchRotation.value);
            dogMat.SetFloat("_HatchTexRotation2", customEffect.thirdHatchRotation.value);
            dogMat.SetFloat("_HatchTexRotation3", customEffect.fourthHatchRotation.value);
            dogMat.SetFloat("_HatchRes1", customEffect.hatchResolution.value);
            dogMat.SetFloat("_HatchRes2", customEffect.hatchResolution2.value);
            dogMat.SetFloat("_HatchRes3", customEffect.hatchResolution3.value);
            dogMat.SetFloat("_HatchRes4", customEffect.hatchResolution4.value);
            dogMat.SetInt("_EnableSecondLayer", customEffect.enableSecondLayer.value ? 1 : 0);
            dogMat.SetInt("_EnableThirdLayer", customEffect.enableThirdLayer.value ? 1 : 0);
            dogMat.SetInt("_EnableFourthLayer", customEffect.enableFourthLayer.value ? 1 : 0);
            dogMat.SetInt("_EnableColoredPencil", customEffect.enableColoredPencil.value ? 1 : 0);
            dogMat.SetFloat("_BrightnessOffset", customEffect.brightnessOffset.value);
            dogMat.SetFloat("_Saturation", customEffect.saturation.value);
            //new Vector4(lineConvolutionStepSizes.x, lineConvolutionStepSizes.y, edgeSmoothStepSizes.x, edgeSmoothStepSizes.y));
            dogMat.SetVector("_IntegralConvolutionStepSizes", new Vector4(customEffect.lineConvolutionStepSizes.value.x, customEffect.lineConvolutionStepSizes.value.y, customEffect.edgeSmoothStepSizes.value.x, customEffect.edgeSmoothStepSizes.value.y));
            dogMat.SetVector("_MinColor", customEffect.minColor.value);
            dogMat.SetVector("_MaxColor", customEffect.maxColor.value);
            dogMat.SetInt("_Thresholding", (int)customEffect.thresholdMode.value);
            dogMat.SetInt("_BlendMode", (int)customEffect.blendMode.value);
            dogMat.SetInt("_Invert", customEffect.invert.value ? 1 : 0);
            dogMat.SetInt("_CalcDiffBeforeConvolution", customEffect.calcDiffBeforeConvolution.value ? 1 : 0);
            dogMat.SetInt("_HatchingEnabled", customEffect.enableHatching.value ? 1 : 0);
            dogMat.SetTexture("_HatchTex", customEffect.hatchTexture.value);

            Blit(cmd, source, destinationA, dogMat, 0);
            var texture=RenderTexture.GetTemporary(desc);
            if(customEffect.useFlow.value||customEffect.smoothEdges.value){
                Blit(cmd, destinationA, destinationB, dogMat, 1);
                Blit(cmd, destinationB, destinationC, dogMat, 2);
                Blit(cmd, destinationC, destinationD, dogMat, 3);
                
                Blit(cmd, destinationD, texture);
                dogMat.SetTexture("_TFM", texture);
            }

            if(customEffect.useFlow.value){
                Blit(cmd, destinationA, destinationB, dogMat, 4);
                Blit(cmd, destinationB, destinationC, dogMat, 5);
            }
            else{
                Blit(cmd, destinationA, destinationB, dogMat, 6);
                Blit(cmd, destinationB, destinationC, dogMat, 7);
            }

            if(customEffect.smoothEdges.value){
                Blit(cmd, destinationC, destinationD, dogMat, 8);
            }
            else{
                Blit(cmd, destinationC, destinationD);
            }
            var texture2=RenderTexture.GetTemporary(desc);
            Blit(cmd, destinationD, texture2);
            dogMat.SetTexture("_DogTex", texture2);
            

            Blit(cmd, source, destinationB, dogMat, 9);
            Blit(cmd, destinationB,source);

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
        cmd.ReleaseTemporaryRT(temporaryRTIdC);
        cmd.ReleaseTemporaryRT(temporaryRTIdD);
    }
}

