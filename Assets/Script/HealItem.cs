using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerController player = collision.GetComponent<PlayerController>();

        if (player == null) return;

        // ส่ง Analytics: collect_potion พร้อม amount = 1
        if (AnalyticsManager.Instance != null)
        {
            AnalyticsManager.Instance.SendCollectPotion(1);
        }
        else
        {
            Debug.LogWarning("❌ ไม่เจอ AnalyticsManager");
        }

        Debug.Log("Get potion");

        Destroy(gameObject);
    }
}