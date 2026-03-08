using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // **อย่าลืมบรรทัดนี้ ต้องมีเพื่อใช้งาน UI**

public class Swordman : PlayerController
{
    [Header("Health System")]
    public int maxHP = 100;
    public int currentHP;
    public bool isDead = false;

    [Header("UI System")]
    public Image hpBarFill; // ตัวแปรรับรูปหลอดเลือดสีแดง/เขียว ที่เราสร้างไว้

    private void Start()
    {
        m_CapsulleCollider = GetComponent<CapsuleCollider2D>();
        m_rigidbody = GetComponent<Rigidbody2D>();

        // ตั้งค่าเลือดเริ่มต้นให้เต็ม
        currentHP = maxHP;
        UpdateHPBar(); // อัปเดตหลอดเลือดตอนเริ่มเกม

        // ป้องกัน error ถ้าไม่มี model
        Transform model = transform.Find("model");
        if (model != null)
            m_Anim = model.GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDead) return;

        checkInput();

        // จำกัดความเร็ว
        if (m_rigidbody.linearVelocity.magnitude > 30)
        {
            m_rigidbody.linearVelocity =
                new Vector2(
                    m_rigidbody.linearVelocity.x - 0.1f,
                    m_rigidbody.linearVelocity.y - 0.1f
                );
        }
    }

    public void checkInput()
    {
        if (m_Anim == null) return;

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

        if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Sit") ||
            m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currentJumpCount < JumpCount)
                    DownJump();
            }
            return;
        }

        m_MoveX = Input.GetAxis("Horizontal");

        GroundCheckUpdate();

        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                m_Anim.Play("Attack");
            }
            else
            {
                if (m_MoveX == 0)
                {
                    if (!OnceJumpRayCheck)
                        m_Anim.Play("Idle");
                }
                else
                {
                    m_Anim.Play("Run");
                }
            }
        }

        // กดปุ่ม 1 เพื่อจำลองการโดนโจมตี (ลดเลือดทีละ 20)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(20);
        }

        if (Input.GetKey(KeyCode.D))
        {
            MoveCharacter();

            if (!Input.GetKey(KeyCode.A))
                Filp(false);
        }

        else if (Input.GetKey(KeyCode.A))
        {
            MoveCharacter();

            if (!Input.GetKey(KeyCode.D))
                Filp(true);
        }

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
    }

    void MoveCharacter()
    {
        if (isGrounded)
        {
            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;

            transform.Translate(Vector2.right * m_MoveX * MoveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0));
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

    // ==========================================
    // ฟังก์ชันจัดการ HP และความเสียหาย
    // ==========================================

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHP -= damageAmount;
        Debug.Log("Player โดนโจมตี! เลือดเหลือ: " + currentHP);

        // อัปเดตหลอดเลือดบนหน้าจอ
        UpdateHPBar();

        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
    }

    // ฟังก์ชันคำนวณและลดหลอดเลือด UI
    private void UpdateHPBar()
    {
        if (hpBarFill != null)
        {
            // สูตรคำนวณเปอร์เซ็นต์เลือด (0.0 ถึง 1.0)
            hpBarFill.fillAmount = (float)currentHP / maxHP;
        }
    }

    private void Die()
    {
        isDead = true;
        m_Anim.Play("Die");
        m_rigidbody.linearVelocity = Vector2.zero;
        Debug.Log("Player ตายแล้ว!");
    }
}