using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private BCG_EnterExitPlayer _enterExitPlayer;
    private void Start()
    {
        _enterExitPlayer = GetComponent<BCG_EnterExitPlayer>();

        CoreGameSignals.Instance.OnSalesmanWaited += OnSalesmanWaited;
        CoreGameSignals.Instance.OnBargainingHasEnded += OnBargainingHasEnded;
    }

    private void OnSalesmanWaited(bool i, DealerManager dealer)
    {
        if (InputManager.Instance.PlayerInteractThisFrame() && i)
        {
            _enterExitPlayer.CanMove = false;
            CoreUISignals.Instance.OnGetCarPartInformation?.Invoke(dealer.GetVehiclePartInformation());
            CoreUISignals.Instance.OnGetVehicleInformation?.Invoke(dealer.GetCarController());
            CoreGameSignals.Instance.OnBargainingHasStart?.Invoke(dealer);

            //Animation
            CoreAnimationSignals.Instance.OnBargainingHasStartAnimation?.Invoke(dealer, transform);

            //Cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

        }
    }

    private void OnBargainingHasEnded()
    {
        _enterExitPlayer.CanMove = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
