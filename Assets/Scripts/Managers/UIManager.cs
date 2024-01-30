using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Image playerImage;
    [SerializeField] private Player_SO playerDialogSO;
    [SerializeField] private TextMeshProUGUI playerNameSurnameText;
    [SerializeField] private TextMeshProUGUI playerMoneyText;

    [Space]
    [Header("Dealer")]
    [SerializeField] private Image dealerImage;
    [SerializeField] private TextMeshProUGUI dealerNameSurnameText;
    [SerializeField] private TextMeshProUGUI dealerDialogText;
    private DealerManager _currentDealer;
    private DealerDialog_SO dealerSO;

    [Space]
    [Header("Vehicle")]
    [SerializeField] private TextMeshProUGUI vehicleModelText;
    [SerializeField] private TextMeshProUGUI vehicleSaleValueText;
    [SerializeField] private TextMeshProUGUI vehicleMarketValueText;

    [Space]
    [Header("UI References")]
    [SerializeField] private Button makeOfferButton;
    [SerializeField] private Button withdrawOfferButton;
    [SerializeField] private Button testAreaButton;
    [SerializeField] private Button exitTestAreaButton;
    [SerializeField] private TMP_InputField offerInputField;
    [SerializeField] private RectTransform bargainingPanelRectTransform;
    [SerializeField] private RectTransform partsInformationRectTransform;
    [SerializeField] private RectTransform vehicleInformationRectTransform;
    [SerializeField] private GameObject partInformationPrefabGameObject;
    [SerializeField] private GameObject vehicleInformationPrefabGameObject;
    [SerializeField] private BargainingPanel bargainingPanel;
    [SerializeField] private CanvasGroup testAreaLoadingPanel;


    private void Start()
    {
        playerImage.sprite = playerDialogSO.PlayerData.PlayerSprite;
        playerNameSurnameText.text = playerDialogSO.PlayerData.PlayerName;
        int temporaryPlayerMoneyValue = playerDialogSO.PlayerData.Money;
        playerMoneyText.text = string.Format("{0:N0}", temporaryPlayerMoneyValue) + "$";
    }



    private void OnEnable()
    {
        SubscribeEvents();
        makeOfferButton.onClick.AddListener(OnControlOffer);
        withdrawOfferButton.onClick.AddListener(WithdrawOffer);
        testAreaButton.onClick.AddListener(TestPhaseStarted);
        exitTestAreaButton.onClick.AddListener(TestPhaseEnded);
    }



    private void SubscribeEvents()
    {
        CoreGameSignals.Instance.OnBargainingHasStart += OnBargainingHasStart;
        CoreUISignals.Instance.OnGetCarPartInformation += OnGetCarPartInformation;
        CoreUISignals.Instance.OnGetVehicleInformation += OnGetVehicleInformation;
        CoreUISignals.Instance.OnUpdatedPlayerMoneyText += OnUpdatedPlayerMoneyText;
        CoreUISignals.Instance.OnOfferInvalid += OnOfferInvalid;
        CoreUISignals.Instance.OnBargaininPanelClosed += OnBargaininPanelClosed;
        CoreGameSignals.Instance.OnBargainingHasEnded += OnBargainingHasEnded;
    }



    private void OnBargainingHasStart(DealerManager dealer)
    {
        #region Vehicle Info
        vehicleModelText.text = dealer.GetVehicleTypeSO().VehicleTypeData.CarModel;
        int temporarySaleValue = dealer.GetSaleValue();
        vehicleSaleValueText.text = string.Format("{0:N0}", temporarySaleValue);
        int temporaryMarketValue = dealer.GetMarketValue();
        vehicleMarketValueText.text = string.Format("{0:N0}", temporaryMarketValue);

        #endregion


        #region Dealer
        _currentDealer = dealer;
        dealerSO = dealer.GetDealerSO();
        dealerImage.sprite = dealerSO.SalesmanDialogImage;
        dealerNameSurnameText.text = dealerSO.NameSurname;
        dealerDialogText.text = $"Selam, aracýmýn fiyatý {vehicleSaleValueText.text}$. Eðer ciddi alýcýysanýz pazarlýk konusunda anlaþabiliriz";
        #endregion

        bargainingPanelRectTransform.gameObject.SetActive(true);
        bargainingPanel.OpenPanel();
    }


    private void OnGetCarPartInformation(List<VehiclePartData> partType)
    {

        foreach (var item in partType)
        {
            bool isPainted = item.IsPainted;
            bool isDamaged = item.IsDamaged;
            string partName = item.carParts.ToString();

            var partGameObject = Instantiate(partInformationPrefabGameObject, partsInformationRectTransform);

            var partText = partGameObject.GetComponentInChildren<TextMeshProUGUI>();
            var paintedToogle = partGameObject.transform.GetChild(1).GetComponent<Toggle>();
            var damagedToogle = partGameObject.transform.GetChild(2).GetComponent<Toggle>();

            partText.text = partName;
            paintedToogle.isOn = isPainted;
            damagedToogle.isOn = isDamaged;
        }
    }

    private void OnGetVehicleInformation(RCC_CarControllerV3 carController)
    {

        foreach (Transform item in vehicleInformationRectTransform)
        {
            Destroy(item.gameObject);
        }

        CreateVehicleInfoText("Maksimum Hýz", carController.maxspeed);
        CreateVehicleInfoText("Maksimum Tork", carController.maxEngineTorque);
        CreateVehicleInfoText("Vites", carController.totalGears);
        float useNosFloat = carController.useNOS ? 1.0f : 0.0f;
        CreateVehicleInfoText("NOS", useNosFloat);
    }

    private void CreateVehicleInfoText(string labelText, float value)
    {
        var vehicleGameObject = Instantiate(vehicleInformationPrefabGameObject, vehicleInformationRectTransform);
        var vehicleInfoText = vehicleGameObject.GetComponentInChildren<TextMeshProUGUI>();
        vehicleInfoText.text = $"{labelText}: {value}";
    }

    private void OnBargainingHasEnded()
    {
        if (partsInformationRectTransform.childCount > 0)
        {
            foreach (Transform item in partsInformationRectTransform)
            {
                Destroy(item.gameObject);
            }
        }
        _currentDealer = null;
        

    }


    private void OnBargaininPanelClosed()
    {
        bargainingPanel.ClosePanel();
    }

    private void OnControlOffer()
    {
        int temporaryOffer = 0;

        if (int.TryParse(offerInputField.text, out int offer))
        {
            temporaryOffer = offer;

            bool moneyControl = CoreGameSignals.Instance.TriggerOfferAndAvailableMoneyControl(temporaryOffer);

            if (moneyControl)
            {
                CoreGameSignals.Instance.OnMakeOffer?.Invoke(temporaryOffer, _currentDealer);
            }
            else
            {
                dealerDialogText.text = "Veresiye yok kardeþim!!";
            }
        }
    }


    private void OnOfferInvalid(int offer)
    {
        dealerDialogText.text = $"Heyy saçma sapan fiyat verip sinirimi bozma, {offer}$ anca aracýn tekerini alýrsýn!";
    }


    private void OnUpdatedPlayerMoneyText(int money)
    {

        playerMoneyText.text = string.Format("{0:N0}", money) + "$";
    }

    private void WithdrawOffer()
    {
        CoreGameSignals.Instance.OnBargainingHasEnded?.Invoke();
        OnBargaininPanelClosed();
    }

    private void TestPhaseStarted()
    {
        CoreGameSignals.Instance.OnDecreasePlayerMoney?.Invoke(5000);
        testAreaLoadingPanel.gameObject.SetActive(true);
        testAreaLoadingPanel.DOFade(1, 1f).OnComplete(
            () =>
            CoreGameSignals.Instance.OnTestingPhaseHasStarted?.Invoke(_currentDealer.GetCarController().gameObject,testAreaLoadingPanel));

        exitTestAreaButton.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void TestPhaseEnded()
    {
        CoreGameSignals.Instance.OnTestingPhaseHasEnded?.Invoke(exitTestAreaButton.gameObject);
    }
}
