using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pedestrian_Color : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> arms;

    private void Start()
    {
        Dictionary<bodyPart, List<Shader_Color>> pool_colors = Color_Manager.self.bodies_colors;
        Material[] materials = GetComponent<MeshRenderer>().materials;
        int random_index = 0;
        Shader_Color color = new Shader_Color();

        foreach (var material in materials)
        {
            if (material.name.StartsWith("maglietta"))
            {
                random_index = Random.Range(0, pool_colors[bodyPart.shirt].Count);
                color = pool_colors[bodyPart.shirt][random_index];
                material.SetColor("_top_color", color.top_color);
                material.SetColor("_bottom_color", color.bottom_color);
                Material[] arm = arms[0].materials;
                arm[1].SetColor("_top_color", color.top_color);
                arm[1].SetColor("_bottom_color", color.bottom_color);
                arms[0].sharedMaterials = arm;
                arms[1].sharedMaterials = arm;
            }
            if (material.name.StartsWith("capelli"))
            {
                random_index = Random.Range(0, pool_colors[bodyPart.hair].Count);
                color = pool_colors[bodyPart.hair][random_index];
                material.SetColor("_top_color", color.top_color);
                material.SetColor("_bottom_color", color.bottom_color);
            }
            if (material.name.StartsWith("pelle"))
            {
                random_index = Random.Range(0, pool_colors[bodyPart.skin].Count);
                color = pool_colors[bodyPart.skin][random_index];
                material.SetColor("_top_color", color.top_color);
                material.SetColor("_bottom_color", color.bottom_color);
                Material[] arm = arms[0].materials;
                arm[0].SetColor("_top_color", color.top_color);
                arm[0].SetColor("_bottom_color", color.bottom_color);
                arms[0].sharedMaterials = arm;
                arms[1].sharedMaterials = arm;
            }
        }
        GetComponent<MeshRenderer>().sharedMaterials = materials;
        Destroy(this);
    }
}
