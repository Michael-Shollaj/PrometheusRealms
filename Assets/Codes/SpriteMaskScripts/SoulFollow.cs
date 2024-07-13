using UnityEngine;
using System.Collections;

public class SoulFollow : MonoBehaviour
{
    public Transform player;
    public float orbitSpeed = 180f;
    public float orbitRadius = 1f;
    public float bobAmplitude = 0.2f;
    public float bobFrequency = 2f;
    private float orbitAngle = 0f;
    private Vector3 orbitCenter;

    private FollowCursor followCursor;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3f;

    // Attack variables
    public float detectionRange = 5f;
    public float attackRange = 2f;
    public float attackCooldown = 0.1f;
    public int attackDamage = 1;
    public LayerMask enemyLayer;
    private float lastAttackTime;

    // Visual feedback
    public GameObject attackEffectPrefab;
    private Animator animator;

    // Enemy targeting
    private Transform targetEnemy;
    public float enemyOrbitSpeed = 5f;
    public float enemyOrbitRadius = 1.5f;

    // Projectile variables
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    // New variables for limiting distance and prioritizing closer enemies
    public float maxDistanceFromPlayer = 10f;
    public float returnToPlayerThreshold = 8f;

    void Start()
    {
        followCursor = GetComponent<FollowCursor>();
        lastAttackTime = -attackCooldown;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!followCursor.IsFollowingMouse())
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer > returnToPlayerThreshold)
            {
                targetEnemy = null;
                ReturnToPlayer();
            }
            else if (targetEnemy == null)
            {
                OrbitPlayer();
                FindNearestEnemyCloseToPlayer();
            }
            else
            {
                OrbitAndAttackEnemy();
            }
        }
    }

    void OrbitPlayer()
    {
        orbitCenter = player.position;
        orbitAngle += orbitSpeed * Time.deltaTime;

        Vector3 orbitPosition = orbitCenter + Quaternion.Euler(0, 0, orbitAngle) * Vector3.right * orbitRadius;

        float bobOffset = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        orbitPosition += Vector3.up * bobOffset;

        transform.position = Vector3.SmoothDamp(transform.position, orbitPosition, ref velocity, smoothTime);

        Vector3 lookDirection = (orbitPosition - transform.position).normalized;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void FindNearestEnemyCloseToPlayer()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(player.position, detectionRange, enemyLayer);
        float closestDistance = float.MaxValue;

        foreach (Collider2D enemyCollider in enemiesInRange)
        {
            float distanceToPlayer = Vector2.Distance(player.position, enemyCollider.transform.position);
            if (distanceToPlayer < closestDistance && distanceToPlayer <= maxDistanceFromPlayer)
            {
                closestDistance = distanceToPlayer;
                targetEnemy = enemyCollider.transform;
            }
        }
    }

    void OrbitAndAttackEnemy()
    {
        if (targetEnemy != null)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, targetEnemy.position);
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToEnemy > detectionRange || distanceToPlayer > maxDistanceFromPlayer)
            {
                targetEnemy = null;
                return;
            }

            Vector3 orbitPosition = targetEnemy.position + (transform.position - targetEnemy.position).normalized * enemyOrbitRadius;
            transform.position = Vector3.Slerp(transform.position, orbitPosition, enemyOrbitSpeed * Time.deltaTime);

            Vector3 directionToEnemy = (targetEnemy.position - transform.position).normalized;
            float angle = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            TryAttack();
        }
        else
        {
            FindNearestEnemyCloseToPlayer();
        }
    }

    void ReturnToPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, player.position, enemyOrbitSpeed * Time.deltaTime);

        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
        {
            return;
        }

        FireProjectile();
        lastAttackTime = Time.time;
    }

    void FireProjectile()
    {
        if (projectilePrefab != null && targetEnemy != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Vector2 direction = (targetEnemy.position - transform.position).normalized;
            projectile.transform.right = direction;

            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.damage = attackDamage;
                projectileScript.targetLayer = enemyLayer;
            }

            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            if (attackEffectPrefab != null)
            {
                Instantiate(attackEffectPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(player.position, maxDistanceFromPlayer);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(player.position, returnToPlayerThreshold);
    }
}