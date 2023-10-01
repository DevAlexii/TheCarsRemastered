using System;
using System.Collections.Generic;
using UnityEngine;

public class CarInfosRef : Singleton<CarInfosRef>
{
    [SerializeField] private List<CarDataInfo> AllListCarDataInfo;
    public Dictionary<CarType, List<CarInfo>> AllCarInfoData;

    [SerializeField] private List<CarDataInfo> DefaultListCarDataInfo;
    public Dictionary<CarType, List<CarInfo>> DefaultCarInfoData;

    private void Awake()
    {
        GenerateDictionary(out AllCarInfoData, AllListCarDataInfo);
        GenerateDictionary(out DefaultCarInfoData, DefaultListCarDataInfo);
    }
    private void GenerateDictionary(out Dictionary<CarType, List<CarInfo>> toGenerate, List<CarDataInfo> listRef)
    {
        toGenerate = new Dictionary<CarType, List<CarInfo>>();
        foreach (var data in listRef)
        {
            toGenerate[data.key] = new List<CarInfo>();
            toGenerate[data.key] = data.value;
        }
    }
}
[Serializable]
public struct CarDataInfo
{
    public CarType key;
    public List<CarInfo> value;
}
