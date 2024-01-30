using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoreAnimationSignals : MonoBehaviour
{
    #region Singleton
    private static CoreAnimationSignals _instance;
    public static CoreAnimationSignals Instance
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
    public UnityAction<DealerManager,Transform> OnBargainingHasStartAnimation;
    public UnityAction<DealerManager> OnBargainingFailAnimation;
    public UnityAction<DealerManager> OnBargainingSuccessfulAnimation;
    #endregion
}
