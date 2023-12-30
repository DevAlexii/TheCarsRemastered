using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Scocca_Color : MonoBehaviour
{
    void Start()
    {
        GameObject scocca = GetComponent<GameObject>();
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
