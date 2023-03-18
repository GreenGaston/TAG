using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class ToneMappingPass : ScriptableRenderPass
{

    RenderTargetIdentifier source;
    RenderTargetIdentifier destinationA;
    RenderTargetIdentifier destinationB;
    RenderTargetIdentifier latestDest;

    readonly int temporaryRTIdA = Shader.PropertyToID("_TempRT");
    readonly int temporaryRTIdB = Shader.PropertyToID("_TempRTB");

    public ToneMappingPass()
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
        var materials = ToneMappingMaterials.Instance;
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
        var customEffect = stack.GetComponent<ToneMappingVariables>();
        // Only process if the effect is active
        if (customEffect.IsActive())
        {
            //write effects here
            var mat = materials.customEffect;
        
            mat.SetFloat("_Ldmax", customEffect.Ldmax.value);
            mat.SetFloat("_Cmax", customEffect.Cmax.value);
            mat.SetFloat("_P", customEffect.p.value);
            mat.SetFloat("_HiVal", customEffect.hiVal.value);
            mat.SetFloat("_Cwhite", customEffect.Cwhite.value);
            mat.SetFloat("_A", customEffect.shoulderStrength.value);
            mat.SetFloat("_B", customEffect.linearStrength.value);
            mat.SetFloat("_C", customEffect.linearAngle.value);
            mat.SetFloat("_D", customEffect.toeStrength.value);
            mat.SetFloat("_E", customEffect.toeNumerator.value);
            mat.SetFloat("_F", customEffect.toeDenominator.value);
            mat.SetFloat("_W", customEffect.linearWhitePoint.value);
            mat.SetFloat("_M", customEffect.maxBrightness.value);
            mat.SetFloat("_a", customEffect.contrast.value);
            mat.SetFloat("_m", customEffect.linearStart.value);
            mat.SetFloat("_l", customEffect.linearLength.value);
            mat.SetFloat("_c", customEffect.blackTightnessShape.value);
            mat.SetFloat("_b", customEffect.blackTightnessOffset.value);

            RenderTexture grayscale = new RenderTexture(1920, 1080, 0, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear);
            grayscale.useMipMap = true;
            grayscale.Create();

            mat.SetTexture("_LuminanceTex", grayscale);

            Blit(cmd,source, destinationA, mat, (int)customEffect.mode.value);
            Blit(cmd,destinationA,source);





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

