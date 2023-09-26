using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pedestrian_Color : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> arms;

    private void Start()
    {
        Dictionary<bodyPart, List<Material>> pool_colors = Color_Manager.self.bodies_colors;
        List<Material> materials = GetComponent<MeshRenderer>().sharedMaterials.ToList();
        int random_index = 0;

        for (int i = 0; i < materials.Count; i++)
        {
            switch (materials[i].name)
            {
                case "maglietta":
                    random_index = Random.Range(0, pool_colors[bodyPart.shirt].Count);
                    materials[i] = pool_colors[bodyPart.shirt][random_index];
                    List<Material> arm = arms[0].sharedMaterials.ToList();
                    arm[1] = pool_colors[bodyPart.shirt][random_index];
                    arms[0].sharedMaterials = arm.ToArray();
                    arms[1].sharedMaterials = arm.ToArray();
                    break;
                case "capelli":
                    random_index = Random.Range(0, pool_colors[bodyPart.hair].Count);
                    materials[i] = pool_colors[bodyPart.hair][random_index];
                    break;
                case "pelle":
                    random_index = Random.Range(0, pool_colors[bodyPart.skin].Count);
                    materials[i] = pool_colors[bodyPart.skin][random_index];
                    break;
            }
        }
        GetComponent<MeshRenderer>().sharedMaterials = materials.ToArray();
        Destroy(this);
    }
}