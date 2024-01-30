using System;
using UnityEngine;

[Serializable]
public struct VehiclePartData
{
    public CarParts carParts;
    [field: SerializeField] public int MaxNumberPartsAvailable { get; private set; }
    [field: SerializeField] public float PaintedPartMultiplier { get; private set; }
    [field: SerializeField] public float DamagedPartMultiplier { get; private set; }

    [HideInInspector] public bool IsPainted;
    [HideInInspector] public bool IsDamaged;
}


[Serializable]
public enum CarParts
{
    Tavan,
    Kaput,
    Bagaj,
    Kapilar,
    YanBolge
}
