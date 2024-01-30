using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "VehiclePart", menuName = "Vehicle System/Vehicle Part", order = 1)]
public class VehiclePart_SO : ScriptableObject
{
    public List<VehiclePartData> Parts;
}
