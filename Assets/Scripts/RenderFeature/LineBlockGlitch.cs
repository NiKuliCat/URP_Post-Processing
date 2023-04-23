using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LineBlockGlitch : ScriptableRendererFeature
{
    [Serializable]
    public class Settings
    {
        [Header("Glitch")]
        [Range(0f, 3f)]
        public float speed;
        [Range(0f, 2f)]
        public float mulit;

        [Header("Line"),Space(5)]
        [Range(0f, 10f)]
        public float lienWidth;
        [Range(0f, 1f)]
        public float offset;
        [Range(0f, 1f)]
        public float alpha;
        [Range(0f, 10f)]
        public float frequency;

        public ConfigSettings configSettings;
    }

    static int Gparam_id = Shader.PropertyToID("_GlitchParams");
    static int Lparam_id = Shader.PropertyToID("_LineParams");

    [Serializable]
    public class ConfigSettings
    {
        public RenderPassEvent PassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        public FilterMode filterMode;
    }

    public Settings settings = new Settings();
    class LineBlockGlitchRenderPass : ScriptableRenderPass
    {
        private Settings m_Settings;
        private Material m_Material;
        public LineBlockGlitchRenderPass(Settings settings)
        {
            m_Settings = settings;
        }

        private float time = 0f;
        private Vector2 glitchParams;

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("LineBlockGlitch");
            m_Material = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Glitch/LineBlockGlitch"));

            if (m_Material == null)
                return;

            var source = renderingData.cameraData.renderer.cameraColorTarget;
            UpdateParams();
            m_Material.SetVector(Gparam_id, glitchParams);
            m_Material.SetVector(Lparam_id, new Vector4(m_Settings.frequency, m_Settings.lienWidth, m_Settings.offset, m_Settings.alpha));
            cmd.Blit(source, source, m_Material, 0);

            context.ExecuteCommandBuffer(cmd);


            cmd.Clear();
            cmd.Release();
        }
        public void UpdateParams()
        {
            time += Time.deltaTime;
            if (time > 100f)
            {
                time = 0f;
            }
            glitchParams = new Vector2(time * m_Settings.speed, m_Settings.mulit);
        }
        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    LineBlockGlitchRenderPass m_RenderPass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_RenderPass = new LineBlockGlitchRenderPass(settings);

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


