using UnityEngine;
using System.Collections;

public class ParryEffectManager : MonoBehaviour
{
    public static ParryEffectManager Instance { get; private set; }

    [Header("Camera Shake Settings")]
    [Tooltip("Kéo Camera chính vào đây (nếu trống script sẽ tự tìm Camera.main)")]
    [SerializeField] private Camera mainCamera;
    
    [Tooltip("Độ mạnh của cú giật camera")]
    [SerializeField] private float shakeIntensity = 0.25f;

    [Tooltip("Thời gian rung camera (tính theo giây thực tế)")]
    [SerializeField] private float shakeDuration = 0.15f;

    [Header("Full Charge Slow-Motion Settings")]
    [Tooltip("Tốc độ thời gian khi làm chậm (0.1 = chậm 10 lần)")]
    [Range(0.01f, 1f)]
    [SerializeField] private float slowMotionTimeScale = 0.1f;

    [Tooltip("Thời gian làm chậm tính theo thời gian THỰC tế ngoài đời (giây)")]
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

    /// <summary>
    /// Kích hoạt chuỗi hiệu ứng khi Full Charge Parry thành công
    /// </summary>
    public void TriggerFullChargeParrySlowMotion()
    {
        // 1. Kích hoạt Slow Motion
        if (slowMotionRoutine != null) StopCoroutine(slowMotionRoutine);
        slowMotionRoutine = StartCoroutine(DoSlowMotionRoutine());

        // 2. Kích hoạt Giật Camera nhẹ
        TriggerCameraShake();
    }

    /// <summary>
    /// Gọi riêng hiệu ứng Giật Camera (có thể tái sử dụng cho các đòn đánh khác nếu muốn)
    /// </summary>
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
            // Trả camera về vị trí cũ trước khi bắt đầu cú rung mới
            mainCamera.transform.localPosition = originalCameraPosition;
        }

        cameraShakeRoutine = StartCoroutine(DoCameraShakeRoutine());
    }

    private IEnumerator DoSlowMotionRoutine()
    {
        Time.timeScale = slowMotionTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Dùng WaitForSecondsRealtime để không bị hoãn đếm giờ do Time.timeScale bị hạ thấp
        yield return new WaitForSecondsRealtime(slowMotionDuration);

        ResetTimeScale();
    }

    private IEnumerator DoCameraShakeRoutine()
    {
        originalCameraPosition = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            // Tạo vị trí ngẫu nhiên xung quanh vị trí gốc của Camera
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            // Giữ nguyên trục Z để không bị lỗi đè/lệch khoảng cách nhìn 2D/3D
            randomOffset.z = 0f; 

            mainCamera.transform.localPosition = originalCameraPosition + randomOffset;

            // Dùng unscaledDeltaTime để độ rung không bị đờ ra khi game đang Slow Motion
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        // Trả Camera về đúng vị trí ban đầu
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