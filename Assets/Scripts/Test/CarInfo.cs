using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CarInfo/CreateNewCarInfo", order = 1)]
public class CarInfo : ScriptableObject
{
    public List<Mesh> meshes;
    public float MaxSpeed;
    public float MaxBreakForce;
    public float MaxRotationSpeed;
    public float QuequeRange;
}
