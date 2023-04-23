using System;
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

    [Serializable]
    public class Settings
    {
        [Range(0f, 1f)]
        public float intensity;
        [Range(0f, 20f)]
        public float speed;
        [Range(0f, 10f)]
        public float mulit;
        [Range(0f, 20f)]
        public float blockSize;

        public ConfigSettings configSettings;
    }

    public Settings settings = new Settings();

    static int param_id = Shader.PropertyToID("_GlitchParams");
    static int blockSize_id = Shader.PropertyToID("_BlockSize");

    ImageBlockGlitchRenderPass m_RenderPass;

    class ImageBlockGlitchRenderPass : ScriptableRenderPass
    {
        private Settings m_Settings;
        private Material m_Material;
        public ImageBlockGlitchRenderPass(Settings settings)
        {
            m_Settings = settings;
        }
        private Vector4 glitchParams;
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("RGBSplitGlitch");
            m_Material = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Glitch/ImageBlockGlitch"));

            if (m_Material == null)
                return;

            var source = renderingData.cameraData.renderer.cameraColorTarget;
            UpdateParams();
            m_Material.SetVector(param_id, glitchParams);
            m_Material.SetFloat(blockSize_id, m_Settings.blockSize);

            cmd.Blit(source, source, m_Material, 0);

            context.ExecuteCommandBuffer(cmd);


            cmd.Clear();
            cmd.Release();
        }
        public void UpdateParams()
        {
            glitchParams = new Vector4(m_Settings.intensity, m_Settings.speed, UnityEngine.Random.Range(-1f, 1f), m_Settings.mulit);
        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }


    /// <inheritdoc/>
    public override void Create()
    {
        m_RenderPass = new ImageBlockGlitchRenderPass(settings);

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


