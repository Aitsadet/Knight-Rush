using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;

    private int eventCount = 0;
    private bool isInitialized = false;

    async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            await UnityServices.InitializeAsync();
            isInitialized = true;

            Debug.Log("✅ Analytics Initialized");

            StartCoroutine(AutoFlush());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SendEvent(string eventName)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("❌ Analytics ยังไม่พร้อม");
            return;
        }

        // 📊 ยิง event
        AnalyticsService.Instance.RecordEvent(eventName);
        eventCount++;

        Debug.Log("📤 Sent Event: " + eventName);

        // 🚀 Flush ทุก 3 ครั้ง
        if (eventCount % 3 == 0)
        {
            AnalyticsService.Instance.Flush();
            Debug.Log("🚀 Flush (count)");
        }
    }

    IEnumerator AutoFlush()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (isInitialized)
            {
                AnalyticsService.Instance.Flush();
                Debug.Log("⏱️ Auto Flush");
            }
        }
    }
}