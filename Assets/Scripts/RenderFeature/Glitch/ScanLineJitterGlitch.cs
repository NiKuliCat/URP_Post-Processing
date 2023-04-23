using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScanLineJitterGlitch : ScriptableRendererFeature
{
    [Serializable]
    public class Settings
    {
        [Range(0f, 1f)]
        public float intensity;
        [Range(0f, 3f)]
        public float speed;
        [Range(0f, 2f)]
        public float threshold;
        [Range(0f, 1f)]
        public float mulit;

        public ConfigSettings configSettings;
    }

    static int param_id = Shader.PropertyToID("_GlitchParams");

    [Serializable]
    public class ConfigSettings
    {
        public RenderPassEvent PassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        public FilterMode filterMode;
    }

    public Settings settings = new Settings();
    class ScanLineJitterGlitchRenderPass : ScriptableRenderPass
    {
        private Settings m_Settings;
        private Material m_Material;
        public ScanLineJitterGlitchRenderPass(Settings settings)
        {
            m_Settings = settings;
        }
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("ScanLineJitterGlitch");
            m_Material = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Glitch/ScanLineJitterGlitch"));

            if (m_Material == null)
                return;

            var source = renderingData.cameraData.renderer.cameraColorTarget;

            m_Material.SetVector(param_id, new Vector4(m_Settings.intensity, m_Settings.speed, m_Settings.threshold,m_Settings.mulit));

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
        m_RenderPass = new ScanLineJitterGlitchRenderPass(settings);

        // Configures where the render pass should be injected.
        m_RenderPass.renderPassEvent = settings.configSettings.PassEvent;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_RenderPass);
    }
}


