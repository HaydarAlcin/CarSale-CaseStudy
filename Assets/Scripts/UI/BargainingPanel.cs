using DG.Tweening;
using UnityEngine;
public class BargainingPanel : MonoBehaviour
{

	public void OpenPanel()
	{
		GetComponent<CanvasGroup>().DOFade(1, .5f);
	}

	public void ClosePanel()
	{
		GetComponent<CanvasGroup>().DOFade(0, .5f).OnComplete(() => gameObject.SetActive(false));
	}
}
