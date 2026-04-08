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

    [Header("Climbing Settings")]
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private float gravityScaleAtStart;

    [Header("Inputs")]
    [SerializeField] private float moveInputX;
    [SerializeField] private float moveInputY;

    private void Update()
    {
        moveInputX = Input.GetAxisRaw("Horizontal");
        moveInputY = Input.GetAxisRaw("Vertical");

        Jump();

        if(moveInputX < 0 && transform.localScale.x > 0 || moveInputX > 0 && transform.localScale.x < 0)
        {
            Flip();
        }

        CrouchDown();

        CrouchUp();

        ClimbLadder();

        HandleAnimations();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(moveInputX * speed, rb.linearVelocity.y);
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
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetBool("isCrouching", true);
            boxCollider.enabled = false;
        }
    }

    void CrouchUp()
    {
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("isCrouching", false);
            boxCollider.enabled = true;
        }
    }

    void ClimbLadder()
    {
        if(!rb.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            rb.gravityScale = gravityScaleAtStart;
            animator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(rb.linearVelocity.x, moveInputY * climbSpeed);
        rb.linearVelocity = climbVelocity;
        rb.gravityScale = 0;

        bool playerHasVerticalSpeed = Mathf.Abs(rb.linearVelocity.y) > Mathf.Epsilon && !isGrounded;
        animator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void HandleAnimations()
    {
        animator.SetFloat("Speed", Mathf.Abs(moveInputX));
        animator.SetBool("isJumping",rb.linearVelocity.y > 0.1);
    }
}