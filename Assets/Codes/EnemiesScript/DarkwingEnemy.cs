using UnityEngine;

public class DarkwingEnemy : MonoBehaviour
{
    public float detectionRange = 5f;
    public float chargeSpeed = 10f;
    public float returnSpeed = 5f;
    public float jumpForce = 5f;
    public float hoverHeight = 2f;
    public float hoverSpeed = 1f;
    public GameObject startPositionObject; // New: GameObject to mark the starting position
    public Transform bodyToFlip; // New: The body part that should be flipped

    private Vector3 startPosition;
    private Transform player;
    private bool isCharging = false;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isFacingRight = true;

    // Animator parameter names
    private const string ANIM_IS_CHARGING = "IsCharging";
    private const string ANIM_IS_HOVERING = "IsHovering";

    void Start()
    {
        // Use the position of the startPositionObject if it's set, otherwise use current position
        startPosition = startPositionObject != null ? startPositionObject.transform.position : transform.position;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Ensure bodyToFlip is set
        if (bodyToFlip == null)
        {
            Debug.LogError("Body to flip is not set on " + gameObject.name);
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && !isCharging)
        {
            StartCharging();
        }
        else if (distanceToPlayer > detectionRange && isCharging)
        {
            ReturnToStart();
        }

        // Flip body to face player
        FlipTowardsPlayer();

        // Hover when not charging
        if (!isCharging)
        {
            Hover();
        }

        // Update animator
        UpdateAnimator();
    }

    void FlipTowardsPlayer()
    {
        if (bodyToFlip != null)
        {
            if ((player.position.x > transform.position.x && !isFacingRight) ||
                (player.position.x < transform.position.x && isFacingRight))
            {
                isFacingRight = !isFacingRight;
                bodyToFlip.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1);
            }
        }
    }

    void Hover()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void StartCharging()
    {
        isCharging = true;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = Vector2.zero; // Reset velocity before jumping
        rb.AddForce(new Vector2(direction.x * chargeSpeed, jumpForce), ForceMode2D.Impulse);
    }

    void ReturnToStart()
    {
        isCharging = false;
        rb.velocity = (startPosition - transform.position).normalized * returnSpeed;
    }

    void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetBool(ANIM_IS_CHARGING, isCharging);
            animator.SetBool(ANIM_IS_HOVERING, !isCharging);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (startPositionObject != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(startPositionObject.transform.position, 0.5f);
        }
    }
}