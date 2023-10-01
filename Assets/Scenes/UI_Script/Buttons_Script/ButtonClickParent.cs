using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClickParent : MonoBehaviour , IPointerClickHandler
{
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        float randomPitch = Random.Range(0.7f, 1.1f);
        AudioCallBack.self.PlayAudio(AudioType.Button, randomPitch);
    }
}
