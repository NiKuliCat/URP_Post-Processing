using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ImageBlockGlittch : ScriptableRendererFeature
{

    [Serializable]
    public class ConfigSettings
    {
        public RenderPassEvent PassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        public FilterMode filterMode;
    }

    

    public ConfigSettings settings = new ConfigSettings();

    static int param_id = Shader.PropertyToID("_GlitchParams");
    static int blockSize_id = Shader.PropertyToID("_BlockSize");

    ImageBlockGlitchRenderPass m_RenderPass;

    class ImageBlockGlitchRenderPass : ScriptableRenderPass
    {

        private Material m_Material;
        private ImageBlockGlitchVolume volume;
        public ImageBlockGlitchRenderPass()
        {


            var stack = VolumeManager.instance.stack;
            volume = stack.GetComponent<ImageBlockGlitchVolume>();

        }
        private Vector4 glitchParams;
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {

            if (!volume.IsActive())
                return;

            CommandBuffer cmd = CommandBufferPool.Get("ImageBlockGlitch");
            m_Material = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Glitch/ImageBlockGlitch"));

            if (m_Material == null)
                return;

            var source = renderingData.cameraData.renderer.cameraColorTarget;
            UpdateParams();
            m_Material.SetVector(param_id, glitchParams);
            m_Material.SetFloat(blockSize_id, volume.BlockSize.value);

            cmd.Blit(source, source, m_Material, 0);

            context.ExecuteCommandBuffer(cmd);


            cmd.Clear();
            cmd.Release();
        }
        public void UpdateParams()
        {
            glitchParams = new Vector4(volume.Intensity.value, volume.Wave.value, UnityEngine.Random.Range(-1f, 1f), volume.Multi.value);
        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }


    /// <inheritdoc/>
    public override void Create()
    {
        m_RenderPass = new ImageBlockGlitchRenderPass();

        // Configures where the render pass should be injected.
        m_RenderPass.renderPassEvent = settings.PassEvent;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_RenderPass);
    }
}


