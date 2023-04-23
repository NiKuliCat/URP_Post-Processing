using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlackWhiteFlash : ScriptableRendererFeature
{
    [Serializable]
    public class Settings
    {
        public Material material;
        [Range(0f, 1f)]
        public float intensity;
        [Range(0f, 1f)]
        public float noiseIntensity;
        [Range(0f, 3f)]
        public float noiseWave_U;
        [Range(0f, 3f)]
        public float noiseWave_V;
        public Vector2 center;

        [Range(0,50f)]
        public float radialTilling;
        [Range(0, 50f)]
        public float lengthTilling;


        public ConfigSettings configSettings;
    }

    static int params2_id = Shader.PropertyToID("_Params2");
    static int params1_id = Shader.PropertyToID("_Params1");

    [Serializable]
    public class ConfigSettings
    {
        public RenderPassEvent PassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        public FilterMode filterMode;
    }

    public Settings settings = new Settings();

    class BlackWhiteFlashRenderPass : ScriptableRenderPass
    {
        private Settings m_Settings;
        public BlackWhiteFlashRenderPass(Settings settings)
        {
            m_Settings = settings;
        }
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("BlackWhiteFlash");

            if (m_Settings.material == null)
                return;

            var source = renderingData.cameraData.renderer.cameraColorTarget;

            m_Settings.material.SetVector(params2_id, new Vector4( m_Settings.intensity,m_Settings.noiseIntensity,m_Settings.noiseWave_U, m_Settings.noiseWave_V)); 
            m_Settings.material.SetVector(params1_id, new Vector4(m_Settings.center.x, m_Settings.center.y,m_Settings.radialTilling,m_Settings.lengthTilling));


            cmd.Blit(source, source, m_Settings.material, 0);

            context.ExecuteCommandBuffer(cmd);


            cmd.Clear();
            cmd.Release();
        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    BlackWhiteFlashRenderPass m_ScriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new BlackWhiteFlashRenderPass(settings);

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = settings.configSettings.PassEvent;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


