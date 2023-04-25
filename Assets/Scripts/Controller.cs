using System.Collections;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.Rendering;

public class Controller : MonoBehaviour
{
    private Volume volumeStack;

    private Black_WhiteFlashVolume flashVolume;

    private ImageBlockGlitch_DoubleVolume imageBlockGlitch_DoubleVolume;

    private BlurVolume blurVolume;

    private ScanLineJitterGlitchVolume scanLineJitterGlitchVolume;

    private RGBSplitGlitchVolume RGB_SplitGlitchVolume;

    [GradientUsage(true)]
    public Gradient gradient;

    private void OnEnable()
    {
        volumeStack = GetComponent<Volume>();

        volumeStack.profile.TryGet<Black_WhiteFlashVolume>(out flashVolume);
        volumeStack.profile.TryGet<ImageBlockGlitch_DoubleVolume>(out imageBlockGlitch_DoubleVolume);
        volumeStack.profile.TryGet<BlurVolume>(out blurVolume);
        volumeStack.profile.TryGet<ScanLineJitterGlitchVolume>(out scanLineJitterGlitchVolume);
        volumeStack.profile.TryGet<RGBSplitGlitchVolume>(out RGB_SplitGlitchVolume);

    }

   

    IEnumerator Evolve()
    {


        yield return new WaitForSeconds(2f);


        while (flashVolume.Intensity.value < 1.0f)
        {
            flashVolume.Intensity.value += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        while (flashVolume.NoiseIntensity.value < 0.45f)
        {
            flashVolume.NoiseIntensity.value += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        blurVolume.enable.value = true;

        yield return new WaitForSeconds(5f);


        RGB_SplitGlitchVolume.Enable.value = true;


        yield return new WaitForSeconds(5f);


        scanLineJitterGlitchVolume.Enable.value = true;

        yield return new WaitForSeconds(5f);

        imageBlockGlitch_DoubleVolume.Enable.value = true;

        StartCoroutine(Reverse());
    }


    IEnumerator Reverse()
    {
        yield return new WaitForSeconds(5f);

        imageBlockGlitch_DoubleVolume.Enable.value = false;

        yield return new WaitForSeconds(1f);

        scanLineJitterGlitchVolume.Enable.value = false;

        yield return new WaitForSeconds(1f);


        RGB_SplitGlitchVolume.Enable.value = false;

        yield return new WaitForSeconds(1f);

        blurVolume.enable.value = false;

        yield return new WaitForSeconds(1f);

        while (flashVolume.NoiseIntensity.value > 0.0f)
        {
            flashVolume.NoiseIntensity.value -= Time.deltaTime;
            yield return null;
        }


       // yield return new WaitForSeconds(2f);


        while (flashVolume.Intensity.value > 0.0f)
        {
            flashVolume.Intensity.value -= Time.deltaTime;
            yield return null;
        }



        StartCoroutine(Evolve());

    }
    private void Start()
    {
        //StartCoroutine(Evolve());
        StartCoroutine(UpdateFlashColor());
    }

    IEnumerator UpdateFlashColor()
    {
        float time = 0f;
        while (true)
        {
            while(time <1.0f)
            {
                time += Time.deltaTime * 0.1f;
                flashVolume.WhiteTintColor.value = gradient.Evaluate(time);
                yield return null;
            }
            time = 0f;
        }

    }
}
