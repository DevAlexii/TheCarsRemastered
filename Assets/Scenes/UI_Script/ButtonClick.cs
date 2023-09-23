using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;

public class ButtonClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject panelToActivate;
    [SerializeField] bool deactivateParent;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (deactivateParent)
        {
            transform.parent.gameObject.SetActive(false);
            panelToActivate.SetActive(true);

            float randomPitch = Random.Range(0.7f, 1.1f);
            AudioCallback.self.PlayAudioSource(randomPitch);
        }
        else
        {
            panelToActivate.SetActive(true);
            gameObject.SetActive(false);
            float randomPitch = Random.Range(0.7f, 1.1f);
            AudioCallback.self.PlayAudioSource(randomPitch);
        }
    }

}
