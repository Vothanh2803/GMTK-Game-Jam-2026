using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_StatBar : MonoBehaviour
{
    [Header("Bar Images")]
    [SerializeField] private Image fillImage;         // Thanh chính (HP/Rage)
    [SerializeField] private Image virtualFillImage;  // Thanh ảo trễ phía sau (Có thể để trống nếu không dùng)
    [SerializeField] private RectTransform barContainer; // Khung để nẩy/rung khi thay đổi

    [Header("Tween Settings")]
    [SerializeField] private float mainFillDuration = 0.15f;  // Thời gian thanh chính sụt
    [SerializeField] private float virtualDelay = 0.4f;         // Thời gian chờ trước khi thanh ảo chạy
    [SerializeField] private float virtualFillDuration = 0.5f;// Thời gian thanh ảo sụt
    [SerializeField] private float punchStrength = 0.12f;       // Độ giật nẩy của khung

    private Tween fillTween;
    private Tween virtualTween;
    private Tween punchTween;

    private float currentRatio = 1f;

    /// <summary>
    /// Khởi tạo giá trị ban đầu cho thanh (ví dụ: 100/100 -> ratio = 1)
    /// </summary>
    public void Initialize(float currentValue, float maxValue)
    {
        currentRatio = Mathf.Clamp01(currentValue / maxValue);

        if (fillImage != null) fillImage.fillAmount = currentRatio;
        if (virtualFillImage != null) virtualFillImage.fillAmount = currentRatio;
    }

    /// <summary>
    /// Cập nhật giá trị mới cho thanh (Tự động kích hoạt hiệu ứng Dotween)
    /// </summary>
    public void UpdateBar(float currentValue, float maxValue)
    {
        float targetRatio = Mathf.Clamp01(currentValue / maxValue);

        // Nếu giá trị không đổi thì bỏ qua
        if (Mathf.Approximately(targetRatio, currentRatio)) return;

        if (targetRatio < currentRatio)
        {
            // --- GIẢM GIÁ TRỊ (MẤT MÁU / DÙNG RAGE) ---
            fillTween?.Kill();
            virtualTween?.Kill();
            punchTween?.Kill();

            // 1. Thanh chính giật lùi về
            if (fillImage != null)
            {
                fillTween = fillImage.DOFillAmount(targetRatio, mainFillDuration)
                    .SetEase(Ease.OutBack);
            }

            // 2. Rung nhẹ khung bar
            if (barContainer != null)
            {
                barContainer.localScale = Vector3.one;
                punchTween = barContainer.DOPunchScale(new Vector3(punchStrength, punchStrength, 0f), 0.2f, 8, 1f);
            }

            // 3. Thanh ảo trượt về sau khoảng delay
            if (virtualFillImage != null)
            {
                virtualTween = virtualFillImage.DOFillAmount(targetRatio, virtualFillDuration)
                    .SetDelay(virtualDelay)
                    .SetEase(Ease.OutCubic);
            }
        }
        else
        {
            // --- TĂNG GIÁ TRỊ (HỒI MÁU / TĂNG RAGE) ---
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