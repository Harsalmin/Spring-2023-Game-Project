using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsWindow : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;

    [SerializeField]
    private AudioControl masterControl;
    [SerializeField]
    private AudioControl musicControl;
    [SerializeField]
    private AudioControl sfxControl;

    [SerializeField]
    private string sfxName;
    [SerializeField]
    private string musicName;
    [SerializeField]
    private string masterName;

    /// <summary>
    /// At start, setup the audio controls.
    /// </summary>
    private void Start()
    {
        masterControl.Setup(mixer, masterName);
        musicControl.Setup(mixer, musicName);
        sfxControl.Setup(mixer, sfxName);
    }

    /// <summary>
    /// If we put save on FixedUpdate, the volume change will register in real time.
    /// </summary>
    public void FixedUpdate()
    {
        masterControl.Save();
        musicControl.Save();
        sfxControl.Save();
    }

    /// <summary>
    /// Make sure the volume cahnges are registered.
    /// </summary>
    public void CloseOptions()
    {
        masterControl.Save();
        musicControl.Save();
        sfxControl.Save();
    }
}