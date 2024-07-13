using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health playerHealth = collision.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);

            // Optional: Add feedback for damage here
            // For example:
            // PlayDamageEffect();

            // If you want to destroy this object after dealing damage:
            // Destroy(gameObject);
        }
    }

    // Optional: Method for visual or audio feedback
    // private void PlayDamageEffect()
    // {
    //     // Add code for damage effect (e.g., particle system, sound)
    // }
}