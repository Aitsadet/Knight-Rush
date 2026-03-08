using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // ==========================================
    // สำหรับหน้า You Win (กลับหน้าหลัก)
    // ==========================================
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    // ==========================================
    // สำหรับหน้า Main Menu (เริ่มเกม & ออกเกม)
    // ==========================================

    // ฟังก์ชันสำหรับปุ่ม PLAY
    public void StartGame()
    {
        Time.timeScale = 1f; // ทำให้แน่ใจว่าเวลาเดินปกติ
        SceneManager.LoadScene("Level 1"); // โหลดหน้า Level 1 (พิมพ์ชื่อให้ตรงเป๊ะ)
    }

    // ฟังก์ชันสำหรับปุ่ม QUIT
    public void QuitGame()
    {
        Debug.Log("กดปุ่มออกเกมแล้ว!"); // แสดงข้อความใน Console เพื่อให้รู้ว่าปุ่มทำงาน
        Application.Quit(); // คำสั่งปิดเกม (จะทำงานจริงๆ ตอนที่คุณ Build เกมเป็นไฟล์ .exe แล้วเท่านั้น)
    }
}