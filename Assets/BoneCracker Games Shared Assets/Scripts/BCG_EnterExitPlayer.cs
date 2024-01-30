//----------------------------------------------
//            BCG Shared Assets
//
// Copyright © 2014 - 2021 BoneCracker Games
// https://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Enter Exit for FPS Player.
/// </summary>
[AddComponentMenu("BoneCracker Games/Shared Assets/Enter-Exit/BCG Enter Exit Script For Player Characters")]
public class BCG_EnterExitPlayer : MonoBehaviour
{

    #region HaydarAlcin Added
    private PlayerManager _playerManager;
    private bool _canISendTheRaycast;
    #endregion

    public bool isTPSController = false;
    public float rayHeight = 1f;


    //HaydarAlcin
    public bool CanMove = true;

    public bool canControl = true;
    public bool showGui = false;
    public BCG_EnterExitVehicle targetVehicle;

    public bool playerStartsAsInVehicle = false;
    public BCG_EnterExitVehicle inVehicle;

    public Camera characterCamera;

    public delegate void onBCGPlayerSpawned(BCG_EnterExitPlayer player);
    public static event onBCGPlayerSpawned OnBCGPlayerSpawned;

    public delegate void onBCGPlayerDestroyed(BCG_EnterExitPlayer player);
    public static event onBCGPlayerDestroyed OnBCGPlayerDestroyed;

    public delegate void onBCGPlayerEnteredAVehicle(BCG_EnterExitPlayer player, BCG_EnterExitVehicle vehicle);
    public static event onBCGPlayerEnteredAVehicle OnBCGPlayerEnteredAVehicle;

    public delegate void onBCGPlayerExitedFromAVehicle(BCG_EnterExitPlayer player, BCG_EnterExitVehicle vehicle);
    public static event onBCGPlayerExitedFromAVehicle OnBCGPlayerExitedFromAVehicle;

    void Awake()
    {

        if (!playerStartsAsInVehicle)
            inVehicle = null;

        if (!isTPSController)
            characterCamera = GetComponentInChildren<Camera>();

        //HaydarAlcin
        CoreGameSignals.Instance.OnSendRaycast += OnSendRaycast;
        _canISendTheRaycast = false;

        _playerManager = GetComponent<PlayerManager>();

    }

    void OnEnable()
    {

        if (OnBCGPlayerSpawned != null)
            OnBCGPlayerSpawned(this);

        if (playerStartsAsInVehicle)
            StartCoroutine(StartInVehicle());

    }

    IEnumerator StartInVehicle()
    {

        yield return new WaitForFixedUpdate();
        GetIn(inVehicle);

    }

    public void GetIn(BCG_EnterExitVehicle vehicle)
    {

        if (OnBCGPlayerEnteredAVehicle != null)
            OnBCGPlayerEnteredAVehicle(this, vehicle);

    }

    public void GetOut()
    {

        if (inVehicle == null)
            return;

        if (inVehicle.speed > BCG_EnterExitSettings.Instance.enterExitSpeedLimit)
            return;

        if (OnBCGPlayerExitedFromAVehicle != null)
            OnBCGPlayerExitedFromAVehicle(this, inVehicle);


    }

    private void Update()
    {

        HandleEnterExitControl();
    }

    private void HandleEnterExitControl()
    {
        if (!canControl)
            return;
        if (!_canISendTheRaycast)
            return;

        Vector3 rayPosition;
        Quaternion rayRotation = new Quaternion();

        if (characterCamera && !isTPSController)
        {

            rayPosition = characterCamera.transform.position;
            rayRotation = characterCamera.transform.rotation;

        }
        else
        {

            rayPosition = transform.position + (Vector3.up * rayHeight);
            rayRotation = transform.rotation;

        }

        Vector3 rayDirection = rayRotation * Vector3.forward;
        RaycastHit hit;



        Debug.DrawRay(rayPosition, rayDirection * 1.5f, Color.blue);
        if (Physics.Raycast(rayPosition, rayDirection, out hit, 1.5f))
        {
            if (!targetVehicle)
            {
                //targetVehicle = hit.collider.transform.GetComponentInParent<BCG_EnterExitVehicle>();
                VehicleManager vehicleManager = hit.collider.GetComponentInParent<VehicleManager>();
                if (vehicleManager != null)
                {
                    if (_playerManager.GetOwnedVehicles().Contains(vehicleManager.transform))
                    {
                        Debug.Log("Girdi");
                        targetVehicle = hit.collider.transform.GetComponentInParent<BCG_EnterExitVehicle>();
                    }
                }
                
            }
            else
            {

                //if (Input.GetKeyDown(BCG_EnterExitSettings.Instance.enterExitVehicleKB))
                //    GetIn(targetVehicle);
                VehicleManager vehicleManager = hit.collider.GetComponentInParent<VehicleManager>();
                if (vehicleManager!=null)
                {
                    showGui = true;
                }
            }

            #region HaydarAlcin Added
            if (hit.collider.gameObject.TryGetComponent(out DealerManager salesman))
            {
                salesman.OutlineActive();
                CoreUISignals.Instance.OnBarainingPanelWaited?.Invoke(true);
                CoreGameSignals.Instance.OnSalesmanWaited?.Invoke(true, salesman);
            }
            #endregion
        }
        else
        {
            showGui = false;
            targetVehicle = null;

            //HaydarAlcin Added
            CoreUISignals.Instance.OnBarainingPanelWaited?.Invoke(false);
            CoreGameSignals.Instance.OnSalesmanOutlineDeactive?.Invoke();

        }
    }

    private void OnGUI()
    {

        if (showGui)
        {

            GUI.skin.label.fontSize = 36;
            GUI.Label(new Rect((Screen.width / 2f) - 30f, (Screen.height / 2f) - 35f, 600f, 50f), "F: Bin");

        }

    }

    void OnDestroy()
    {

        if (OnBCGPlayerDestroyed != null)
            OnBCGPlayerDestroyed(this);

    }


    #region HaydarAlcin Added
    private void OnSendRaycast(bool canSend)
    {
        _canISendTheRaycast = canSend;
    }

    public IGetInteraction GetSalesmanCollider(IGetInteraction interaction)
    {
        return interaction;
    }
    #endregion


}
