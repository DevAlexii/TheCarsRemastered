using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian_Color : MonoBehaviour
{
    [SerializeField] List<Material> mat;
    private void Awake()
    {
        ChangeColor();
    }

    void ChangeColor()
    {
        Material[] materials = GetComponent<MeshRenderer>().materials;
        foreach (var material in materials)
        {
            int col = Random.Range(0, mat.Count);
            material.shader = mat[col].shader;
            GetComponent<MeshRenderer>().material = material;
        }
    }
}
