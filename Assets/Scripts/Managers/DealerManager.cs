using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DealerManager : MonoBehaviour, IGetInteraction
{
    [SerializeField] private DealerDialog_SO dealerDialogSO;

    private VehicleManager _vehicleManager;
    private RCC_CarControllerV3 _carController;
    private Outline outline;

    DealerManager IGetInteraction.dealerManager => this;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        _vehicleManager = GetComponentInChildren<VehicleManager>();
        _carController = GetComponentInChildren<RCC_CarControllerV3>();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        CoreGameSignals.Instance.OnSalesmanOutlineDeactive += OnSalesmanOutlineDeactive;
        CoreGameSignals.Instance.OnMakeOffer += OnMakeOffer;
    }

    private void OnSalesmanOutlineDeactive()
    {
        outline.enabled = false;
    }
    private void OnMakeOffer(int offer, DealerManager dealer)
    {
        if (dealer == this)
        {

            if (offer >= GetSaleValue() * 0.9f)
            {

                CoreGameSignals.Instance.OnOfferValid?.Invoke(offer, transform.GetChild(0));
                CoreGameSignals.Instance.OnBargainingHasEnded?.Invoke();
                CoreUISignals.Instance.OnBargaininPanelClosed?.Invoke();
                
                //Animation
                CoreAnimationSignals.Instance.OnBargainingSuccessfulAnimation?.Invoke(this);
                Destroy(this.gameObject, 3f);

            }

            else
            {
                CoreUISignals.Instance.OnOfferInvalid?.Invoke(offer);

                //Animation
                CoreAnimationSignals.Instance.OnBargainingFailAnimation?.Invoke(this);
            }
        }
    }

    private void UnSubscribeEvents()
    {
        CoreGameSignals.Instance.OnSalesmanOutlineDeactive -= OnSalesmanOutlineDeactive;
        CoreGameSignals.Instance.OnMakeOffer += OnMakeOffer;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    #region Interface Methods
    public void Interaction()
    {

    }

    public void OutlineActive()
    {
        outline.enabled = true;
    }
    #endregion


    public DealerDialog_SO GetDealerSO()
    {
        return dealerDialogSO;
    }

    public VehicleManager GetVehicleManager()
    {
        return this._vehicleManager;
    }

    public RCC_CarControllerV3 GetCarController()
    {
        return _carController;
    }

    public int GetSaleValue()
    {
        return _vehicleManager.GetSalesValue();
    }
    public int GetMarketValue()
    {
        return _vehicleManager.GetMarketValue();
    }

    public List<VehiclePartData> GetVehiclePartInformation()
    {
        return _vehicleManager.GetVehiclePartInformation();
    }

    public VehicleType_SO GetVehicleTypeSO()
    {
        return _vehicleManager.GetCarSO();
    }
}
