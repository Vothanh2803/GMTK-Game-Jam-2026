using UnityEngine;
using UnityEngine.EventSystems;

// Sử dụng IPointerDownHandler và IPointerUpHandler để bắt sự kiện Chạm / Thả
public class ParryBtnUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private PlayerParry playerParry;

    private void Start()
    {
        // Tự động tìm Player Controller để lấy PlayerParry
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            playerParry = player.GetComponent<PlayerParry>();
        }
    }

    // Khi người chơi BẮT ĐẦU BẤM GIỮ nút UI
    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerParry != null)
        {
            playerParry.StartCharging();
        }
    }

    // Khi người chơi THẢ TAY khỏi nút UI
    public void OnPointerUp(PointerEventData eventData)
    {
        if (playerParry != null)
        {
            playerParry.ReleaseParry();
        }
    }
}