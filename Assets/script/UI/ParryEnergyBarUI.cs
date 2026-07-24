using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ParryEnergyBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerParry playerParry;
    [SerializeField] private Image fillImage;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Hold Threshold")]
    [SerializeField] private float holdThreshold = 0.15f;

    [Header("Shake Effect")]
    [SerializeField] private float shakeStrength = 8f;
    [SerializeField] private int shakeVibrato = 20;

    [Header("Animation Speeds")]
    [SerializeField] private float fadeInDuration = 0.15f;
    [SerializeField] private float fadeOutDuration = 0.2f;
    [SerializeField] private float failDrainDuration = 0.15f;

    private float currentHoldTimer = 0f;
    private bool isBarVisible = false;
    private bool isShaking = false;
    private bool isDrainingOnFail = false;

    private float startEnergyOnShow = 0f;

    private Tween fadeTween;
    private Tween shakeTween;
    private Tween drainTween;
    private Vector3 originalPosition;

    private void Awake()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = transform.localPosition;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void Start()
    {
        if (playerParry == null)
        {
            playerParry = FindFirstObjectByType<PlayerParry>();
        }
    }

    private void Update()
    {
        if (playerParry == null) return;

        if (isDrainingOnFail) return;

        float energy = playerParry.CurrentEnergy;

        if (energy > 0f)
        {
            currentHoldTimer += Time.deltaTime;

            if (currentHoldTimer >= holdThreshold)
            {
                if (!isBarVisible)
                {
                    startEnergyOnShow = energy; 
                    ShowBar();
                }

                if (fillImage != null)
                {
                    float remainingRange = 100f - startEnergyOnShow;

                    if (remainingRange > 0f)
                    {
                        float normalizedFill = (energy - startEnergyOnShow) / remainingRange;
                        fillImage.fillAmount = Mathf.Clamp01(normalizedFill);
                    }
                    else
                    {
                        fillImage.fillAmount = 1f;
                    }
                }

                if (energy >= 100f && !isShaking)
                {
                    StartFullEnergyShake();
                }
            }
        }
        else
        {
            currentHoldTimer = 0f;
            startEnergyOnShow = 0f;

            if (isBarVisible)
            {
                HideBar();
            }
        }
    }

    private void ShowBar()
    {
        isBarVisible = true;

        // Đặt ngay vạch fill về 0% tại khoảnh khắc hiện lên
        if (fillImage != null) fillImage.fillAmount = 0f;

        fadeTween?.Kill();
        fadeTween = canvasGroup.DOFade(1f, fadeInDuration).SetUpdate(true);
    }

    private void HideBar()
    {
        isBarVisible = false;
        StopFullEnergyShake();

        fadeTween?.Kill();
        fadeTween = canvasGroup.DOFade(0f, fadeOutDuration).SetUpdate(true);
    }

    private void StartFullEnergyShake()
    {
        isShaking = true;
        shakeTween?.Kill();

        shakeTween = transform.DOShakePosition(100f, new Vector3(shakeStrength, shakeStrength, 0), shakeVibrato)
            .SetLoops(-1)
            .SetUpdate(true);
    }

    private void StopFullEnergyShake()
    {
        isShaking = false;
        shakeTween?.Kill();
        transform.localPosition = originalPosition;
    }

    public void TriggerOverchargeFailUI()
    {
        if (!isBarVisible) return;

        isDrainingOnFail = true;
        StopFullEnergyShake();

        if (fillImage != null)
        {
            drainTween?.Kill();
            drainTween = fillImage.DOFillAmount(0f, failDrainDuration)
                .SetEase(Ease.InQuad)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    isDrainingOnFail = false;
                    HideBar();
                });
        }
        else
        {
            isDrainingOnFail = false;
            HideBar();
        }
    }

    private void OnDestroy()
    {
        fadeTween?.Kill();
        shakeTween?.Kill();
        drainTween?.Kill();
    }
}