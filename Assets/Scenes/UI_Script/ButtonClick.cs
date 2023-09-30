using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;

public class ButtonClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject panelToActivate;
    [SerializeField] bool deactivateParent;

    [SerializeField] Animator animator_;
    [SerializeField] string menuToShow;
    [SerializeField] string menuToHide;
    [SerializeField] bool noNeedToHideMenu;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (noNeedToHideMenu)
        {
            animator_.SetTrigger(menuToHide);
        }
        animator_.SetTrigger(menuToShow);

        float randomPitch = Random.Range(0.7f, 1.1f);
        AudioCallBack.self.PlayAudio(AudioType.Button, randomPitch);
    }

}
