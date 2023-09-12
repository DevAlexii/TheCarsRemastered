using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] Button buttonForOption;

    void Start()
    {

    }

    void Update()
    {

    }

    public void ActivateMenu(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void DeactivateMenu(GameObject panel)
    {
        panel.SetActive(false);
    }
}
