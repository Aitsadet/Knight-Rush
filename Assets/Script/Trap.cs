using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("ตั้งค่าความเสียหาย")]
    public int damageAmount = 20; // โดน 1 ที เลือดลดเท่าไหร่ ปรับได้ใน Inspector

    // ฟังก์ชันนี้จะทำงานทันทีที่มีอะไรมาชน/ทับกับดัก
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. เช็กว่าสิ่งที่มาชน มี Tag ชื่อ "Player" ใช่หรือไม่?
        if (collision.CompareTag("Player"))
        {
            // 2. ถ้าใช่ ให้ไปดึงสคริปต์ Swordman ที่อยู่บนตัวผู้เล่นมา
            Swordman playerScript = collision.GetComponent<Swordman>();

            // 3. เช็กให้ชัวร์ว่าผู้เล่นมีสคริปต์นี้จริงๆ
            if (playerScript != null)
            {
                // 4. สั่งเรียกฟังก์ชันลดเลือด (TakeDamage) ที่เราเขียนไว้!
                playerScript.TakeDamage(damageAmount, Vector2.zero);
                AnalyticsManager.Instance.SendEvent("damage_trap");
                // (แถม) แสดงข้อความเตือนใน Console ว่าโดนหนามแทง
                Debug.Log("โดนหนามทิ่ม! เลือดลด: " + damageAmount);
            }
        }
    }
}