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
        public ConfigSettings configSettings;
    }

    static int params2_id = Shader.PropertyToID("_Params2");
    static int params1_id = Shader.PropertyToID("_Params1");

    static int Color1_id = Shader.PropertyToID("_FlashColor_1");
    static int Color2_id = Shader.PropertyToID("_FlashColor_2");

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
        private Black_WhiteFlashVolume volume;

        public BlackWhiteFlashRenderPass(Settings settings)
        {
            m_Settings = settings;

            var stack = VolumeManager.instance.stack;
            volume = stack.GetComponent<Black_WhiteFlashVolume>();
        }
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!volume.IsActive())
                return;


            CommandBuffer cmd = CommandBufferPool.Get("BlackWhiteFlash");

            if (m_Settings.material == null)
                return;

            var source = renderingData.cameraData.renderer.cameraColorTarget;

            m_Settings.material.SetVector(params2_id, new Vector4( volume.Intensity.value, volume.Intensity.value * volume.NoiseIntensity.value,volume.NoiseWave_U.value, volume.NoiseWave_V.value)); 
            m_Settings.material.SetVector(params1_id, new Vector4(volume.Center_X_SS.value, volume.Center_Y_SS.value,volume.RadialTilling.value,volume.LengthTilling.value));
            m_Settings.material.SetColor(Color1_id, volume.BlackTintColor.value);
            m_Settings.material.SetColor(Color2_id, volume.WhiteTintColor.value);

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


