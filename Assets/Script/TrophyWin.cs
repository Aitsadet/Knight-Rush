using UnityEngine;

public class TrophyWin : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject youWinPanel; // ลากหน้าจอ You Win มาใส่ตรงนี้

    [Header("Game Rules")]
    [Tooltip("ถ้าติ๊กถูก ผู้เล่นต้องฆ่ามอนสเตอร์ให้หมดฉากก่อน ถ้วยถึงจะทำงาน")]
    public bool mustKillAllEnemies = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // เช็คว่าคนที่มาแตะคือ Player
        if (collision.CompareTag("Player"))
        {
            if (mustKillAllEnemies)
            {
                // ค้นหาว่ามี Object ไหนในด่านที่มี Tag "Enemy" เหลืออยู่บ้าง
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

                if (enemies.Length > 0)
                {
                    Debug.Log("ยังกำจัดมอนสเตอร์ไม่หมด! เหลืออีก: " + enemies.Length + " ตัว");
                    // อาจจะใส่โค้ดเด้งข้อความแจ้งเตือนผู้เล่นบนจอตรงนี้ได้
                    return; // ยกเลิกการทำงาน ถ้วยยังไม่เก็บ
                }
            }

            // ถ้าไม่มีมอนสเตอร์เหลือแล้ว (หรือไม่ได้ติ๊กบังคับไว้)
            TriggerWin();
        }
    }

    private void TriggerWin()
    {
        Debug.Log("ชนะแล้ว! You Win!");

        if (youWinPanel != null)
        {
            youWinPanel.SetActive(true); // เปิดหน้าจอ You Win
        }

        Time.timeScale = 0f; // หยุดเวลาในเกม
    }
}