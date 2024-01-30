using System;
using UnityEngine;


[Serializable]
public struct VehicleTypeData
{
    [field: SerializeField] public VehicleType CarType { get; private set; }
    [field: SerializeField] public string CarModel { get; private set; }
    [field: SerializeField] public int MaxMarketValue { get; private set; }
    [field: SerializeField] public int MinMarketValue { get; private set; }
    [field: SerializeField] public int MinSpeed { get; private set; }
    [field: SerializeField] public int MaxSpeed { get; private set; }
    [field: SerializeField] public int MinTorque { get; private set; }
    [field: SerializeField] public int MaxTorque { get; private set; }

    [field: SerializeField] public int CarPriceMultiplier { get; private set; }
}

[Serializable]
public enum VehicleType
{
    Sedan,
    Truck,
    Sport,
    Hatchback
}
