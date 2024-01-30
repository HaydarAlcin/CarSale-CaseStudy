using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("References")]
    private Player_SO _playerData;
    [SerializeField] private List<Transform> ownedVehicles;
    [SerializeField] private Transform ownedVehicleListTransform;

    private int _money;

    private void Awake()
    {
        ownedVehicles = new List<Transform>();
        GetPlayerData();
        Init();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        CoreGameSignals.Instance.OnOfferValid += OnOfferValid;
        CoreGameSignals.Instance.OnOfferAndAvailableMoneyControl += OnOfferAndAvailableMoneyControl;
        CoreGameSignals.Instance.OnDecreasePlayerMoney += OnDecreasePlayerMoney;
    }

    private void OnOfferValid(int offer, Transform vehicle)
    {
        _money -= offer;
        ownedVehicles.Add(vehicle);
        vehicle.SetParent(ownedVehicleListTransform);
        CoreUISignals.Instance.OnUpdatedPlayerMoneyText?.Invoke(_money);

    }

    public bool OnOfferAndAvailableMoneyControl(int offer)
    {
        if (offer > _money)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void OnDecreasePlayerMoney(int amount)
    {
        _money -= amount;
        CoreUISignals.Instance.OnUpdatedPlayerMoneyText.Invoke(_money);
    }

    private void UnsubscribeEvents()
    {
        CoreGameSignals.Instance.OnOfferValid -= OnOfferValid;
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void GetPlayerData()
    {
        _playerData = Resources.Load<Player_SO>(path: "Player/Player_SO");
    }
    private void Init()
    {
        _money = _playerData.PlayerData.Money;
    }


    
    public List<Transform> GetOwnedVehicles()
    {
        return ownedVehicles.ToList();
    }

}
