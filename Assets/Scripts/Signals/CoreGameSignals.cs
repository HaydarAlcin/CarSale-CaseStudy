using System;
using UnityEngine;
using UnityEngine.Events;

public class CoreGameSignals : MonoBehaviour
{
    #region Singleton
    private static CoreGameSignals _instance;
    public static CoreGameSignals Instance
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
    public UnityAction OnSalesmanOutlineDeactive = delegate { };
    public UnityAction<bool> OnSendRaycast = delegate { };

    public UnityAction<bool, DealerManager> OnSalesmanWaited = delegate { };
    public UnityAction<DealerManager> OnBargainingHasStart = delegate { };

    public UnityAction<int, DealerManager> OnMakeOffer = delegate { };
    public UnityAction OnBargainingHasEnded = delegate { };

    public UnityAction<int, Transform> OnOfferValid = delegate { };

    public event Func<int, bool> OnOfferAndAvailableMoneyControl;

    public bool TriggerOfferAndAvailableMoneyControl(int offer)
    {
        return OnOfferAndAvailableMoneyControl?.Invoke(offer) ?? false;
    }

    public UnityAction<int> OnDecreasePlayerMoney=delegate { };

    //TEST AREA
    public UnityAction<GameObject,CanvasGroup> OnTestingPhaseHasStarted=delegate { };
    public UnityAction<GameObject> OnTestingPhaseHasEnded=delegate { };
    #endregion
}
