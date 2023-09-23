using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCallback : Singleton<AudioCallback>
{
    [SerializeField] AudioSource source;

    public void PlayAudioSource(float pitch)
    {
        source.pitch = pitch;
        source.Play();
    }
}
