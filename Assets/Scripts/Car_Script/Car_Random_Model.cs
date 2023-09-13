using System.Collections.Generic;
using UnityEngine;

public class Car_Random_Model : MonoBehaviour
{
    [SerializeField] private List<GameObject> models;
    void Awake()
    {
        int random_index = Random.Range(0, models.Count);
        GameObject random_model = models[random_index];

        transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh = random_model.transform.GetChild(4).GetComponentInChildren<MeshFilter>().sharedMesh;
        transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials = random_model.transform.GetChild(4).GetComponentInChildren<MeshRenderer>().sharedMaterials;
        transform.GetChild(0).GetComponent<Outline>().enabled = true;
        Destroy(this);
    }
}