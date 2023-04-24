using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RGBSplitGlitchRenderFeature : ScriptableRendererFeature
{


    [Serializable]
    public class ConfigSettings
    {
        public RenderPassEvent PassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        public FilterMode filterMode;
    }


    static int param_id = Shader.PropertyToID("_GlitchParams");


    public ConfigSettings settings = new ConfigSettings();

    RGBSplitGlitchRenderPass m_RenderPass;





    class RGBSplitGlitchRenderPass : ScriptableRenderPass
    {


        private Material m_Material;

        private RGBSplitGlitchVolume volume;

        public RGBSplitGlitchRenderPass()
        {


            var stack = VolumeManager.instance.stack;
            volume = stack.GetComponent<RGBSplitGlitchVolume>();
        }

        private float time = 0f;
        private Vector4 glitchParams;

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {

        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!volume.IsActive())
                return;


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
            glitchParams = new Vector4(volume.Intensity.value, time * volume.Wave.value, UnityEngine.Random.Range(-1f, 1f), volume.Multi.value);
        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    /// <inheritdoc/>
    public override void Create()
    {
        m_RenderPass = new RGBSplitGlitchRenderPass();

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


