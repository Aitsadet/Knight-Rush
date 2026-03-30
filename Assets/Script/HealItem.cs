using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 20;
    public GameObject effectPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Swordman player = collision.GetComponent<Swordman>();

        if (player == null || player.isDead) return;

        // ❤️ Heal
        player.Heal(healAmount);

        // ✨ Effect
        if (effectPrefab != null)
        {
            GameObject fx = Instantiate(effectPrefab, transform.position, Quaternion.identity);

            ParticleSystem ps = fx.GetComponent<ParticleSystem>();
            if (ps != null)
                ps.Play();

            Destroy(fx, 1f);
        }

        Destroy(gameObject);
    }
}