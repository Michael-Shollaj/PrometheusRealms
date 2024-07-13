using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int maxDeaths = 3;
    private int currentDeaths = 0;
    public GameObject GameOverPanel;
    public GameObject respawnParticlePrefab;

    private void Start()
    {
        GameOverPanel.SetActive(false);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerDied()
    {
        currentDeaths++;
        if (currentDeaths >= maxDeaths)
        {
            ShowGameOverScreen();
        }
        else
        {
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        Vector3 respawnPosition = SaveSystem.instance.GetLastSavePosition();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = respawnPosition;
            player.GetComponent<Health>().ResetHealth();

            // Play respawn particle effect
            if (respawnParticlePrefab != null)
            {
                Instantiate(respawnParticlePrefab, respawnPosition, Quaternion.identity);
            }
        }
    }

    private void ShowGameOverScreen()
    {
        GameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        currentDeaths = 0;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}