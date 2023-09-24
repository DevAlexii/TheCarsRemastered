using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal.Builders;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioCallback : Singleton<AudioCallback>
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] Slider musicSlider;

    public void PlayAudioSource(float pitch)
    {
        source.pitch = pitch;
        source.Play();

        musicSlider.onValueChanged.AddListener(SetVolumeBySlider);
    }

    void SetVolumeBySlider(float value)
    {
        audioMixer.SetFloat("GeneralVolume", Mathf.Log10(value) * 20f);
    }
}
