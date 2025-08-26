using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.AI;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public float hoverScale = 1.1f;
    public float pressScale = 0.9f;
    public float duration = 0.2f;
    private Vector3 originalScale;
  

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(originalScale * hoverScale, duration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, duration).SetEase(Ease.OutBack);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(originalScale * pressScale, duration / 2).SetEase(Ease.OutBack);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(originalScale * hoverScale, duration / 2).SetEase(Ease.OutBack);
    }
}
