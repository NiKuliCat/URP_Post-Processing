using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RGBSplitGlitchRenderFeature : ScriptableRendererFeature
{

    [Serializable]
    public class Settings
    {
        [Range(0f, 1f)]
        public float intensity;
        [Range(0f, 3f)]
        public float speed;
        [Range(0f, 10f)]
        public float mulit;
        public ConfigSettings configSettings;
    }

    //static int colorBuffer_id = Shader.PropertyToID("_ColorBuffer");
    static int param_id = Shader.PropertyToID("_GlitchParams");

    [Serializable]
    public class ConfigSettings
    {
        public RenderPassEvent PassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        public FilterMode filterMode;
    }

    public Settings settings = new Settings();

    RGBSplitGlitchRenderPass m_RenderPass;





    class RGBSplitGlitchRenderPass : ScriptableRenderPass
    {

        private Settings m_Settings;
        private Material m_Material;
        public RGBSplitGlitchRenderPass(Settings settings)
        {
            m_Settings = settings;
        }

        private float time = 0f;
        private Vector4 glitchParams;

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {

        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("RGBSplitGlitch");
            m_Material = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Glitch/RGBSplitGlitch"));

            if (m_Material == null)
                return;

            var source = renderingData.cameraData.renderer.cameraColorTarget;

            UpdateParams();

            m_Material.SetVector(param_id, glitchParams);

            cmd.Blit(source,source, m_Material,0);

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
            glitchParams = new Vector4(m_Settings.intensity, time * m_Settings.speed, UnityEngine.Random.Range(-1f, 1f), m_Settings.mulit);
        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    /// <inheritdoc/>
    public override void Create()
    {
        m_RenderPass = new RGBSplitGlitchRenderPass(settings);

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


