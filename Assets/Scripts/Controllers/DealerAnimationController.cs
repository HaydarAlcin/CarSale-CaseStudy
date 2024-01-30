using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class DealerAnimationController : MonoBehaviour
{
    private DealerManager _dealerManager;
    private Transform _meshDealer;
    [SerializeField] private Animator anim;

    private void Start()
    {
        _dealerManager = GetComponent<DealerManager>();
        _meshDealer = anim.transform;
    }
    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        CoreAnimationSignals.Instance.OnBargainingHasStartAnimation += OnBargainingHasStartAnimation;
        CoreAnimationSignals.Instance.OnBargainingFailAnimation += OnBargainingFailAnimation;
        CoreAnimationSignals.Instance.OnBargainingSuccessfulAnimation += OnBargainingSuccessfulAnimation;

    }

    private void OnBargainingHasStartAnimation(DealerManager dealer,Transform playerTransform)
    {
        //Start
        if (_dealerManager == dealer)
        {
            _meshDealer.DOLookAt(new Vector3(playerTransform.position.x,_meshDealer.position.y, playerTransform.position.z), 1.5f).OnComplete(()=> anim.SetTrigger("Welcome"));
            
        }
    }

    private void OnBargainingFailAnimation(DealerManager dealer)
    {
        //Fail
        if (_dealerManager == dealer)
        {
            anim.SetTrigger("Failed");
        }
    }

    private void OnBargainingSuccessfulAnimation(DealerManager dealer)
    {
        //Successful
        if (_dealerManager == dealer)
        {
            anim.SetTrigger("Successful");
        }
    }

    private void UnsubscribeEvents()
    {
        CoreAnimationSignals.Instance.OnBargainingHasStartAnimation += OnBargainingHasStartAnimation;
        CoreAnimationSignals.Instance.OnBargainingFailAnimation += OnBargainingFailAnimation;
        CoreAnimationSignals.Instance.OnBargainingSuccessfulAnimation += OnBargainingSuccessfulAnimation;
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }


}
