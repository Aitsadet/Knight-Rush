using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Swordman : PlayerController
{
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
        m_CapsulleCollider = GetComponent<CapsuleCollider2D>();
        m_rigidbody = GetComponent<Rigidbody2D>();

        currentHP = maxHP;
        UpdateHPBar();

        Transform model = transform.Find("model");
        if (model != null)
            m_Anim = model.GetComponent<Animator>();
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
                    m_Anim.Play("Run");
                else
                    m_Anim.Play("Idle");
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
                    prefromJump();
                else
                    DownJump();
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
            Filp(false);
        else if (m_MoveX < 0)
            Filp(true);
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
                EnemyController enemy =
                    col.GetComponent<EnemyController>();

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
    // DAMAGE SYSTEM
    // =====================

    public void TakeDamage(int damageAmount, Vector2 knockback)
    {
        if (isDead) return;

        currentHP -= damageAmount;

        Debug.Log("Player HP: " + currentHP);

        UpdateHPBar();

        StartCoroutine(Knockback(knockback));

        if (currentHP <= 0)
        {
            currentHP = 0;
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
    }

    // =====================
    // PLAYER DIE
    // =====================

    void Die()
    {
        isDead = true;

        m_rigidbody.linearVelocity = Vector2.zero;

        if (m_Anim != null)
            m_Anim.SetTrigger("die");

        if (controlUI != null)
            controlUI.SetActive(false);

        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(1.2f);

        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
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