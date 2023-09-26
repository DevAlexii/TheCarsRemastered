using System.Collections.Generic;
using UnityEngine;

public class Car_Random_Model : MonoBehaviour
{
    [SerializeField] private bool enable_only_random_color;
    [SerializeField] private List<GameObject> models;
    [SerializeField] private GameObject scocca;
    void Awake()
    {
        if (enable_only_random_color)
        {
            RandomColor();
            Destroy(this);
            return;
        }
        else
        {
            RandomModel();
            RandomColor();
            Destroy(this);
        }
    }

    private void RandomModel()
    {
        int random_index = Random.Range(0, models.Count);
        GameObject random_model = models[random_index];

        transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh = random_model.transform.GetChild(4).GetComponentInChildren<MeshFilter>().sharedMesh;
        transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials = random_model.transform.GetChild(4).GetComponentInChildren<MeshRenderer>().sharedMaterials;
        transform.GetChild(0).GetComponent<Outline>().enabled = true;
    }

    void RandomColor()
    {
        Material[] materials = scocca.GetComponent<MeshRenderer>().materials;
        Shader_Color color = Color_Manager.self.GetRandomShaderColor;

        foreach (var material in materials)
        {
            if (material.name.StartsWith("shader"))
            {
                material.SetColor("_top_color", color.top_color);
                material.SetColor("_bottom_color", color.bottom_color);
            }
        }
        scocca.GetComponent<MeshRenderer>().materials = materials;
    }
}