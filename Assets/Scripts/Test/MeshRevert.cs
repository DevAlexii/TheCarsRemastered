
using UnityEngine;

public class MeshRevert : MonoBehaviour
{
    void Start()
    {
        Destroy(GetComponent<MeshFilter>());
        Destroy(GetComponent<MeshFilter>());
    }
}
