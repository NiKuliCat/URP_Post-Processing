# URP_Post-Processing
Extend the Unity URP post-processing system

---
此仓库包含一些本人学习Unity URP 屏幕后处理的过程中所实现的效果<br>
当然，一些后处理效果的算法借鉴于[浅墨-X-PostProcessing-Library](https://github.com/QianMo/X-PostProcessing-Library),十分感谢!<br>

### 包括 :
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
![](https://github.com/NiKuliCat/Accessory_Blog/blob/9d184943998e33a40e3a558b1f93fac69c30c4ad/URP_Post-Processing/ImageBlockGlitch.png)
![](https://github.com/NiKuliCat/Accessory_Blog/blob/9d184943998e33a40e3a558b1f93fac69c30c4ad/URP_Post-Processing/flash.png)
![](https://github.com/NiKuliCat/Accessory_Blog/blob/66cd001f45606e0c2d73a4371ae18ac944a31565/URP_Post-Processing/edit.png)


