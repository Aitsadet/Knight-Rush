using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 1;
    public float knockbackForce = 5f;

    BoxCollider2D hitbox;

    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
        hitbox.enabled = false; // ปิดก่อน
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Attack());
        }
    }

    System.Collections.IEnumerator Attack()
    {
        hitbox.enabled = true;

        yield return new WaitForSeconds(0.2f);

        hitbox.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        EnemyController enemy = col.GetComponent<EnemyController>();

        if (enemy != null)
        {
            Vector2 knockback = (col.transform.position - transform.position).normalized * knockbackForce;
            enemy.TakeDamage(damage, knockback);
        }
    }
}