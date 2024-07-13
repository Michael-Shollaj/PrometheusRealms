using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public LayerMask targetLayer;
    public float lifetime = 2f;
    public float speed = 10f;
    public float rotationSpeed = 200f;
    public float homingStrength = 5f;

    private Transform target;
    private Vector3 lastKnownPosition;

    private void Start()
    {
        Destroy(gameObject, lifetime);
        FindTarget();
    }

    private void Update()
    {
        if (target != null)
        {
            lastKnownPosition = target.position;
        }

        Vector3 direction = (lastKnownPosition - transform.position).normalized;
        Vector3 newDirection = Vector3.RotateTowards(transform.right, direction, rotationSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);
        transform.right = newDirection;

        float distanceToTarget = Vector3.Distance(transform.position, lastKnownPosition);
        float currentSpeed = speed + (homingStrength / distanceToTarget);
        transform.position += transform.right * currentSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    private void FindTarget()
    {
        Collider2D[] potentialTargets = Physics2D.OverlapCircleAll(transform.position, 10f, targetLayer);
        float closestDistance = float.MaxValue;
        foreach (Collider2D potentialTarget in potentialTargets)
        {
            float distance = Vector2.Distance(transform.position, potentialTarget.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = potentialTarget.transform;
                lastKnownPosition = target.position;
            }
        }
    }
}