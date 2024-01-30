using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoreUISignals : MonoBehaviour
{
    #region Singleton
    private static CoreUISignals _instance;
    public static CoreUISignals Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance);
            return;
        }
        _instance = this;
    }
    #endregion

    #region Signals
    public UnityAction<bool> OnBarainingPanelWaited = delegate { }; //Klavye ile beklemede
    //public UnityAction OnBarainingPanelOpen = delegate { }; //Pazarlik paneli acik
    public UnityAction OnBargaininPanelClosed = delegate { }; //Pazarlik paneli kapali

    public UnityAction<List<VehiclePartData>> OnGetCarPartInformation = delegate { };
    public UnityAction<RCC_CarControllerV3> OnGetVehicleInformation = delegate { };
    public UnityAction<int> OnOfferInvalid = delegate { };

    public UnityAction<int> OnUpdatedPlayerMoneyText = delegate { };

    public UnityAction OnLoadingPanelDeactive=delegate { };
    #endregion
}
