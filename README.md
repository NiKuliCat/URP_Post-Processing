# URP_Post-Processing
Extend the Unity URP post-processing system

---
�˲ֿ����һЩ����ѧϰUnity URP ��Ļ����Ĺ�������ʵ�ֵ�Ч��<br>
��Ȼ��һЩ����Ч�����㷨�����[ǳī-X-PostProcessing-Library](https://github.com/QianMo/X-PostProcessing-Library),ʮ�ָ�л!<br>

### Ŀǰʵ�ֵĺ���Ч������ :
+ Blur
	+ [Gaussian Blur](https://github.com/NiKuliCat/URP_Post-Processing/blob/4f4023e698ac9cad10eae3f51ae933cedebdb908/Assets/Shader/Post-process/Blur/GaussianBlur.shader)
	+ [Box Blur](https://github.com/NiKuliCat/URP_Post-Processing/blob/0f0d46f4e9809324e54103c5fbf10644ad4ba695/Assets/Shader/Post-process/Blur/BoxBlur.shader)
	+ [Tent Blur](https://github.com/NiKuliCat/URP_Post-Processing/blob/0f0d46f4e9809324e54103c5fbf10644ad4ba695/Assets/Shader/Post-process/Blur/TentBlur.shader)
	+ [Grainy Blur](https://github.com/NiKuliCat/URP_Post-Processing/blob/0f0d46f4e9809324e54103c5fbf10644ad4ba695/Assets/Shader/Post-process/Blur/GrainyBlur.shader)
	+ [Kawase Blur](https://github.com/NiKuliCat/URP_Post-Processing/blob/0f0d46f4e9809324e54103c5fbf10644ad4ba695/Assets/Shader/Post-process/Blur/KawaseBlur.shader)
	+ [DualKawase Blur](https://github.com/NiKuliCat/URP_Post-Processing/blob/0f0d46f4e9809324e54103c5fbf10644ad4ba695/Assets/Shader/Post-process/Blur/DualKawaseBlur.shader)
	+ [Bokeh Blur](https://github.com/NiKuliCat/URP_Post-Processing/blob/0f0d46f4e9809324e54103c5fbf10644ad4ba695/Assets/Shader/Post-process/Blur/BokehBlur.shader)
	+ [Lris Blur](https://github.com/NiKuliCat/URP_Post-Processing/blob/0f0d46f4e9809324e54103c5fbf10644ad4ba695/Assets/Shader/Post-process/Blur/LrisBlur.shader)
	+ [Radial Blur](https://github.com/NiKuliCat/URP_Post-Processing/blob/0f0d46f4e9809324e54103c5fbf10644ad4ba695/Assets/Shader/Post-process/Blur/RadialBlur.shader)
	+ [TiltShift Blur](https://github.com/NiKuliCat/URP_Post-Processing/blob/0f0d46f4e9809324e54103c5fbf10644ad4ba695/Assets/Shader/Post-process/Blur/TiltShiftBlur.shader)
	+ [RenderFeature](https://github.com/NiKuliCat/URP_Post-Processing/blob/0f0d46f4e9809324e54103c5fbf10644ad4ba695/Assets/Scripts/RenderFeature/BlurRenderFeature.cs) and [VolumeComponent](https://github.com/NiKuliCat/URP_Post-Processing/blob/0f0d46f4e9809324e54103c5fbf10644ad4ba695/Assets/Scripts/Post-processVolume/Blur.cs)

+ Glitch
	+ [RGB Split Glitch](https://github.com/NiKuliCat/URP_Post-Processing/blob/d7b71c13a9ff2a4bf2234b7821c157da26226c21/Assets/Shader/Post-process/Glitch/RGBSplitGlitch.shader)
	+ [Image Block Glitch](https://github.com/NiKuliCat/URP_Post-Processing/blob/d7b71c13a9ff2a4bf2234b7821c157da26226c21/Assets/Shader/Post-process/Glitch/ImageBlockGlitch.shader)
	+ [Line Block Glitch](https://github.com/NiKuliCat/URP_Post-Processing/blob/d7b71c13a9ff2a4bf2234b7821c157da26226c21/Assets/Shader/Post-process/Glitch/LineBlockGlitch.shader)
	+ [Scan Line Jitter Glitch](https://github.com/NiKuliCat/URP_Post-Processing/blob/d7b71c13a9ff2a4bf2234b7821c157da26226c21/Assets/Shader/Post-process/Glitch/ScanLineJitterGlitch.shader)
	+ [RenderFeature](https://github.com/NiKuliCat/URP_Post-Processing/tree/main/Assets/Scripts/Post-processVolume/Glitch) and [VolumeComponent](https://github.com/NiKuliCat/URP_Post-Processing/tree/main/Assets/Scripts/Post-processVolume/Glitch)

+ Flash
	+ [Black_White Flash](https://github.com/NiKuliCat/URP_Post-Processing/blob/d7b71c13a9ff2a4bf2234b7821c157da26226c21/Assets/Shader/Post-process/Black_WhiteFlash/BlackwhiteFlash.shader)
	+ [RenderFeature](https://github.com/NiKuliCat/URP_Post-Processing/blob/d7b71c13a9ff2a4bf2234b7821c157da26226c21/Assets/Scripts/RenderFeature/Black_WhiteFlash/BlackWhiteFlash.cs) and [VolumeComponent](https://github.com/NiKuliCat/URP_Post-Processing/blob/d7b71c13a9ff2a4bf2234b7821c157da26226c21/Assets/Scripts/Post-processVolume/Black_WhiteFlash/Black_WhiteFlashVolume.cs)


---
![](https://github.com/NiKuliCat/Accessory_Blog/blob/fc5ecacfe669ab9c50be62db1417b730e816207e/URP_Post-Processing/GriaryBlur.png)
<video src = "https://github.com/NiKuliCat/Accessory_Blog/blob/fc5ecacfe669ab9c50be62db1417b730e816207e/URP_Post-Processing/ImageBlockGlitch.mp4"> </video>
<video src = "https://github.com/NiKuliCat/Accessory_Blog/blob/fc5ecacfe669ab9c50be62db1417b730e816207e/URP_Post-Processing/Black_WhiteFlash.mp4"> </cideo>

<iframe 
src="https://github.com/NiKuliCat/Accessory_Blog/blob/fc5ecacfe669ab9c50be62db1417b730e816207e/URP_Post-Processing/ImageBlockGlitch.mp4" 
scrolling="no" 
border="0" 
frameborder="no" 
framespacing="0" 
allowfullscreen="true" 
height=600 
width=800> 
</iframe>

