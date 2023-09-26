using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pedestrian_Color : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> arms;

    private void Start()
    {
        Dictionary<bodyPart, List<Shader_Color>> pool_colors = Color_Manager.self.bodies_colors;
        List<Material> materials = GetComponent<MeshRenderer>().sharedMaterials.ToList();
        int random_index = 0;

        for (int i = 0; i < materials.Count; i++)
        {
            switch (materials[i].name)
            {
                case "maglietta":
                    random_index = Random.Range(0, pool_colors[bodyPart.shirt].Count);
                    materials[i].SetColor("_top_color", pool_colors[bodyPart.hair][random_index].top_color);
                    materials[i].SetColor("_bottom_color", pool_colors[bodyPart.hair][random_index].bottom_color);
                    List<Material> arm = arms[0].sharedMaterials.ToList();
                    arm[1].SetColor("_top_color", pool_colors[bodyPart.hair][random_index].top_color);
                    arm[1].SetColor("_bottom_color", pool_colors[bodyPart.hair][random_index].bottom_color);
                    arms[0].sharedMaterials = arm.ToArray();
                    arms[1].sharedMaterials = arm.ToArray();
                    break;
                case "capelli":
                    random_index = Random.Range(0, pool_colors[bodyPart.hair].Count);
                    materials[i].SetColor("_top_color", pool_colors[bodyPart.hair][random_index].top_color);
                    materials[i].SetColor("_bottom_color", pool_colors[bodyPart.hair][random_index].bottom_color);
                    break;
                case "pelle":
                    random_index = Random.Range(0, pool_colors[bodyPart.skin].Count);
                    materials[i].SetColor("_top_color", pool_colors[bodyPart.hair][random_index].top_color);
                    materials[i].SetColor("_bottom_color", pool_colors[bodyPart.hair][random_index].bottom_color);
                    break;
            }
        }
        GetComponent<MeshRenderer>().sharedMaterials = materials.ToArray();
        Destroy(this);
    }
}