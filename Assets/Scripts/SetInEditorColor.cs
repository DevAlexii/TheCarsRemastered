using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[ExecuteInEditMode]
public class SetInEditorColor : MonoBehaviour
{
    [Header("Pedestrian")]
    [SerializeField] List<PoolColors> colors;
    public Dictionary<bodyPart, List<Shader_Color>> bodies_colors { get; private set; }
    private void Start()
    {
        bodies_colors = new Dictionary<bodyPart, List<Shader_Color>>();
        for (int i = 0; i < colors.Count; i++)
        {
            bodies_colors[colors[i].key] = colors[i].values;
        }
    }

    void Update()
    {
        List<Material> materials = GetComponent<MeshRenderer>().sharedMaterials.ToList();
        int random_index = 0;

        for (int i = 0; i < materials.Count; i++)
        {
            switch (materials[i].name)
            {
                case "maglietta":
                    random_index = Random.Range(0, bodies_colors[bodyPart.shirt].Count);
                    materials[i].SetColor("_top_color", bodies_colors[bodyPart.shirt][random_index].top_color);
                    materials[i].SetColor("_bottom_color", bodies_colors[bodyPart.shirt][random_index].bottom_color);
                    break;
                case "capelli":
                    random_index = Random.Range(0, bodies_colors[bodyPart.hair].Count);
                    materials[i].SetColor("_top_color", bodies_colors[bodyPart.hair][random_index].top_color);
                    materials[i].SetColor("_bottom_color", bodies_colors[bodyPart.hair][random_index].bottom_color);
                    break;
                case "pelle":
                    random_index = Random.Range(0, bodies_colors[bodyPart.skin].Count);
                    materials[i].SetColor("_top_color", bodies_colors[bodyPart.skin][random_index].top_color);
                    materials[i].SetColor("_bottom_color", bodies_colors[bodyPart.skin][random_index].bottom_color);
                    break;
            }
        }
        GetComponent<MeshRenderer>().sharedMaterials = materials.ToArray();
    }
}
