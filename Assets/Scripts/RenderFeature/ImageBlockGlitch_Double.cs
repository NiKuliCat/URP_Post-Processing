using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ImageBlockGlitch_Double : ScriptableRendererFeature
{
    [Serializable]
    public class ConfigSettings
    {
        public RenderPassEvent PassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        public FilterMode filterMode;
    }
    [Serializable]
    public class BlockSettings
    {
        [Range(0f,30f)]
        public float blockSize_X;
        [Range(0f, 30f)]
        public float blockSize_Y;
        [Range(0f,20f)]
        public float blockIntensity;
    }

    [Serializable]
    public class Settings
    {
        [Range(0f, 1f)]
        public float glitchIntensity;
        [Range(0f, 20f)]
        public float speed;
        [Range(0f, 10f)]
        public float mulit;
        public BlockSettings blockLayer_1 = new BlockSettings();
        public BlockSettings blockLayer_2 = new BlockSettings();
        public ConfigSettings configSettings;
    }

    public Settings settings = new Settings();

    static int param_id = Shader.PropertyToID("_GlitchParams");
    static int blockSize_id = Shader.PropertyToID("_BlockSize");
    static int blockIntensity_id = Shader.PropertyToID("_BlockIntensity");

    ImageBlockGlitch_DoubleRenderPass m_RenderPass;

    class ImageBlockGlitch_DoubleRenderPass : ScriptableRenderPass
    {
        private Settings m_Settings;
        private Material m_Material;
        public ImageBlockGlitch_DoubleRenderPass(Settings settings)
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
            m_Material = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Glitch/ImageBlockGlitch_Double"));

            if (m_Material == null)
                return;

            var source = renderingData.cameraData.renderer.cameraColorTarget;

            UpdateParams();
            m_Material.SetVector(param_id, glitchParams);
            m_Material.SetVector(blockSize_id, new Vector4(m_Settings.blockLayer_1.blockSize_X, m_Settings.blockLayer_1.blockSize_Y, m_Settings.blockLayer_2.blockSize_X, m_Settings.blockLayer_2.blockSize_Y));
            m_Material.SetVector(blockIntensity_id, new Vector2(m_Settings.blockLayer_1.blockIntensity, m_Settings.blockLayer_2.blockIntensity));

            cmd.Blit(source, source, m_Material, 0);

            context.ExecuteCommandBuffer(cmd);


            cmd.Clear();
            cmd.Release();
        }
        public void UpdateParams()
        {
            glitchParams = new Vector4(m_Settings.glitchIntensity, m_Settings.speed, UnityEngine.Random.Range(-1f, 1f), m_Settings.mulit);
        }
        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }


    /// <inheritdoc/>
    public override void Create()
    {
        m_RenderPass = new ImageBlockGlitch_DoubleRenderPass(settings);

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


