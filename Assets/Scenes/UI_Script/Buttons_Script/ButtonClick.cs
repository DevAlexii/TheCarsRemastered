using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class ButtonClick : ButtonClickParent
{
    [SerializeField] Animator animator_;
    [SerializeField] string menuToShow;

    [SerializeField] AudioMixer audioMixed_;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        animator_.SetTrigger(menuToShow);
    }
}
