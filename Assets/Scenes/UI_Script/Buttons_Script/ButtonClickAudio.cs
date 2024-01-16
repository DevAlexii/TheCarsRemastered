using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClickAudio : ButtonClickParent
{
    [SerializeField] AudioMixer audioMixed_;

    bool clikedFirst = true;

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (clikedFirst)
        {
            audioMixed_.SetFloat("GeneralVolume", Mathf.Log10(0.0001f) * 20);
            transform.GetComponent<Image>().sprite = newImage;
        }
        else
        {
            audioMixed_.SetFloat("GeneralVolume", Mathf.Log10(1f) * 20);
            transform.GetComponent<Image>().sprite = baseImage;
        }
        base.OnPointerClick(eventData);
        clikedFirst = !clikedFirst;
    }
}
