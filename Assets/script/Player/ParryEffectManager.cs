using UnityEngine;
using System.Collections;

public class ParryEffectManager : MonoBehaviour
{
    public static ParryEffectManager Instance { get; private set; }

    [Header("Camera Shake Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float shakeIntensity = 0.25f;
    [SerializeField] private float shakeDuration = 0.15f;

    [Header("Full Charge Slow-Motion Settings")]
    [Range(0.01f, 1f)]
    [SerializeField] private float slowMotionTimeScale = 0.05f;

    [Tooltip("Thời gian GIỮ ĐỨNG KHỰNG thời gian hoàn toàn (Hitstop/Impact Phase)")]
    [SerializeField] private float holdDuration = 0.1f;

    [Tooltip("Thời gian TĂNG TỐC DẦN từ Slow-Motion trở lại bình thường (Recovery Phase)")]
    [SerializeField] private float recoverDuration = 0.2f;

    [Tooltip("Độ cong tăng tốc: 1 = Đều, 2 = Chậm lúc đầu rồi vọt nhanh (Đề xuất: 2 - 3)")]
    [SerializeField] private float recoverEasePower = 2f;

    private Coroutine slowMotionRoutine;
    private Coroutine cameraShakeRoutine;
    private Vector3 originalCameraPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    public void TriggerFullChargeParrySlowMotion()
    {
        if (slowMotionRoutine != null) StopCoroutine(slowMotionRoutine);
        slowMotionRoutine = StartCoroutine(DoSlowMotionRoutine());

        TriggerCameraShake();
    }

    public void TriggerCameraShake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null) return;
        }

        if (cameraShakeRoutine != null)
        {
            StopCoroutine(cameraShakeRoutine);
            mainCamera.transform.localPosition = originalCameraPosition;
        }

        cameraShakeRoutine = StartCoroutine(DoCameraShakeRoutine());
    }

    /// <summary>
    /// 🔥 Khựng cứng -> Giữ vững Hitstop -> Tăng tốc mượt dần về lại bình thường
    /// </summary>
    private IEnumerator DoSlowMotionRoutine()
    {
        // -------------------------------------------------------------
        // BƯỚC 1: ĐỨNG KHỰNG HOÀN TOÀN (HITSTOP PHASE)
        // -------------------------------------------------------------
        Time.timeScale = slowMotionTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Giữ nguyên mức làm chậm trong đúng holdDuration
        yield return new WaitForSecondsRealtime(holdDuration);

        // -------------------------------------------------------------
        // BƯỚC 2: TĂNG TỐC MƯỢT VỀ LẠI 1.0 (RECOVERY PHASE)
        // -------------------------------------------------------------
        float elapsed = 0f;

        while (elapsed < recoverDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / recoverDuration);

            // Dùng đường cong Power (Mathf.Pow) để nhịp tăng tốc phi tuyến tính (mượt & có lực hơn)
            float easedT = Mathf.Pow(t, recoverEasePower);

            Time.timeScale = Mathf.Lerp(slowMotionTimeScale, 1f, easedT);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            yield return null;
        }

        ResetTimeScale();
    }

    private IEnumerator DoCameraShakeRoutine()
    {
        originalCameraPosition = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            randomOffset.z = 0f; 

            mainCamera.transform.localPosition = originalCameraPosition + randomOffset;

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalCameraPosition;
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    private void OnDestroy()
    {
        ResetTimeScale();
    }
}