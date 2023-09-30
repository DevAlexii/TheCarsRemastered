using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioCallBack : Singleton<AudioCallBack>
{
    [Header("Clip and Types")]
    [SerializeField] public List<AudioClip> audioClips;
    [SerializeField] public List<AudioType> audioTypes;

    [SerializeField] private AudioSource audioSource;
    private Dictionary<AudioType, AudioClip> audioClipDictionary = new Dictionary<AudioType, AudioClip>();


    [SerializeField] Slider sliderAudio;
    [SerializeField] AudioMixer audioMixer;
    private void Start()
    {
        if (audioClips.Count != audioTypes.Count)
        {
            return;
        }
        for (int i = 0; i < audioClips.Count; i++)
        {
            audioClipDictionary[audioTypes[i]] = audioClips[i];
        }
        sliderAudio.onValueChanged.AddListener(SetVolumeBySlider);

    }
    public void PlayAudio(AudioType audioType, float pitch)
    {
        if (audioClipDictionary.ContainsKey(audioType))
        {
            AudioClip clipToPlay = audioClipDictionary[audioType];
            audioSource.pitch = pitch;
            audioSource.clip = clipToPlay;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioType not found " + audioType);
        }
    }
    void SetVolumeBySlider(float value)
    {
        audioMixer.SetFloat("GeneralVolume", Mathf.Log10(value) * 20f);
    }

    //Se vogliamo mettere audio ambientali crows ecc usiamo questo
    private float elapsedTime = 0.0f;
    public void AmbientVolume(float startVolume,float targetVolume,float duration)
    {
        float newVolume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);

        audioSource.volume = newVolume;
    }
}
