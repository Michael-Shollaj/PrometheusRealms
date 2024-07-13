using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed;
    public float chaseRange = 10f;
    public float attackRange = 2f; // Range within which the enemy will attack
    private GameObject player;
    public Transform startingPoint;
    public Animator anim;

    private bool isAttacking = false; // Track if the enemy is currently attacking

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // Check if within attack range
        if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking)
            {
                AttackPlayer();
            }
        }
        else if (distanceToPlayer <= chaseRange)
        {
            // Chase the player if within chase range but outside attack range
            ChasePlayer();
        }
        else
        {
            ReturnToStart();
        }

        FlipTowardsPlayer();
    }

    private void ChasePlayer()
    {
        isAttacking = false;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        anim.SetBool("IsChasing", true);
        anim.SetBool("Attack", false);
    }

    private void AttackPlayer()
    {
        isAttacking = true;
        anim.SetBool("Attack", true);
    }

    private void ReturnToStart()
    {
        isAttacking = false;
        transform.position = Vector2.MoveTowards(transform.position, startingPoint.position, speed * Time.deltaTime);
        anim.SetBool("IsChasing", false);
        anim.SetBool("Attack", false);
    }

    private void FlipTowardsPlayer()
    {
        if (transform.position.x > player.transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}