using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public Sprite activeFlagSprite; // ลาก Sprite ธงที่กางแล้วมาใส่ใน Inspector

    private SpriteRenderer spriteRenderer;
    private bool isActivated = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ตรวจสอบว่าสิ่งที่มาชนมี Tag ว่า "Player" และ Checkpoint ยังไม่ถูกเปิดใช้งาน
        if (collision.CompareTag("Player") && !isActivated)
        {
            ActivateCheckpoint(collision.gameObject);
        }
    }

    private void ActivateCheckpoint(GameObject player)
    {
        isActivated = true;

        if (activeFlagSprite != null)
        {
            spriteRenderer.sprite = activeFlagSprite;
        }

        // โค้ดส่วนที่แก้: ดึง Script "Swordman" แทน PlayerController
        Swordman playerScript = player.GetComponent<Swordman>();
        if (playerScript != null)
        {
            playerScript.UpdateRespawnPosition(transform.position);
            Debug.Log("Checkpoint บันทึกแล้วที่ตำแหน่ง: " + transform.position);
        }
    }
}