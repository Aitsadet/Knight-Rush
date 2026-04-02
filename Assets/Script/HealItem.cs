using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Swordman player = collision.GetComponent<Swordman>();
        if (player == null || player.isDead) return;

        // ❤️ Heal
        player.Heal(healAmount);

        // 📊 ยิง Analytics ผ่าน Manager
        AnalyticsManager.Instance.SendEvent("collect_potion");

        Debug.Log("เก็บ potion");

        Destroy(gameObject);
    }
}