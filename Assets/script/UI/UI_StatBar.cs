using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_StatBar : MonoBehaviour
{
    [Header("Bar Images")]
    [SerializeField] private Image fillImage;
    [SerializeField] private Image virtualFillImage; 
    [SerializeField] private RectTransform barContainer; 

    [Header("Tween Settings")]
    [SerializeField] private float mainFillDuration = 0.15f; 
    [SerializeField] private float virtualDelay = 0.4f;
    [SerializeField] private float virtualFillDuration = 0.5f;
    [SerializeField] private float punchStrength = 0.12f;

    private Tween fillTween;
    private Tween virtualTween;
    private Tween punchTween;

    private float currentRatio = 1f;

    public void Initialize(float currentValue, float maxValue)
    {
        currentRatio = Mathf.Clamp01(currentValue / maxValue);

        if (fillImage != null) fillImage.fillAmount = currentRatio;
        if (virtualFillImage != null) virtualFillImage.fillAmount = currentRatio;
    }

    public void UpdateBar(float currentValue, float maxValue)
    {
        float targetRatio = Mathf.Clamp01(currentValue / maxValue);

        if (Mathf.Approximately(targetRatio, currentRatio)) return;

        if (targetRatio < currentRatio)
        {
            fillTween?.Kill();
            virtualTween?.Kill();
            punchTween?.Kill();

            if (fillImage != null)
            {
                fillTween = fillImage.DOFillAmount(targetRatio, mainFillDuration)
                    .SetEase(Ease.OutBack);
            }

            if (barContainer != null)
            {
                barContainer.localScale = Vector3.one;
                punchTween = barContainer.DOPunchScale(new Vector3(punchStrength, punchStrength, 0f), 0.2f, 8, 1f);
            }

            if (virtualFillImage != null)
            {
                virtualTween = virtualFillImage.DOFillAmount(targetRatio, virtualFillDuration)
                    .SetDelay(virtualDelay)
                    .SetEase(Ease.OutCubic);
            }
        }
        else
        {
            fillTween?.Kill();
            virtualTween?.Kill();

            if (fillImage != null)
            {
                fillTween = fillImage.DOFillAmount(targetRatio, 0.2f);
            }
            if (virtualFillImage != null)
            {
                virtualFillImage.fillAmount = targetRatio;
            }
        }

        currentRatio = targetRatio;
    }

    private void OnDestroy()
    {
        fillTween?.Kill();
        virtualTween?.Kill();
        punchTween?.Kill();
    }
}