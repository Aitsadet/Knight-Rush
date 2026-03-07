using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordman : PlayerController
{
    private void Start()
    {
        m_CapsulleCollider = GetComponent<CapsuleCollider2D>();
        m_rigidbody = GetComponent<Rigidbody2D>();

        // ป้องกัน error ถ้าไม่มี model
        Transform model = transform.Find("model");
        if (model != null)
            m_Anim = model.GetComponent<Animator>();
    }

    private void Update()
    {
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

        if (Input.GetKey(KeyCode.Alpha1))
            m_Anim.Play("Die");

        // เดินขวา
        if (Input.GetKey(KeyCode.D))
        {
            MoveCharacter();

            if (!Input.GetKey(KeyCode.A))
                Filp(false);
        }

        // เดินซ้าย
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
            !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            m_Anim.Play("Idle");
        }
    }
}