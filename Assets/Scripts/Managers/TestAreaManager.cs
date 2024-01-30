using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TestAreaManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BCG_EnterExitPlayer enterExitPlayer;
    [SerializeField] private Transform testVehicleInstantiateTransform;

    private BCG_EnterExitVehicle currentVehicle;
    private BCG_EnterExitVehicle previousVehicle;
    private CanvasGroup loadingPanelCanvasGroup;
    
    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        CoreGameSignals.Instance.OnTestingPhaseHasStarted += OnTestingPhaseHasStarted;
        CoreGameSignals.Instance.OnTestingPhaseHasEnded += OnTestingPhaseHasEnded;
    }

    private void OnTestingPhaseHasStarted(GameObject testVehicle,CanvasGroup loadingPanel)
    {
        //LoadingPanel
        loadingPanel.DOFade(0, 2f).OnComplete(() => loadingPanel.gameObject.SetActive(false));
        loadingPanelCanvasGroup = loadingPanel;

        currentVehicle=testVehicle.GetComponent<BCG_EnterExitVehicle>();

        if (previousVehicle != null)
            Destroy(previousVehicle);

        CoreUISignals.Instance.OnBargaininPanelClosed?.Invoke();
        var temporaryTestVehicle = Instantiate(testVehicle, testVehicleInstantiateTransform.position, Quaternion.identity);
        var enterExitVehicle= temporaryTestVehicle.GetComponent<BCG_EnterExitVehicle>();
        previousVehicle = enterExitVehicle;
        enterExitPlayer.GetIn(previousVehicle);
        enterExitPlayer.playerStartsAsInVehicle = true;
        enterExitPlayer.inVehicle = previousVehicle;

    }

    private void OnTestingPhaseHasEnded(GameObject exitButton)
    {
        exitButton.SetActive(false);
        loadingPanelCanvasGroup.gameObject.SetActive(true);
        loadingPanelCanvasGroup.DOFade(1, 1.5f).OnComplete(() =>
        TestedEnding()
        ) ;
    }

    private void TestedEnding()
    {
        loadingPanelCanvasGroup.DOFade(0, 1.5f).OnComplete(() => loadingPanelCanvasGroup.gameObject.SetActive(false));
        enterExitPlayer.inVehicle = currentVehicle;
        enterExitPlayer.GetOut();
        enterExitPlayer.CanMove = true;
    }
}
