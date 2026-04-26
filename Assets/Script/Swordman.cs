using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Swordman : PlayerController
{
    private Vector2 respawnPosition; // ตัวแปรเก็บตำแหน่งจุดเกิด

    [Header("Health System")]
    public int maxHP = 100;
    public int currentHP;
    public bool isDead = false;

    [Header("UI")]
    public Image hpBarFill;
    public GameObject gameOverPanel;
    public GameObject controlUI;

    [Header("Attack")]
    public float attackCooldown = 0.5f;
    float lastAttackTime;

    public int attackDamage = 20;
    public float attackRange = 1.2f;
    public float attackHitCooldown = 0.4f;
    float lastHitTime;

    [Header("Knockback")]
    public float knockbackDuration = 0.25f;

    bool isKnockback = false;

    void Start()
    {
        // บันทึกจุดเกิดเริ่มต้นตอนเพิ่งเปิดฉาก
        respawnPosition = transform.position;

        m_CapsulleCollider = GetComponent<CapsuleCollider2D>();
        m_rigidbody = GetComponent<Rigidbody2D>();

        currentHP = maxHP;
        UpdateHPBar();

        Transform model = transform.Find("model");
        if (model != null)
        {
            m_Anim = model.GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (isDead) return;

        checkInput();
    }

    public void checkInput()
    {
        if (m_Anim == null) return;

        m_MoveX = Input.GetAxis("Horizontal");

        GroundCheckUpdate();

        // ==========================================
        // Analytics: นับจำนวนการคลิกเมาส์
        // click_left / click_right
        // ==========================================
        if (Input.GetMouseButtonDown(0)) // คลิกซ้าย
        {
            if (AnalyticsManager.Instance != null)
            {
                AnalyticsManager.Instance.SendClickLeft();
            }
            else
            {
                Debug.LogWarning("❌ ไม่เจอ AnalyticsManager ตอนกดคลิกซ้าย");
            }
        }

        if (Input.GetMouseButtonDown(1)) // คลิกขวา
        {
            if (AnalyticsManager.Instance != null)
            {
                AnalyticsManager.Instance.SendClickRight();
            }
            else
            {
                Debug.LogWarning("❌ ไม่เจอ AnalyticsManager ตอนกดคลิกขวา");
            }
        }

        // นั่ง
        if (Input.GetKeyDown(KeyCode.S))
        {
            IsSit = true;
            m_Anim.Play("Sit");
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            m_Anim.Play("Idle");
            IsSit = false;
        }

        // โจมตี + cooldown
        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    lastAttackTime = Time.time;
                    m_Anim.Play("Attack");
                }
            }
            else
            {
                if (Mathf.Abs(m_MoveX) > 0.1f)
                {
                    m_Anim.Play("Run");
                }
                else
                {
                    m_Anim.Play("Idle");
                }
            }
        }

        // กระโดด
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;

            if (currentJumpCount < JumpCount)
            {
                if (!IsSit)
                {
                    prefromJump();
                }
                else
                {
                    DownJump();
                }
            }
        }

        MoveCharacter();
    }

    void MoveCharacter()
    {
        if (isKnockback) return;

        float move = m_MoveX * MoveSpeed;

        m_rigidbody.linearVelocity =
            new Vector2(move, m_rigidbody.linearVelocity.y);

        if (m_MoveX > 0)
        {
            Filp(false);
        }
        else if (m_MoveX < 0)
        {
            Filp(true);
        }
    }

    protected override void LandingEvent()
    {
        base.LandingEvent();

        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Run") &&
            !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            !isDead)
        {
            m_Anim.Play("Idle");
        }
    }

    // =====================
    // ATTACK HITBOX
    // =====================

    public void HitEnemy()
    {
        if (Time.time - lastHitTime < attackHitCooldown)
            return;

        lastHitTime = Time.time;

        Collider2D[] enemies =
            Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D col in enemies)
        {
            if (col.CompareTag("Enemy"))
            {
                EnemyController enemy = col.GetComponent<EnemyController>();

                if (enemy != null)
                {
                    float dir =
                        Mathf.Sign(col.transform.position.x - transform.position.x);

                    Vector2 knockback =
                        new Vector2(dir * 4f, 2f);

                    enemy.TakeDamage(attackDamage, knockback);
                }
            }
        }
    }

    // =====================
    // HEAL SYSTEM
    // =====================

    public void Heal(int amount)
    {
        if (isDead) return;

        int beforeHP = currentHP;

        currentHP += amount;

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        UpdateHPBar();

        Debug.Log("❤️ Heal +" + amount + " | HP: " + beforeHP + " → " + currentHP + "/" + maxHP);
    }

    // =====================
    // DAMAGE SYSTEM
    // =====================

    public void TakeDamage(int damageAmount, Vector2 knockback)
    {
        if (isDead) return;

        int beforeHP = currentHP;

        currentHP -= damageAmount;

        if (currentHP < 0)
        {
            currentHP = 0;
        }

        Debug.Log("💔 Player HP: " + beforeHP + " → " + currentHP);

        UpdateHPBar();

        StartCoroutine(Knockback(knockback));

        if (currentHP <= 0)
        {
            Die();
        }
    }

    IEnumerator Knockback(Vector2 force)
    {
        isKnockback = true;

        m_rigidbody.linearVelocity = Vector2.zero;

        m_rigidbody.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        isKnockback = false;
    }

    void UpdateHPBar()
    {
        if (hpBarFill != null)
        {
            hpBarFill.fillAmount = (float)currentHP / maxHP;
        }
        else
        {
            Debug.LogWarning("❌ ยังไม่ได้ลาก hpBarFill ใส่ใน Inspector");
        }
    }

    // =====================
    // PLAYER DIE & RESPAWN
    // =====================

    void Die()
    {
        isDead = true;

        m_rigidbody.linearVelocity = Vector2.zero;

        if (m_Anim != null)
        {
            m_Anim.SetTrigger("die");
        }

        if (controlUI != null)
        {
            controlUI.SetActive(false);
        }

        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(1.2f);

        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // ฟังก์ชันสำหรับอัปเดตจุดเกิดเมื่อเดินผ่าน Checkpoint
    public void UpdateRespawnPosition(Vector2 newPosition)
    {
        respawnPosition = newPosition;
    }

    // ฟังก์ชันนี้ไว้ใช้ผูกกับปุ่ม Retry ในหน้า Game Over
    public void RespawnPlayer()
    {
        Time.timeScale = 1f;
        isDead = false;

        currentHP = maxHP;
        UpdateHPBar();

        transform.position = respawnPosition;
        m_rigidbody.linearVelocity = Vector2.zero;
        isKnockback = false;

        if (m_Anim != null)
        {
            m_Anim.Play("Idle");
        }

        if (controlUI != null)
        {
            controlUI.SetActive(true);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    // =====================
    // DEBUG HIT RANGE
    // =====================

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}