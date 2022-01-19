using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed = 7f, jumpForce = 20f;

    private Rigidbody2D myBody;

    private Transform groundCheckPos;

    [SerializeField]
    private LayerMask groundLayer;

    private bool canDoubleJump;

    private PlayerAnimationsWithTransitions playerAnim;

    [SerializeField]
    private float attackWaitTime = 0.5f;

    private float attackTimer;
    private bool canAttack;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();

        groundCheckPos = transform.GetChild(0).transform;

        playerAnim = GetComponent<PlayerAnimationsWithTransitions>();

    }

    private void Update()
    {
        PlayerJump();
        AnimatePlayer();
        GetAttackInput();
        HandleAttackAction();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, groundLayer);
    }

    void PlayerJump()
    {
        
        if (Input.GetKeyDown(KeyCode.W) || Input.GetButtonDown(TagManager.JUMP_BUTTON))
        {

            if (!IsGrounded() && canDoubleJump)
            {
                canDoubleJump = false;

                myBody.velocity = Vector2.zero;
                myBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

                playerAnim.PlayDoubleJump();

            }

            if (IsGrounded())
            {
                canDoubleJump = true;
                myBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }

        }

    }

    void AnimatePlayer()
    {
        playerAnim.PlayJump(myBody.velocity.y);
        playerAnim.PlayFromJumpToRunning(IsGrounded());
    }

    void GetAttackInput()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            if (Time.time > attackTimer)
            {
                attackTimer = Time.time + attackWaitTime;
                canAttack = true;
                SoundManager.instance.Play_PlayerAttack_Sound();
            }
        }
    }

    void HandleAttackAction()
    {
        if (canAttack && IsGrounded())
        {
            canAttack = false;
            playerAnim.PlayAttack();
        }
        else if (canAttack && !IsGrounded())
        {
            canAttack = false;
            playerAnim.PlayJumpAttack();
        }
    }

} // class































