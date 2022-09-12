using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;

    private float activeMoveSpeed;
    private float dashSpeed;
    private float dashLength   = 0.5f;
    private float dashCooldown = 1f;
    private float dashCounter;
    private float dashCoolCounter;

    public JoyStickControl joyStickControl; 
    
    private Rigidbody2D rigid2d;
    private Vector3     movementVector;
    private Animator    anim;
    
    [Header("캐릭터")]
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private TrailRenderer  _trailRenderer;

 

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
            rigid2d.velocity = new Vector2(joyStickControl.joystickVec.x * moveSpeed,
                                           joyStickControl.joystickVec.y * moveSpeed);
            // 움직임 실행 코루틴
            StartCoroutine("HorizontalCheck");
            anim.SetBool("isMoving", true);
        }
        else // 초기화
        {
            rigid2d.velocity = Vector2.zero;
            
            anim.SetBool("isMoving", false);
        }
    }

    private IEnumerator HorizontalCheck()
    {
        // 조이스틱 x벡터값으로 좌우 확인
        if (joyStickControl.joystickVec.x < 0)
            _spriteRenderer.flipX = true;
        else
            _spriteRenderer.flipX = false;
        
        yield break;
    }
    
}
