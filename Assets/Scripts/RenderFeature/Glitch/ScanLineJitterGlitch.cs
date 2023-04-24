using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScanLineJitterGlitch : ScriptableRendererFeature
{
    

    static int param_id = Shader.PropertyToID("_GlitchParams");

    [Serializable]
    public class ConfigSettings
    {
        public RenderPassEvent PassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        public FilterMode filterMode;
    }

    public ConfigSettings settings = new ConfigSettings();
    class ScanLineJitterGlitchRenderPass : ScriptableRenderPass
    {
        private Material m_Material;
        private ScanLineJitterGlitchVolume volume;
        public ScanLineJitterGlitchRenderPass()
        {
            var stack = VolumeManager.instance.stack;
            volume = stack.GetComponent<ScanLineJitterGlitchVolume>();
        }
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!volume.IsActive())
                return;


            CommandBuffer cmd = CommandBufferPool.Get("ScanLineJitterGlitch");
            m_Material = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Glitch/ScanLineJitterGlitch"));

            if (m_Material == null)
                return;

            var source = renderingData.cameraData.renderer.cameraColorTarget;

            m_Material.SetVector(param_id, new Vector4(volume.Intensity.value, volume.Wave.value, volume.Threshold.value,volume.Multi.value));

            cmd.Blit(source, source, m_Material, 0);

            context.ExecuteCommandBuffer(cmd);


            cmd.Clear();
            cmd.Release();

        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    ScanLineJitterGlitchRenderPass m_RenderPass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_RenderPass = new ScanLineJitterGlitchRenderPass();

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


