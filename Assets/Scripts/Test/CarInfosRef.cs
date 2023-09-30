using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CarInfosRef : Singleton<CarInfosRef>
{
    public enum CarType { BaseCar, Tir, Van }
    [Serializable]
    public struct CarDataInfo
    {
        public CarType key;
        public List<CarInfo> value;
    }
    [SerializeField] private List<CarDataInfo> ListCarDataInfo;
    public Dictionary<CarType, List<CarInfo>> CarInfoData { get; private set; }

    private void Start()
    {
        CarInfoData = new Dictionary<CarType, List<CarInfo>>();
        foreach (var data in ListCarDataInfo)
        {
            CarInfoData[data.key] = new List<CarInfo>();
            CarInfoData[data.key] = data.value;
        }
    }
}
