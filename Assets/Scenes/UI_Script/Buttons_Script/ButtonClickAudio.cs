using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class ButtonClickAudio : ButtonClickParent
{
    [SerializeField] AudioMixer audioMixed_;

    bool clikedFirst = true;
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (clikedFirst)
        {
            audioMixed_.SetFloat("GeneralVolume", Mathf.Log10(0.0001f) * 20);
        }
        else
        {
            audioMixed_.SetFloat("GeneralVolume", Mathf.Log10(1f) * 20);
        }
        base.OnPointerClick(eventData);
        clikedFirst = !clikedFirst;
    }
}
