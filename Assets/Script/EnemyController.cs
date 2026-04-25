using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Setting")]
    public float moveSpeed = 2f;
    public float chaseDistance = 6f;
    public float attackDistance = 2f;

    [Header("Attack Setting")]
    public float attackCooldown = 1.5f;
    float lastAttackTime;

    [Header("Damage")]
    public int damage = 20;

    [Header("Knockback")]
    public float knockbackForce = 6f;
    public float knockbackUp = 2f;

    [Header("Health")]
    public int maxHP = 3;
    int currentHP;

    [Header("HP Bar")]
    public Image hpFill;

    [Header("Hit Cooldown")]
    public float hitCooldown = 0.4f;
    float lastHitTime;

    [Header("Target")]
    public Transform player;

    Rigidbody2D rb;
    Animator anim;
    Collider2D col;

    bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        currentHP = maxHP;

        UpdateHPBar();

        // หา player อัตโนมัติ
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;
        if (isDead) return;

        float distance = Vector2.Distance(transform.position, player.position);

        float dir = Mathf.Sign(player.position.x - transform.position.x);

        Flip(dir);

        if (distance <= attackDistance)
        {
            Attack();
        }
        else if (distance <= chaseDistance)
        {
            Chase(dir);
        }
        else
        {
            Idle();
        }
    }

    void Idle()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        anim.SetBool("isWalk", false);
    }

    void Chase(float dir)
    {
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
        anim.SetBool("isWalk", true);
    }

    void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;

        rb.linearVelocity = Vector2.zero;

        anim.SetBool("isWalk", false);

        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        anim.SetTrigger("attack");

        yield return new WaitForSeconds(0.25f);

        DamagePlayer();
    }

    void DamagePlayer()
    {
        if (player == null) return;

        Swordman playerScript = player.GetComponent<Swordman>();

        if (playerScript != null)
        {
            float dir = Mathf.Sign(player.position.x - transform.position.x);

            Vector2 knockback =
                new Vector2(dir * knockbackForce, knockbackUp);

            playerScript.TakeDamage(damage, knockback);

            // ==========================================
            // 🌟 เพิ่มโค้ด Analytics: ส่งข้อมูลตอนผู้เล่นเสียเลือดให้ศัตรู
            // ==========================================
            AnalyticsManager.Instance.SendEvent("damage_enemy");
            // ==========================================
        }
    }

    void Flip(float dir)
    {
        Vector3 scale = transform.localScale;

        if (dir > 0)
            scale.x = Mathf.Abs(scale.x);
        else
            scale.x = -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    // =========================
    // TAKE DAMAGE
    // =========================

    public void TakeDamage(int damageAmount, Vector2 knockback)
    {
        if (isDead) return;

        // กันโดนตีรัว
        if (Time.time - lastHitTime < hitCooldown)
            return;

        lastHitTime = Time.time;

        currentHP -= damageAmount;

        UpdateHPBar();

        rb.AddForce(knockback, ForceMode2D.Impulse);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    // =========================
    // UPDATE HP BAR
    // =========================

    void UpdateHPBar()
    {
        if (hpFill != null)
        {
            hpFill.fillAmount = (float)currentHP / maxHP;
        }
    }

    // =========================
    // DIE
    // =========================

    void Die()
    {
        isDead = true;

        rb.linearVelocity = Vector2.zero;

        anim.SetBool("isDead", true);

        // ปิด collider → Player เดินทะลุได้
        col.enabled = false;

        // ปิด physics
        rb.bodyType = RigidbodyType2D.Kinematic;

        Destroy(gameObject, 3f);
    }
}