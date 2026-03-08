using UnityEngine;
using UnityEngine.SceneManagement; // จำเป็นสำหรับการโหลด Scene

public class NextLevel : MonoBehaviour
{
    [Header("Level Settings")]
    public string nextSceneName = "Level 2"; // ชื่อด่านที่ต้องการให้โหลด

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ตรวจสอบว่าคนที่เดินมาชนมี Tag เป็น "Player"
        if (collision.CompareTag("Player"))
        {
            Debug.Log("กำลังเปลี่ยนด่านไปที่: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName); // สั่งโหลดด่านใหม่
        }
    }
}