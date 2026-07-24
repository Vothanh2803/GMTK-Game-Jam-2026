using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(CanvasGroup))]
public class UIButtonEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [Header("Press Animation Settings")]
    [Tooltip("Tỷ lệ thu nhỏ khi nhấn giữ (0.9 = thu nhỏ 90%)")]
    [SerializeField] private float pressedScale = 0.9f;
    [Tooltip("Thời gian thu nhỏ khi bấm xuống (giây)")]
    [SerializeField] private float pressDuration = 0.08f;
    [Tooltip("Thời gian nảy đàn hồi trở lại (giây)")]
    [SerializeField] private float releaseDuration = 0.2f;

    [Header("Disabled State Settings")]
    [Tooltip("Độ mờ khi nút KHÔNG đủ điều kiện bấm (0.4 = mờ 60%)")]
    [Range(0.1f, 1f)]
    [SerializeField] private float disabledAlpha = 0.4f;
    [Tooltip("Độ mờ khi nút ĐỦ điều kiện bấm")]
    [Range(0.1f, 1f)]
    [SerializeField] private float enabledAlpha = 1f;

    [Header("Denied Shake Settings (Khi cố bấm nút đang mờ)")]
    [Tooltip("Độ lắc khi cố bấm nút bị khóa")]
    [SerializeField] private float shakeStrength = 10f;
    [Tooltip("Thời gian lắc (giây)")]
    [SerializeField] private float shakeDuration = 0.25f;

    private Button button;
    private CanvasGroup canvasGroup;
    private Vector3 originalScale;
    private Tween scaleTween;
    private Tween shakeTween;
    private Tween fadeTween;
    
    public bool IsInteractable { get; private set; } = true;
    public bool IsVisible { get; private set; } = true;

    private void Awake()
    {
        button = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalScale = transform.localScale;
    }

    public void SetInteractable(bool interactable)
    {
        if (!IsVisible) return; // Nếu nút đang bị ẨN thì không xử lý mờ sáng

        IsInteractable = interactable;
        button.interactable = interactable;

        fadeTween?.Kill();
        fadeTween = canvasGroup.DOFade(interactable ? enabledAlpha : disabledAlpha, 0.2f)
            .SetUpdate(true);
    }

    public void SetVisible(bool visible)
    {
        if (IsVisible == visible) return;

        IsVisible = visible;
        fadeTween?.Kill();

        if (visible)
        {
            // Hiện nút lên
            gameObject.SetActive(true);
            canvasGroup.alpha = 0f;
            IsInteractable = true;
            button.interactable = true;

            fadeTween = canvasGroup.DOFade(enabledAlpha, 0.2f).SetUpdate(true);
        }
        else
        {
            // Ẩn nút đi
            IsInteractable = false;
            button.interactable = false;

            fadeTween = canvasGroup.DOFade(0f, 0.15f)
                .SetUpdate(true)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsInteractable || !IsVisible) return;

        scaleTween?.Kill();
        scaleTween = transform.DOScale(originalScale * pressedScale, pressDuration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!IsInteractable || !IsVisible) return;

        scaleTween?.Kill();
        scaleTween = transform.DOScale(originalScale, releaseDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsVisible) return;

        if (!IsInteractable)
        {
            shakeTween?.Kill();
            transform.localScale = originalScale;
            shakeTween = transform.DOShakePosition(shakeDuration, new Vector3(shakeStrength, 0, 0), 10, 90)
                .SetUpdate(true);
        }
    }

    private void OnDestroy()
    {
        scaleTween?.Kill();
        shakeTween?.Kill();
        fadeTween?.Kill();
    }

    public void SimulateClick()
    {
        if (!IsVisible || !IsInteractable) return;

        Sequence pressSeq = DOTween.Sequence();
        pressSeq.Append(transform.DOScale(originalScale * pressedScale, pressDuration).SetEase(Ease.OutQuad))
                .Append(transform.DOScale(originalScale, releaseDuration).SetEase(Ease.OutBack))
                .SetUpdate(true);
    }

    public void SimulatePressDown()
    {
        if (!IsVisible || !IsInteractable) return;

        scaleTween?.Kill();
        scaleTween = transform.DOScale(originalScale * pressedScale, pressDuration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true);
    }

    public void SimulatePressUp()
    {
        if (!IsVisible || !IsInteractable) return;

        scaleTween?.Kill();
        scaleTween = transform.DOScale(originalScale, releaseDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }
}