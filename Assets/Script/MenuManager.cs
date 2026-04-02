using Unity.Services.Analytics;
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
        AnalyticsManager.Instance.SendEvent("game_start");

        SceneManager.LoadScene("Level 1");
    }

    // ฟังก์ชันสำหรับปุ่ม QUIT
    public void QuitGame()
    {
        Debug.Log("กดปุ่มออกเกมแล้ว!"); // แสดงข้อความใน Console เพื่อให้รู้ว่าปุ่มทำงาน
        Application.Quit(); // คำสั่งปิดเกม (จะทำงานจริงๆ ตอนที่คุณ Build เกมเป็นไฟล์ .exe แล้วเท่านั้น)
    }
}