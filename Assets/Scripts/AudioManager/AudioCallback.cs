using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioCallBack : Singleton<AudioCallBack>
{
    [Header("Clip and Types")]
    [SerializeField] public List<AudioClip> audioClips;
    [SerializeField] public List<AudioType> audioTypes;

    [SerializeField] private AudioSource SFX_audioSource;
    [SerializeField] private AudioSource Music_audioSource;
    private Dictionary<AudioType, AudioClip> audioClipDictionary = new Dictionary<AudioType, AudioClip>();

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
    }
    public void PlayAudio(AudioType audioType, float pitch)
    {
        if (audioClipDictionary.ContainsKey(audioType))
        {
            AudioClip clipToPlay = audioClipDictionary[audioType];
            SFX_audioSource.pitch = pitch;
            SFX_audioSource.clip = clipToPlay;
            SFX_audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioType not found " + audioType);
        }
    }
    public void PlayMusic(AudioType audioType, float pitch)
    {
        if (audioClipDictionary.ContainsKey(audioType))
        {
            AudioClip clipToPlay = audioClipDictionary[audioType];
            Music_audioSource.pitch = pitch;
            Music_audioSource.clip = clipToPlay;
            Music_audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioType not found " + audioType);
        }
    }



    //Se vogliamo mettere audio ambientali crows ecc usiamo questo
    private float elapsedTime = 0.0f;
    public void AmbientVolume(float startVolume,float targetVolume,float duration)
    {
        float newVolume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);

        Music_audioSource.volume = newVolume;
    }
}
