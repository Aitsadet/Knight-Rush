using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;

    private bool isInitialized = false;

    private async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            await UnityServices.InitializeAsync();

            // อันนี้อาจขึ้น Warning สีเหลือง แต่ยังใช้ได้ ไม่ใช่ Error
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

    // =========================
    // GAME START
    // Event Name: game_start
    // Parameter: level
    // Type: STRING
    // =========================
    public void SendGameStart(string level)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("❌ Analytics ยังไม่พร้อม");
            return;
        }

        CustomEvent gameStartEvent = new CustomEvent("game_start")
        {
            { "level", level }
        };

        AnalyticsService.Instance.RecordEvent(gameStartEvent);
        AnalyticsService.Instance.Flush();

        Debug.Log("🎮 Sent game_start level = " + level);
    }

    // =========================
    // COLLECT POTION
    // Event Name: collect_potion
    // Parameter: amount
    // Type: INTEGER
    // =========================
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

        Debug.Log("🧪 Sent collect_potion amount = " + amount);
    }

    // =========================
    // CLICK LEFT
    // Event Name: click_left
    // ไม่มี Parameter
    // =========================
    public void SendClickLeft()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("❌ Analytics ยังไม่พร้อม");
            return;
        }

        CustomEvent clickLeftEvent = new CustomEvent("click_left");

        AnalyticsService.Instance.RecordEvent(clickLeftEvent);
        AnalyticsService.Instance.Flush();

        Debug.Log("🖱️ Sent click_left");
    }

    // =========================
    // CLICK RIGHT
    // Event Name: click_right
    // ไม่มี Parameter
    // =========================
    public void SendClickRight()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("❌ Analytics ยังไม่พร้อม");
            return;
        }

        CustomEvent clickRightEvent = new CustomEvent("click_right");

        AnalyticsService.Instance.RecordEvent(clickRightEvent);
        AnalyticsService.Instance.Flush();

        Debug.Log("🖱️ Sent click_right");
    }

    // =========================
    // DAMAGE TRAP
    // Event Name: damage_trap
    // Parameter: damage
    // Type: INTEGER
    // =========================
    public void SendDamageTrap(int damage)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("❌ Analytics ยังไม่พร้อม");
            return;
        }

        CustomEvent damageTrapEvent = new CustomEvent("damage_trap")
        {
            { "damage", damage }
        };

        AnalyticsService.Instance.RecordEvent(damageTrapEvent);
        AnalyticsService.Instance.Flush();

        Debug.Log("💥 Sent damage_trap damage = " + damage);
    }

    // =========================
    // ส่ง Event ธรรมดา ไม่มี Parameter
    // ใช้เฉพาะ Event ที่สร้างใน Dashboard แล้ว
    // =========================
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