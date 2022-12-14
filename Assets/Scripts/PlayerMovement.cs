using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 2f;
    private float activeMoveSpeed;

    public  bool  canDash;
    private float dashSpeed    = 8f;
    private float dashLength   = 0.1f;
    private float dashCooltime = 2f;
    
    public JoyStickControl joyStickControl; 
    
    private Rigidbody2D rigid2d;
    private Vector3     movementVector;
    private Animator    anim;

    [Header("캐릭터")]
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Abilities _abilities;
    
    private void Awake()
    {
        activeMoveSpeed = moveSpeed;

        rigid2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        movementVector = new Vector3();
    }

    private void FixedUpdate()
    {
        // 조이스틱 조작
        if (joyStickControl.joystickVec.y != 0)
        {
            rigid2d.velocity = new Vector2(joyStickControl.joystickVec.x * activeMoveSpeed,
                                           joyStickControl.joystickVec.y * activeMoveSpeed);
  
            _spriteRenderer.flipX = joyStickControl.joystickVec.x < 0;
            anim.SetBool("isMoving", true);
        }
        else // 초기화
        {
            rigid2d.velocity = Vector2.zero;
            
            anim.SetBool("isMoving", false);
        }
    }

    public void Dash()
    {
        if (!canDash)
            StartCoroutine(DashActivate());
    }
    
    private IEnumerator DashActivate()
    {
        // 이동속도 대쉬 속도
        activeMoveSpeed = dashSpeed;

        // 대쉬 애니메이션 실행 후 쿨타임 실행
        anim.SetBool("isSliding", true);
        canDash = false;
        yield return new WaitForSeconds(dashLength);
        
        // 대쉬 애니메이션 종료 후 일반 속도
        anim.SetBool("isSliding", false);
        activeMoveSpeed = moveSpeed;

        // 대쉬 쿨타임 실행
        yield return new WaitForSeconds(dashCooltime);
        
        // 대쉬 초기화
        canDash= true;
    }
}


