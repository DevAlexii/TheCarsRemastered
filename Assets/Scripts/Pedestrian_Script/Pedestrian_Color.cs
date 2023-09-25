using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian_Color : MonoBehaviour
{
    [SerializeField] List<Material> hairMaterial;
    [SerializeField] List<Material> shirtMaterial;

    [SerializeField] List<MeshRenderer> meshBraccia;
    private void Awake()
    {
        ChangeColor();
    }

    void ChangeColor()
    {
        Material[] materials = GetComponent<MeshRenderer>().materials;
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].name.StartsWith("capelli"))
            {
                int col = Random.Range(0, hairMaterial.Count);
                materials[i] = hairMaterial[col];
            }
            if (materials[i].name.StartsWith("maglietta"))
            {
                int col = Random.Range(0, shirtMaterial.Count);
                materials[i].shader = shirtMaterial[col].shader;
                for (int j = 0; j < meshBraccia.Count; j++)
                {
                    meshBraccia[j].materials[1] = shirtMaterial[col];
                }
            }
        }
        GetComponent<MeshRenderer>().sharedMaterials = materials;

    }
}
