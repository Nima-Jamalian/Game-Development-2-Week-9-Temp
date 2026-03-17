using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    [Header("Movement")]
    [SerializeField] float speed = 5f;
    private float horizontalInput;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 15f;
    private bool jumpRequested = false;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded = false;

    // Animation
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        Animate();
    }

    void FixedUpdate()
    {
        Move();
        HandleJump();
        CheckGround();
    }

    private void Move()
    {
        // Move horizontally
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);
    }

    private void CheckGround()
    {
        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void HandleJump()
    {
        if (jumpRequested && isGrounded)
        {
            jumpRequested = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void Animate()
    {
        // Idle and Run Animation
        animator.SetFloat("X", horizontalInput);

        // Flip Sprite (Left and Right Movement)
        if(horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        } else if(horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Always request a jump if space pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
