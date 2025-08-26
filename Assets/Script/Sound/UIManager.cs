using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public float FadeTime = 1f;
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;
     public void PanelFadeIn()
    {
        canvasGroup.alpha = 0f;
        rectTransform.transform.localPosition = new Vector3(0f, -1000f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, 0f), FadeTime, false).SetEase(Ease.OutElastic);
        canvasGroup.DOFade(1, FadeTime);
    }

    public void PanelFadeOut()
    {
        canvasGroup.alpha = 1f;
        rectTransform.transform.localPosition = new Vector3(0f, 0f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, -2200f), FadeTime, false).SetEase(Ease.InOutQuint);
        canvasGroup.DOFade(0, FadeTime);
    }
}
