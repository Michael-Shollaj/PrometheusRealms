using UnityEngine;
using UnityEngine.UI;
using System.Collections;

    public class Health : MonoBehaviour
    {
        public int health;
        public int numOfHearts;

    [SerializeField] private SpriteRenderer[] characterSprites;

    public Image[] hearts;
        public Sprite fullHeart;
        public Sprite emptyHeart;

    public GameObject HeartLoss;
    public GameObject Blood;


        [Header("Audio Settings")]
        [SerializeField] private AudioClip hurtSound;
        [SerializeField] private AudioClip deathSound;

        public GameObject gameOverPanel; // Reference to the Game Over panel
        private SaveSystem saveSystem;


    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);
        saveSystem = SaveSystem.instance;
    }

    // Update is called once per frame
    void Update()
        {
            if (health > numOfHearts)
            {
                health = numOfHearts;
            }

            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < health)
                {
                    hearts[i].sprite = fullHeart;
                }
                else
                {
                    hearts[i].sprite = emptyHeart;
                }

                if (i < numOfHearts)
                {
                    hearts[i].enabled = true;
                }
                else
                {
                    hearts[i].enabled = false;
                }
            }

            if (health <= 0)
            {
                GameOver();
            }

        if (Input.GetKeyDown(KeyCode.C) && SaveSystem.instance.CanSaveGame())
        {
            SaveSystem.instance.SaveGame(transform.position);
        }
    }



    public void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            Debug.LogWarning("Attempted to deal non-positive damage. Ignoring.");
            return;
        }

        Instantiate(Blood, transform.position, Quaternion.identity);
        Instantiate(HeartLoss, transform.position, Quaternion.identity);

        StartCoroutine(DamageAnimation());

        int previousHealth = health;
        health = Mathf.Max(0, health - damage);

        Debug.Log($"TakeDamage called. Health changed from {previousHealth} to {health}. Damage taken: {damage}");

        if (health > 0)
        {
            SoundFXManager.instance.PlaySoundFXClip(hurtSound, transform, 1f);
        }
        else
        {
            SoundFXManager.instance.PlaySoundFXClip(deathSound, transform, 1f);
            GameOver();
        }

        UpdateHeartsUI();
    }

    public void PickupHealth(int amount)
        {
            health += amount;
            if (health > numOfHearts)
            {
                health = numOfHearts;
            }
            UpdateHeartsUI();
        }

        void UpdateHeartsUI()
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < health)
                {
                    hearts[i].sprite = fullHeart;
                }
                else
                {
                    hearts[i].sprite = emptyHeart;
                }

                if (i < numOfHearts)
                {
                    hearts[i].enabled = true;
                }
                else
                {
                    hearts[i].enabled = false;
                }
            }
        }

    IEnumerator DamageAnimation()
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < 3; i++)
        {
            foreach (SpriteRenderer sr in srs)
            {
                Color c = sr.color;
                c.a = 0;
                sr.color = c;
            }

            yield return new WaitForSeconds(.1f);

            foreach (SpriteRenderer sr in srs)
            {
                Color c = sr.color;
                c.a = 1;
                sr.color = c;
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    void GameOver()
    {
        GameManager.instance.PlayerDied();
        // Don't destroy the player object here, as we want to respawn it
        // Destroy(gameObject);
    }


    public void ResetHealth()
    {
        health = numOfHearts;
        UpdateHeartsUI();
    }
}


