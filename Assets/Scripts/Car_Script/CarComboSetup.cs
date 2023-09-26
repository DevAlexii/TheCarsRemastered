using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarComboSetup : MonoBehaviour
{
    [SerializeField] private List<GameObject> cars = new List<GameObject>();
    
    public void ActivateCars(int index)
    {
        for (int i = 0; i < index; i++)
        {
            cars[i].SetActive(true);
        }
        Destroy(this);
    }
}
