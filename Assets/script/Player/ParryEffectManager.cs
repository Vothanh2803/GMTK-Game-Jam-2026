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
    [SerializeField] private float slowMotionTimeScale = 0.1f;
    [SerializeField] private float slowMotionDuration = 0.4f;

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

    private IEnumerator DoSlowMotionRoutine()
    {
        Time.timeScale = slowMotionTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(slowMotionDuration);

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