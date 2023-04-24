using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LineBlockGlitch : ScriptableRendererFeature
{
   

    static int Gparam_id = Shader.PropertyToID("_GlitchParams");
    static int Lparam_id = Shader.PropertyToID("_LineParams");

    [Serializable]
    public class ConfigSettings
    {
        public RenderPassEvent PassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        public FilterMode filterMode;
    }

    public ConfigSettings settings = new ConfigSettings();
    class LineBlockGlitchRenderPass : ScriptableRenderPass
    {
        private Material m_Material;
        private LineBlockGlitchVolume volume;
        public LineBlockGlitchRenderPass()
        {
            var stack = VolumeManager.instance.stack;
            volume = stack.GetComponent<LineBlockGlitchVolume>();
        }

        private float time = 0f;
        private Vector2 glitchParams;

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!volume.IsActive())
                return;

            CommandBuffer cmd = CommandBufferPool.Get("LineBlockGlitch");
            m_Material = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Glitch/LineBlockGlitch"));

            if (m_Material == null)
                return;

            var source = renderingData.cameraData.renderer.cameraColorTarget;
            UpdateParams();
            m_Material.SetVector(Gparam_id, glitchParams);
            m_Material.SetVector(Lparam_id, new Vector4(volume.Frequency.value, volume.LineWidth.value, volume.Offset.value, volume.Alpha.value));
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
            glitchParams = new Vector2(time * volume.Wave.value * 0.06f, volume.Multi.value);
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
        m_RenderPass = new LineBlockGlitchRenderPass();

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


