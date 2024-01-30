using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractController : MonoBehaviour
{
    private List<Collider> interactColliders = new List<Collider>();


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IGetInteraction interaction) || other.gameObject.TryGetComponent(out VehicleManager carManager))
        {
            interactColliders.Add(other);
            CoreGameSignals.Instance.OnSendRaycast?.Invoke(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IGetInteraction interaction) || other.gameObject.TryGetComponent(out VehicleManager carManager))
        {
            interactColliders.Remove(other);
            if (interactColliders.Count <= 0)
            {
                CoreGameSignals.Instance.OnSendRaycast?.Invoke(false);
                CoreGameSignals.Instance.OnSalesmanOutlineDeactive?.Invoke();
            }
        }
    }
}
