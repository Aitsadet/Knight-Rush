using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        if (AnalyticsManager.Instance != null)
        {
            AnalyticsManager.Instance.SendGameStart("Level 1");
            Debug.Log("🎮 กดปุ่ม Play ส่ง game_start level = Level 1");
        }
        else
        {
            Debug.LogWarning("❌ ไม่เจอ AnalyticsManager");
        }

        // รอให้ Analytics ส่งก่อนเปลี่ยนฉาก
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Level 1");
    }

    public void QuitGame()
    {
        Debug.Log("ออกจากเกมแล้ว");
        Application.Quit();
    }
}