using System.Collections;
using UnityEngine;

public class EnemyAttack : Enemies
{
    public int _moveSpeed;
    public int attackDamage;
    public int _lifePoints;
    public float _attackRadius;
    public GameObject Effect;
    public Vector3 attackOffset2;
    public float attackRange2 = 1f;
    public LayerMask attackMask2;
    public float _followRadius;

    Transform playerTransform;
    Animator enemyAnim;
    Flip flipScript;

    void Start()
    {
        playerTransform = FindObjectOfType<Health>().GetComponent<Transform>();
        enemyAnim = gameObject.GetComponent<Animator>();
        flipScript = GetComponent<Flip>();
        flipScript.player = playerTransform;

        setMoveSpeed(_moveSpeed);
        setAttackDamage(attackDamage);
        setLifePoints(_lifePoints);
        setAttackRadius(_attackRadius);
        setFollowRadius(_followRadius);
    }

    void Update()
    {
        if (checkFollowRadius(playerTransform.position.x, transform.position.x))
        {
            flipScript.LookAtPlayer(); // Call LookAtPlayer to ensure the enemy is facing the player

            if (checkAttackRadius(playerTransform.position.x, transform.position.x))
            {
                PerformAttack();
            }
            else
            {
                MoveTowardsPlayer(playerTransform.position.x < transform.position.x ? -1 : 1);
            }
        }
        else
        {
            enemyAnim.SetBool("Walking", false);
            Debug.Log("Idle");
        }
    }

    void PerformAttack()
    {
        enemyAnim.SetBool("Attack", true);
        Debug.Log("Attack");
    }

    void MoveTowardsPlayer(int direction)
    {
        this.transform.position += new Vector3(direction * getMoveSpeed() * Time.deltaTime, 0f, 0f);
        enemyAnim.SetBool("Attack", false);
        enemyAnim.SetBool("Walking", true);
        Debug.Log(direction > 0 ? "Walk Right" : "Walk Left");
    }

    public void Attack1()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset2.x;
        pos += transform.up * attackOffset2.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange2, attackMask2);
        if (colInfo != null)
        {
            colInfo.GetComponent<Health>().TakeDamage(getAttackDamage());
            Instantiate(Effect, transform.position, Effect.transform.rotation);
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset2.x;
        pos += transform.up * attackOffset2.y;

        Gizmos.DrawWireSphere(pos, attackRange2);
    }
}
