using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;

    private bool isInitialized = false;
    private int eventCount = 0;

    private async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            await UnityServices.InitializeAsync();

            // ของเดิม ใช้ต่อได้ แค่เป็น Warning ไม่ใช่ Error
            AnalyticsService.Instance.StartDataCollection();

            isInitialized = true;

            Debug.Log("✅ Analytics Initialized");

            StartCoroutine(AutoFlush());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ส่ง Event game_start พร้อม amount = 1
    public void SendGameStart(int amount)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("❌ Analytics ยังไม่พร้อม");
            return;
        }

        CustomEvent gameStartEvent = new CustomEvent("game_start")
        {
            { "amount", amount }
        };

        AnalyticsService.Instance.RecordEvent(gameStartEvent);
        AnalyticsService.Instance.Flush();

        eventCount++;

        Debug.Log("🎮 Sent game_start amount = " + amount);
    }

    // ส่ง Event collect_potion พร้อม amount = 1
    public void SendCollectPotion(int amount)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("❌ Analytics ยังไม่พร้อม");
            return;
        }

        CustomEvent collectPotionEvent = new CustomEvent("collect_potion")
        {
            { "amount", amount }
        };

        AnalyticsService.Instance.RecordEvent(collectPotionEvent);
        AnalyticsService.Instance.Flush();

        eventCount++;

        Debug.Log("🧪 Sent collect_potion amount = " + amount);
    }

    // ส่ง Event ธรรมดา ไม่มี Parameter
    public void SendEvent(string eventName)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("❌ Analytics ยังไม่พร้อม");
            return;
        }

        CustomEvent customEvent = new CustomEvent(eventName);

        AnalyticsService.Instance.RecordEvent(customEvent);
        AnalyticsService.Instance.Flush();

        eventCount++;

        Debug.Log("📩 Sent Event : " + eventName);
    }

    private IEnumerator AutoFlush()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (isInitialized)
            {
                AnalyticsService.Instance.Flush();
                Debug.Log("🚀 Auto Flush");
            }
        }
    }
}