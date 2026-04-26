using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void StartGame()
    {
        if (AnalyticsManager.Instance != null)
        {
            AnalyticsManager.Instance.SendGameStart(1);
        }
        else
        {
            Debug.LogWarning("❌ ไม่เจอ AnalyticsManager");
        }

        SceneManager.LoadScene("Level 1");
    }

    public void QuitGame()
    {
        Debug.Log("ออกจากเกมแล้ว");
        Application.Quit();
    }
}