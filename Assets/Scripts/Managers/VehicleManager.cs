using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    [Header("Variables")]
    private int _totalNumberPartsInTheVehicle;
    private int _marketValue;
    private float _saleValue;

    private int _vehicleSpeed;
    private int _vehicleTorque;
    private float _speedMultiplier;
    private float _torqueMultiplier;

    private int maxDoor = 4;
    private int maxSide = 4;



    [Header("References")]
    [SerializeField] private VehicleType_SO _vehicleTypeData;
    private VehicleMultipliers_SO _vehicleMultipliersData;
    private VehiclePart_SO _vehiclePartData;
    private RCC_CarControllerV3 _carController;

    [SerializeField] private List<VehiclePartData> _vehiclePartTypeMultiplierList;
    private List<VehiclePartData> _vehicleDamagedCarPartsCloneList = new List<VehiclePartData>();
    private List<VehiclePartData> _damagedCarPartsGetList = new List<VehiclePartData>();

    private int _carPriceMultiplier;

    private void Awake()
    {
        _vehicleMultipliersData = GetCarData();
        _vehiclePartData = GetPartData();
        _vehiclePartTypeMultiplierList = _vehiclePartData.Parts.ToList();

        _carController = GetComponent<RCC_CarControllerV3>();
        SendDataToManagers();
    }

    private void Start()
    {
        Init();
    }


    private VehicleMultipliers_SO GetCarData()
    {
        return Resources.Load<VehicleMultipliers_SO>(path:"Vehicle/VehicleMultipliers/VehicleMultipliers_SO");
    }

    private VehiclePart_SO GetPartData()
    {
        return Resources.Load<VehiclePart_SO>(path: "Vehicle/VehiclePart/VehiclePart_SO");
    }

    private void SendDataToManagers()
    {
        _totalNumberPartsInTheVehicle = Random.Range(1, _vehicleMultipliersData.VehicleMultipliersData.TotalNumberPartsInTheVehicle);
        _marketValue = (int)Random.Range(_vehicleTypeData.VehicleTypeData.MinMarketValue, _vehicleTypeData.VehicleTypeData.MaxMarketValue);
        _carPriceMultiplier = _vehicleTypeData.VehicleTypeData.CarPriceMultiplier;

        int maxSpeed = _vehicleTypeData.VehicleTypeData.MaxSpeed;
        int minSpeed = _vehicleTypeData.VehicleTypeData.MinSpeed;
        int minTorque = _vehicleTypeData.VehicleTypeData.MinTorque;
        int maxTorque = _vehicleTypeData.VehicleTypeData.MaxTorque;
        _vehicleTorque = Random.Range(minTorque, maxTorque);
        _vehicleSpeed = Random.Range(minSpeed, maxSpeed);

        SetCarControllerValue();
        CalculateSpeedMultiplier(minSpeed, maxSpeed, _vehicleSpeed);
        CalculateTorqueMultiplier(minTorque, maxTorque, _vehicleTorque);

    }

    private void SetCarControllerValue()
    {
        _carController.maxspeed = _vehicleSpeed;
        _carController.maxEngineTorque = _vehicleTorque;
    }

    private void CalculateSpeedMultiplier(int minSpeed, int maxSpeed, int currentSpeed)
    {
        float midPoint = (minSpeed + maxSpeed) / 2f;
        float distanceToMidPoint = Mathf.Abs(currentSpeed - midPoint);
        float ratio = distanceToMidPoint / (midPoint - minSpeed);
        float speedMultiplier = Mathf.Lerp(_vehicleMultipliersData.VehicleMultipliersData.MinSpeedMultiplier, _vehicleMultipliersData.VehicleMultipliersData.MaxSpeedMultiplier, ratio);
        _speedMultiplier = speedMultiplier;

    }

    private void CalculateTorqueMultiplier(int minTorque, int maxTorque, int currentTorque)
    {
        float midPoint = (minTorque + maxTorque) / 2f;
        float distanceToMidPoint = Mathf.Abs(currentTorque - midPoint);
        float ratio = distanceToMidPoint / (midPoint - minTorque);
        float torqueMultiplier = Mathf.Lerp(_vehicleMultipliersData.VehicleMultipliersData.MinTorqueMultiplier, _vehicleMultipliersData.VehicleMultipliersData.MaxTorqueMultiplier, ratio);
        _torqueMultiplier = torqueMultiplier;


    }

    private void Init()
    {
        if (Random.value < 0.05f)
        {
            _saleValue = _marketValue;
            return;
        }


        for (int i = 0; i < _totalNumberPartsInTheVehicle; i++)
        {
            int randomPartIndex = Random.Range(0, _vehiclePartTypeMultiplierList.Count);
            VehiclePartData carPartType = _vehiclePartTypeMultiplierList[randomPartIndex];

            switch (carPartType.carParts)
            {
                case CarParts.Tavan:
                case CarParts.Kaput:
                case CarParts.Bagaj:
                    _vehiclePartTypeMultiplierList.Remove(carPartType);
                    break;
                case CarParts.Kapilar:
                    maxDoor--;
                    if (maxDoor <= 0)
                        _vehiclePartTypeMultiplierList.Remove(carPartType);
                    break;
                case CarParts.YanBolge:
                    maxSide--;
                    if (maxSide <= 0)
                        _vehiclePartTypeMultiplierList.Remove(carPartType);
                    break;
            }

            _damagedCarPartsGetList.Add(carPartType);
        }

        ProcessCars();
    }

    private void ProcessCars()
    {
        float valueAdditionToSalesValue = 0;

        foreach (VehiclePartData carPart in _damagedCarPartsGetList)
        {
            bool isFullDamaged = Random.value < 0.1f;

            if (isFullDamaged)
            {
                valueAdditionToSalesValue += (carPart.PaintedPartMultiplier + carPart.DamagedPartMultiplier) * _carPriceMultiplier;

                VehiclePartData carPartClone = new VehiclePartData();

                carPartClone.carParts = carPart.carParts;
                carPartClone.IsPainted = true;
                carPartClone.IsDamaged = true;

                _vehicleDamagedCarPartsCloneList.Remove(carPart);
                _vehicleDamagedCarPartsCloneList.Add(carPartClone);
            }
            else
            {
                bool isPainted = Random.value < 0.7f;
                bool isDamaged = isPainted ? false : true;
                valueAdditionToSalesValue += (isPainted) ? (carPart.PaintedPartMultiplier * _carPriceMultiplier) : carPart.DamagedPartMultiplier * _carPriceMultiplier;
                VehiclePartData carPartClone = new VehiclePartData();

                carPartClone.carParts = carPart.carParts;
                carPartClone.IsPainted = isPainted;
                carPartClone.IsDamaged = isDamaged;

                _vehicleDamagedCarPartsCloneList.Remove(carPart);
                _vehicleDamagedCarPartsCloneList.Add(carPartClone);
            }
        }
        UpdateVehiclePrice(valueAdditionToSalesValue);
    }

    private void UpdateVehiclePrice(float valueAdditionToSalesValue)
    {
        valueAdditionToSalesValue -= (_speedMultiplier + _torqueMultiplier) * 500;
        _saleValue = (_marketValue - valueAdditionToSalesValue);
        if (_saleValue < 10000)
            _saleValue = Mathf.Max(_saleValue, 10000);

    }

    public int GetMarketValue()
    {
        return _marketValue;
    }

    public int GetSalesValue()
    {
        return (int)_saleValue;
    }

    public List<VehiclePartData> GetVehiclePartInformation()
    {
        return _vehicleDamagedCarPartsCloneList;
    }

    public VehicleType_SO GetCarSO()
    {
        return _vehicleTypeData;
    }
}
