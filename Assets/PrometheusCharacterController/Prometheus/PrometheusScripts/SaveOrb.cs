using UnityEngine;

public class SaveOrb : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SaveSystem.instance.CollectSaveOrb();
            if (collectSound != null)
            {
                SoundFXManager.instance.PlaySoundFXClip(collectSound, transform, 1f);
            }
            Destroy(gameObject);
        }
    }
}