using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class BloomPass : ScriptableRenderPass
{

    RenderTargetIdentifier source;
    RenderTargetIdentifier destinationA;
    RenderTargetIdentifier destinationB;
    RenderTargetIdentifier latestDest;

    readonly int temporaryRTIdA = Shader.PropertyToID("_TempRT");
    readonly int temporaryRTIdB = Shader.PropertyToID("_TempRTB");

    public BloomPass()
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
        var materials = BloomMaterials.Instance;
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
        var customEffect = stack.GetComponent<BloomVariables>();
        // Only process if the effect is active
        if (customEffect.IsActive())
        {
            var Mat=materials.customEffect;
             //     bloomMat.SetFloat("_Threshold", threshold);
        // bloomMat.SetFloat("_SoftThreshold", softThreshold);
        // bloomMat.SetFloat("_DownDelta", downSampleDelta);
        // bloomMat.SetFloat("_UpDelta", upSampleDelta);
        // bloomMat.SetTexture("_OriginalTex", source);
        // bloomMat.SetFloat("_Intensity", bloomIntensity);
        //copy this
            // Set the material properties
            Mat.SetFloat("_Threshold", customEffect.Threshold.value);
            Mat.SetFloat("_SoftThreshold", customEffect.softThreshold.value);
            Mat.SetFloat("_DownDelta", customEffect.downSampleDelta.value);
            Mat.SetFloat("_UpDelta", customEffect.upSampleDelta.value);
            Mat.SetFloat("_Intensity", customEffect.intensity.value);
            
            RenderTexture[] rt = new RenderTexture[18];
            int width = renderingData.cameraData.cameraTargetDescriptor.width;
            int height = renderingData.cameraData.cameraTargetDescriptor.height;

            var currentDest=rt[0] = RenderTexture.GetTemporary(width, height, 0);
            Blit(cmd,source, currentDest, Mat, 0);
            var currentSource = currentDest;
            int i=1;
            for (; i < customEffect.downSamples.value; i++)
            {
                width /= 2;
                height /= 2;
                if (height < 2)
                    break;
                currentDest = rt[i] = RenderTexture.GetTemporary(width, height, 0);
                Blit(cmd, currentSource, currentDest, Mat, 1);
                currentSource = currentDest;
            }

            for(i-=2;i>=0;i--)
            {
                currentDest = rt[i];
                rt[i] = null;
                Blit(cmd,currentSource, currentDest, Mat, 2);
                RenderTexture.ReleaseTemporary(currentSource);
                currentSource = currentDest;
            }
            var _OriginalTex=RenderTexture.GetTemporary(renderingData.cameraData.cameraTargetDescriptor.width, renderingData.cameraData.cameraTargetDescriptor.height, 0);
            Blit(cmd,source, _OriginalTex);
            Mat.SetTexture("_OriginalTex", _OriginalTex);
            RenderTexture.ReleaseTemporary(_OriginalTex);
            Blit(cmd,currentSource, source, Mat, 3);
            RenderTexture.ReleaseTemporary(currentSource);
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

