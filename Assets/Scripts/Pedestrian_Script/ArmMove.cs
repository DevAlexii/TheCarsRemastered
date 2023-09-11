using System.Collections.Generic;
using UnityEngine;

public class ArmMove : MonoBehaviour
{
    [SerializeField] List<Mesh> meshFilterPool;
    MeshFilter mF;
    [SerializeField] List<Material> matrials;
    void Start()
    {
        mF = transform.GetChild(0).GetComponent<MeshFilter>();

        int randomMesh = Random.Range(0, meshFilterPool.Count);
        int randomMat = Random.Range(0, matrials.Count);

        mF.sharedMesh = meshFilterPool[randomMesh];

    }

    void Update()
    {

    }
}
