using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CarInfo/CreateNewCarInfo", order = 1)]
public class CarInfo : ScriptableObject
{
    public GameObject BasePrefab;
    public List<GameObject> CarRef;
    public float MaxSpeed;
    public float MaxBreakForce;
    public float MaxRotationSpeed;
    public float QuequeRange;
}
