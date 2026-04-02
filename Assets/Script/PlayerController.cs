using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsSit = false;
    public int currentJumpCount = 0;
    public bool isGrounded = false;
    public bool OnceJumpRayCheck = false;

    public bool Is_DownJump_GroundCheck = false;

    protected float m_MoveX;
    public Rigidbody2D m_rigidbody;
    protected CapsuleCollider2D m_CapsulleCollider;
    protected Animator m_Anim;

    [Header("[Setting]")]
    public float MoveSpeed = 6;
    public int JumpCount = 2;
    public float jumpForce = 15f;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_CapsulleCollider = GetComponent<CapsuleCollider2D>();
        m_Anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        AnimUpdate();
        GroundCheckUpdate();
    }

    void Move()
    {
        m_MoveX = Input.GetAxis("Horizontal");

        m_rigidbody.linearVelocity =
            new Vector2(m_MoveX * MoveSpeed, m_rigidbody.linearVelocity.y);

        if (m_MoveX != 0)
            Filp(m_MoveX < 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentJumpCount < JumpCount)
            {
                prefromJump();
            }
        }
    }

    protected void AnimUpdate()
    {
        if (m_Anim == null) return;

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
    }

    protected void Filp(bool bLeft)
    {
        transform.localScale = new Vector3(bLeft ? 1 : -1, 1, 1);
    }

    protected void prefromJump()
    {
        if (m_Anim != null)
            m_Anim.Play("Jump");

        m_rigidbody.linearVelocity = new Vector2(0, 0);

        m_rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        OnceJumpRayCheck = true;
        isGrounded = false;

        currentJumpCount++;
    }

    protected void DownJump()
    {
        if (!isGrounded)
            return;

        if (!Is_DownJump_GroundCheck)
        {
            if (m_Anim != null)
                m_Anim.Play("Jump");

            m_rigidbody.AddForce(-Vector2.up * 10);

            isGrounded = false;

            m_CapsulleCollider.enabled = false;

            StartCoroutine(GroundCapsulleColliderTimmerFuc());
        }
    }

    IEnumerator GroundCapsulleColliderTimmerFuc()
    {
        yield return new WaitForSeconds(0.3f);
        m_CapsulleCollider.enabled = true;
    }

    Vector2 RayDir = Vector2.down;

    float PretmpY;
    float GroundCheckUpdateTic = 0;
    float GroundCheckUpdateTime = 0.01f;

    protected void GroundCheckUpdate()
    {
        if (!OnceJumpRayCheck)
            return;

        GroundCheckUpdateTic += Time.deltaTime;

        if (GroundCheckUpdateTic > GroundCheckUpdateTime)
        {
            GroundCheckUpdateTic = 0;

            if (PretmpY == 0)
            {
                PretmpY = transform.position.y;
                return;
            }

            float reY = transform.position.y - PretmpY;

            if (reY <= 0)
            {
                if (isGrounded)
                {
                    LandingEvent();
                    OnceJumpRayCheck = false;
                }
                //else
                //{
                    //Debug.Log("ยังไม่ชนพื้น");
                //}
            }

            PretmpY = transform.position.y;
        }
    }

    // 🔴 ตรงนี้คือจุดสำคัญที่แก้
    protected virtual void LandingEvent()
    {
        currentJumpCount = 0;

        if (m_Anim != null)
            m_Anim.Play("Idle");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}