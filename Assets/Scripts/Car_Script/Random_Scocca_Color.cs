using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Scocca_Color : MonoBehaviour
{
    void Start()
    {
        Material[] materials = this.gameObject.GetComponent<MeshRenderer>().materials;
        Shader_Color color = Color_Manager.self.GetRandomShaderColor;

        foreach (var material in materials)
        {
            if (material.name.StartsWith("shader"))
            {
                material.SetColor("_top_color", color.top_color);
                material.SetColor("_bottom_color", color.bottom_color);
            }
        }
        this.gameObject.GetComponent<MeshRenderer>().materials = materials;
    }
}
