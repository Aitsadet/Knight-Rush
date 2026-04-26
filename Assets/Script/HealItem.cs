using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Swordman player = collision.GetComponent<Swordman>();

        if (player == null)
        {
            Debug.LogWarning("❌ เจอ Player แต่ไม่มี Swordman Script");
            return;
        }

        if (player.isDead)
        {
            Debug.LogWarning("❌ Player ตายแล้ว เก็บเลือดไม่ได้");
            return;
        }

        // เพิ่มเลือดจริงในระบบ Swordman
        player.Heal(healAmount);

        // ส่ง Analytics ตอนเก็บเลือด
        if (AnalyticsManager.Instance != null)
        {
            AnalyticsManager.Instance.SendCollectPotion(1);
        }
        else
        {
            Debug.LogWarning("❌ ไม่เจอ AnalyticsManager");
        }

        Debug.Log("Get potion + Heal +" + healAmount);

        Destroy(gameObject);
    }
}