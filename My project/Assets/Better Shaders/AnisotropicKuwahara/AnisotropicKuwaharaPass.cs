using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class AnisotropicKuwaharaPass : ScriptableRenderPass
{

    RenderTargetIdentifier source;
    RenderTargetIdentifier destinationA;
    RenderTargetIdentifier destinationB;
    RenderTargetIdentifier latestDest;

    readonly int temporaryRTIdA = Shader.PropertyToID("_TempRT");
    readonly int temporaryRTIdB = Shader.PropertyToID("_TempRTB");

    public AnisotropicKuwaharaPass()
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
        var materials = AnisotropicKuwaharaMaterials.Instance;
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
        var customEffect = stack.GetComponent<AnisotropicKuwaharaVariables>();
        // Only process if the effect is active
        if (customEffect.IsActive())
        {
            //write effects here
            var mat= materials.customEffect;
        //      kuwaharaMat.SetInt("_KernelSize", kernelSize);
        // kuwaharaMat.SetInt("_N", 8);
        // kuwaharaMat.SetFloat("_Q", sharpness);
        // kuwaharaMat.SetFloat("_Hardness", hardness);
        // kuwaharaMat.SetFloat("_Alpha", alpha);
        // kuwaharaMat.SetFloat("_ZeroCrossing", zeroCrossing);
        // kuwaharaMat.SetFloat("_Zeta", useZeta ? zeta : 2.0f / 2.0f / (kernelSize / 2.0f));

            mat.SetInt("_KernelSize", customEffect.KernelSize.value);
            mat.SetInt("_N", 8);
            mat.SetFloat("_Q", customEffect.Sharpness.value);
            mat.SetFloat("_Hardness", customEffect.Hardness.value);
            mat.SetFloat("_Alpha", customEffect.Alpha.value);
            mat.SetFloat("_ZeroCrossing", customEffect.ZeroCrossing.value);
            mat.SetFloat("_Zeta", customEffect.UseZeta.value ? customEffect.Zeta.value : 2.0f / 2.0f / (customEffect.KernelSize.value / 2.0f));

            var structureTensor = RenderTexture.GetTemporary(renderingData.cameraData.cameraTargetDescriptor);
            Blit(cmd, source, structureTensor, mat, 0);
            var eigenvectors1 = RenderTexture.GetTemporary(renderingData.cameraData.cameraTargetDescriptor);
            Blit(cmd, structureTensor, eigenvectors1, mat, 1);
            var eigenvectors2 = RenderTexture.GetTemporary(renderingData.cameraData.cameraTargetDescriptor);
            Blit(cmd, structureTensor, eigenvectors2, mat, 2);
            mat.SetTexture("_TFM",eigenvectors2);

            RenderTexture[] textures= new RenderTexture[customEffect.Passes.value];
            for (int i = 0; i < customEffect.Passes.value; i++)
            {
                textures[i] = RenderTexture.GetTemporary(renderingData.cameraData.cameraTargetDescriptor);
            }
            Blit(cmd,source,textures[0],mat,3);
            for (int i = 1; i < customEffect.Passes.value; i++)
            {
                Blit(cmd,textures[i-1],textures[i],mat,3);
            }

            Blit(cmd,textures[customEffect.Passes.value-1],source);
            for (int i = 0; i < customEffect.Passes.value; i++)
            {
                RenderTexture.ReleaseTemporary(textures[i]);
            }
            RenderTexture.ReleaseTemporary(structureTensor);
            RenderTexture.ReleaseTemporary(eigenvectors1);
            RenderTexture.ReleaseTemporary(eigenvectors2);


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

