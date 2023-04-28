using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    private AudioMixer mixer;
    private Slider slider;
    private string volumeName;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    /// <summary>
    /// Setup the audio mixer controller, where we want to change volume
    /// </summary>
    /// <param name="mixer">Audio Mixer with the mixer controllers</param>
    /// <param name="volumeName">Name of the controller on audio mixer (master, music, SFX)</param>
    public void Setup(AudioMixer mixer, string volumeName)
    {
        this.mixer = mixer;
        this.volumeName = volumeName;

        if (mixer.GetFloat(volumeName, out float decibel))
        {
            slider.value = ToLinear(decibel);
        }
    }

    /// <summary>
    /// Save changed volume to the controller
    /// </summary>
    public void Save()
    {
        mixer.SetFloat(volumeName, ToDB(slider.value));
    }

    /// <summary>
    /// We need to perform calculations between linear floats and logarithmic scale of dB.
    /// </summary>
    /// <param name="linear"></param>
    /// <returns>The decibel value of correspondent linear value</returns>
    private float ToDB(float linear)
    {
        return linear <= 0 ? -80f : 20f * Mathf.Log10(linear);
    }

    /// <summary>
    /// Opposite to ToDB, change logarithmic to linear value.
    /// </summary>
    /// <param name="decibel">The decibel value of volume</param>
    /// <returns>Linear value of volume</returns>
    private float ToLinear(float decibel)
    {
        return Mathf.Clamp01(Mathf.Pow(10f, decibel / 20f));
    }
}
