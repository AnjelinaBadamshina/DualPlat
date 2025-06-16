using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class VerticalJumpAbility2D : MonoBehaviour, IAbility
{
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public int maxJumps = 2;
    public GameObject wingsObject;

    private Rigidbody2D rb;
    private int jumpCount = 0;
    private bool wasGroundedLastFrame = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (groundCheck == null)
            groundCheck = transform.Find("GroundCheck");

        if (wingsObject != null)
            wingsObject.SetActive(false);
    }

    private void Update()
    {
        bool groundedNow = IsGrounded();

        // Отключаем крылья только если приземлились (переход "в воздух → на землю")
        if (groundedNow && !wasGroundedLastFrame)
        {
            jumpCount = 0;

            if (wingsObject != null)
                wingsObject.SetActive(false);
        }

        wasGroundedLastFrame = groundedNow;
    }

    public void Activate()
    {
        if (jumpCount >= maxJumps)
        {
            Debug.Log("Jump limit reached.");
            return;
        }

        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpCount++;

        // Активируем крылья при любом прыжке по способности
        if (wingsObject != null)
            wingsObject.SetActive(true);

        Debug.Log($"Jump #{jumpCount} activated by ability!");
    }

    private bool IsGrounded()
    {
        if (groundCheck == null)
        {
            Debug.LogWarning("GroundCheck not assigned.");
            return false;
        }

        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
#endif
}
