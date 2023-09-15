using System.Collections;
using System.Collections.Generic;
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
        }
        else
        {
            panelToActivate.SetActive(true);
            gameObject.SetActive(false);
        }
    }

}
