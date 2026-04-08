using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private int facingDirection = 1;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [Header("Crouch Settings")]
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Inputs")]
    [SerializeField] private float moveInput;

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        Jump();

        if(moveInput < 0 && transform.localScale.x > 0 || moveInput > 0 && transform.localScale.x < 0)
        {
            Flip();
        }

        CrouchDown();

        CrouchUp();

        HandleAnimations();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }

    void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void Flip()
    {
        facingDirection *= 1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y,transform.localScale.z);
    }

    void CrouchDown()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("isCrouching", true);
            boxCollider.enabled = false;
        }
    }

    void CrouchUp()
    {
        if(Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("isCrouching", false);
            boxCollider.enabled = true;
        }
    }

    void HandleAnimations()
    {
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("isJumping",rb.linearVelocity.y > 0.1);
    }
}