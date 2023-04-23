using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


namespace PineappleLearning
{
    public class BlurRenderFeature : ScriptableRendererFeature
    {
        [Serializable]
        public class Settings
        {
            public RenderPassEvent PassEvent = RenderPassEvent.AfterRenderingPostProcessing;
            public BlurType blurType;
            public FilterMode filterMode;

        }

        static int blurBuffer_id_1 = Shader.PropertyToID("_BlurBuffer1");
        static int blurBuffer_id_2 = Shader.PropertyToID("_BlurBuffer2");
        static int BlurRadius_id = Shader.PropertyToID("_BlurRadius");

        //BokehBlur
        static int GoldenRot_id = Shader.PropertyToID("_GoldenRot");


        public Settings settings = new Settings();
        BlurRenderPass blurRenderPass;

        class BlurRenderPass : ScriptableRenderPass
        {
            Material blurMaterial;

            BlurType blurType;
            FilterMode filterMode;
            Blur blurVolume;
            #region DualKawase

            Level[] BlurLevels;
            const int MAX_LEVEL = 50;
            struct Level
            {
                internal int down;
                internal int up;
            }
            public void InitBlurLevels()
            {
                BlurLevels = new Level[MAX_LEVEL];
                for (int i = 0;i < MAX_LEVEL;i++)
                {
                    BlurLevels[i] = new Level()
                    {
                        down = Shader.PropertyToID("_DualBlurDown" + i),
                        up = Shader.PropertyToID("_DualBlurUp" + i)
                    };
                }
            }

            #endregion

            #region BokehBlur
            Vector4 goldenRot = new Vector4();

            public void InitGoldenRot()
            {
                float c = Mathf.Cos(2.39996323f);
                float s = Mathf.Sin(2.39996323f);
                goldenRot.Set(c, s, -s, c);
            }

            #endregion
            public BlurRenderPass(Settings settings)
            {
                //获取volume组件
                var stack = VolumeManager.instance.stack;
                blurVolume = stack.GetComponent<Blur>();
             
                blurType = settings.blurType;
                filterMode = settings.filterMode;

                blurMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Blur/GaussianBlur"));

                //初始化必要变量
                InitBlurLevels();
                InitGoldenRot();
            }

                
            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                RenderTextureDescriptor cameraData = renderingData.cameraData.cameraTargetDescriptor;


                int width = (int)(cameraData.width / blurVolume.DownSampler.value);
                int height = (int)(cameraData.height / blurVolume.DownSampler.value);
                //准备需要的资源
                cmd.GetTemporaryRT(blurBuffer_id_1, width,height,0, filterMode);
                cmd.GetTemporaryRT(blurBuffer_id_2, width, height, 0, filterMode);

                if(blurType == BlurType.DualKawaseBlur)
                {
                    for(int i = 0; i < blurVolume.Iteration.value; i++)
                    {
                        int down_id = BlurLevels[i].down;
                        int up_id = BlurLevels[i].up;

                        cmd.GetTemporaryRT(down_id, width, height, 0, filterMode);
                        cmd.GetTemporaryRT(up_id, width, height, 0, filterMode);


                        width = Mathf.Max(width / 2, 1);
                        height = Mathf.Max(height / 2, 1);
                    }

                }

            }
            public override void OnCameraCleanup(CommandBuffer cmd)
            {
                //释放资源
                cmd.Clear();
                cmd.ReleaseTemporaryRT(blurBuffer_id_1);
                cmd.ReleaseTemporaryRT(blurBuffer_id_2);

            }
          
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {

                if (!blurVolume.IsActive())
                    return;
                //执行pass
                if (blurMaterial == null)
                {
                    UnityEngine.Debug.Log("blurMaterial is null !");
                    return;
                }

                CommandBuffer cmd = CommandBufferPool.Get("Blur");

                //寻找对应shader进行操作
                switch(blurType)
                {
                    case BlurType.GaussianBlur:
                        blurMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Blur/GaussianBlur"));
                        Gaussian_Box_Tent_Blur(cmd, context, ref renderingData);
                        break;
                    case BlurType.TentBlur:
                        blurMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Blur/TentBlur"));
                        Gaussian_Box_Tent_Blur(cmd, context, ref renderingData);
                        break;
                    case BlurType.BoxBlur:
                        blurMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Blur/BoxBlur"));
                        Gaussian_Box_Tent_Blur(cmd, context, ref renderingData);
                        break;
                    case BlurType.KawaseBlur:
                        blurMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Blur/KawaseBlur"));
                        Kawase_Blur(cmd, context, ref renderingData);
                        break;
                    case BlurType.DualKawaseBlur:
                        blurMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Blur/DualKawaseBlur"));
                        DualKawase_Blur(cmd,context, ref renderingData);
                        break;
                    case BlurType.BokehBlur:
                        blurMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Blur/BokehBlur"));
                        Bokeh_TiltShift_LrisBlur(cmd, context, ref renderingData);
                        break;
                    case BlurType.TiltShiftBlur:
                        blurMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Blur/TiltShiftBlur"));
                        Bokeh_TiltShift_LrisBlur(cmd, context, ref renderingData);
                        break;
                    case BlurType.LrisBlur:
                        blurMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Blur/LrisBlur"));
                        Bokeh_TiltShift_LrisBlur(cmd, context, ref renderingData);
                        break;
                    case BlurType.GrainyBlur:
                        blurMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Blur/GrainyBlur"));
                        GrainyBlur(cmd, context, ref renderingData);
                        break;
                    case BlurType.RadialBlur:
                        blurMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Pineapple/Post-Processing/Blur/RadialBlur"));
                        RadialBlur(cmd, context, ref renderingData);
                        break;

                }
            }
            public void Gaussian_Box_Tent_Blur(CommandBuffer cmd, ScriptableRenderContext context,ref RenderingData renderingData)
            {
                var source = renderingData.cameraData.renderer.cameraColorTarget;
                var descriptor = renderingData.cameraData.cameraTargetDescriptor;
                //将摄像机颜色复制给buffer1
                cmd.Blit(source, blurBuffer_id_1);

                blurMaterial.SetVector(BlurRadius_id, new Vector4(blurVolume.BlurRadius.value / descriptor.width, blurVolume.BlurRadius.value / descriptor.height, 0, 0));

                for (int i = 0; i < blurVolume.Iteration.value; i++)
                {
                    //horizontal
                    cmd.Blit(blurBuffer_id_1, blurBuffer_id_2, blurMaterial, 0);

                    //vertical
                    cmd.Blit(blurBuffer_id_2, blurBuffer_id_1, blurMaterial, 1);

                }

                cmd.Blit(blurBuffer_id_1, source);

                context.ExecuteCommandBuffer(cmd);


                cmd.Clear();
                cmd.Release();
            }

            public void Kawase_Blur(CommandBuffer cmd, ScriptableRenderContext context,ref RenderingData renderingData)
            {

                //获取摄像机颜色输出
                var source = renderingData.cameraData.renderer.cameraColorTarget;

                //将摄像机颜色复制给buffer1
                cmd.Blit(source, blurBuffer_id_1);

                bool needSwitch = true;
                
                for(int i = 0;i <= blurVolume.Iteration.value; i++)
                {
                    blurMaterial.SetVector(BlurRadius_id, new Vector4(i / blurVolume.DownSampler.value + blurVolume.BlurRadius.value, 0, 0, 0));

                    cmd.Blit(needSwitch ? blurBuffer_id_1 : blurBuffer_id_2, needSwitch ? blurBuffer_id_2 : blurBuffer_id_1, blurMaterial, 0);

                    needSwitch = !needSwitch;
                }

                cmd.Blit(needSwitch ? blurBuffer_id_1 : blurBuffer_id_2, source);

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                cmd.Release();
            }

            public void DualKawase_Blur(CommandBuffer cmd,ScriptableRenderContext context,ref RenderingData renderingData)
            {
                var source = renderingData.cameraData.renderer.cameraColorTarget;

                blurMaterial.SetVector(BlurRadius_id, new Vector4(blurVolume.BlurRadius.value, 0, 0, 0));

                //DownSample
                RenderTargetIdentifier lastDownTarget = source;
                for(int i = 0; i< blurVolume.Iteration.value; i++)
                {

                    int down_id = BlurLevels[i].down;

                    cmd.Blit(lastDownTarget, down_id, blurMaterial, 0);

                    lastDownTarget = down_id;
                }

                //UpSample
                int lastUpTarget = BlurLevels[blurVolume.Iteration.value - 1].down;
                for(int i = blurVolume.Iteration.value - 2; i >= 0; i--)
                {
                    int Up_id = BlurLevels[i].up;

                    cmd.Blit(lastUpTarget, Up_id, blurMaterial, 1);
                    lastUpTarget = Up_id;

                }

                cmd.Blit(lastUpTarget, source,blurMaterial,1);

                //clean up
                for(int i = 0;i< blurVolume.Iteration.value; i++)
                {
                    if (BlurLevels[i].down != lastUpTarget)
                        cmd.ReleaseTemporaryRT(BlurLevels[i].down);
                    if (BlurLevels[i].up != lastUpTarget)
                        cmd.ReleaseTemporaryRT(BlurLevels[i].up);

                }

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                cmd.Release();
            }

            public void Bokeh_TiltShift_LrisBlur(CommandBuffer cmd,ScriptableRenderContext context,ref RenderingData renderingData)
            {
                var source = renderingData.cameraData.renderer.cameraColorTarget;
                var descriptor = renderingData.cameraData.cameraTargetDescriptor;

                cmd.Blit(source, blurBuffer_id_1);

                blurMaterial.SetVector(GoldenRot_id, goldenRot);
                blurMaterial.SetVector(BlurRadius_id,new Vector4(blurVolume.Iteration.value,blurVolume.BlurRadius.value,1f/descriptor.width,1f/descriptor.height));

                cmd.Blit(blurBuffer_id_1,source,blurMaterial,0);

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                cmd.Release();
            }

            public void GrainyBlur(CommandBuffer cmd,ScriptableRenderContext context, ref RenderingData renderingData)
            {
                var source = renderingData.cameraData.renderer.cameraColorTarget;
                var descriptor = renderingData.cameraData.cameraTargetDescriptor;

                cmd.Blit(source, blurBuffer_id_1);

                blurMaterial.SetVector(BlurRadius_id, new Vector4(blurVolume.Iteration.value, blurVolume.BlurRadius.value / descriptor.height, 0, 0));

                cmd.Blit(blurBuffer_id_1 ,source,blurMaterial,0);

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                cmd.Release();
            }

            public void RadialBlur(CommandBuffer cmd,ScriptableRenderContext context, ref RenderingData renderingData)
            {

                var source = renderingData.cameraData.renderer.cameraColorTarget;

                cmd.Blit(source, blurBuffer_id_1);

                blurMaterial.SetVector(BlurRadius_id, new Vector4(blurVolume.Iteration.value, blurVolume.BlurRadius.value * 0.01f, 0.5f, 0.5f)) ;

                cmd.Blit(blurBuffer_id_1, source, blurMaterial, 0);

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                cmd.Release();
            }

        }


        public override void Create()
        {
            blurRenderPass = new BlurRenderPass(settings);

            blurRenderPass.renderPassEvent = settings.PassEvent;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(blurRenderPass);
        }
    }
}


