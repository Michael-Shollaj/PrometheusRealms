using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    [Header("Save Orb Settings")]
    public int maxSaveOrbs = 3;
    private int currentSaveOrbs = 0;

    [Header("UI Elements")]
    public Image[] orbUIElements;
    public Image saveReadyIndicator;
    public Text saveReadyText;

    [Header("Sprites")]
    public Sprite activeOrbSprite;
    public Sprite inactiveOrbSprite;

    private List<Vector3> savePositions = new List<Vector3>();

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

    private void Start()
    {
        UpdateSaveOrbsUI();
        CheckSaveReadiness();
    }

    public void CollectSaveOrb()
    {
        if (currentSaveOrbs < maxSaveOrbs)
        {
            currentSaveOrbs++;
            UpdateSaveOrbsUI();
            CheckSaveReadiness();
        }
    }

    public Vector3 GetLastSavePosition()
    {
        if (savePositions.Count > 0)
        {
            return savePositions[savePositions.Count - 1];
        }
        else
        {
            // Return a default position if no saves exist
            return Vector3.zero;
        }
    }

    public bool CanSaveGame()
    {
        return currentSaveOrbs == maxSaveOrbs;
    }

    public void SaveGame(Vector3 playerPosition)
    {
        if (CanSaveGame())
        {
            savePositions.Add(playerPosition);
            currentSaveOrbs = 0;
            UpdateSaveOrbsUI();
            CheckSaveReadiness();
            SaveGameState(playerPosition);
            Debug.Log("Game saved at " + playerPosition);
        }
    }

    private void UpdateSaveOrbsUI()
    {
        for (int i = 0; i < orbUIElements.Length; i++)
        {
            if (i < currentSaveOrbs)
            {
                orbUIElements[i].sprite = activeOrbSprite;
                orbUIElements[i].color = Color.white;
            }
            else
            {
                orbUIElements[i].sprite = inactiveOrbSprite;
                orbUIElements[i].color = Color.gray;
            }
        }
    }

    private void CheckSaveReadiness()
    {
        bool canSave = CanSaveGame();
        if (saveReadyIndicator != null)
        {
            saveReadyIndicator.gameObject.SetActive(canSave);
        }
        if (saveReadyText != null)
        {
            saveReadyText.gameObject.SetActive(canSave);
            saveReadyText.text = canSave ? "Ready to Save!" : "";
        }
    }

    private void SaveGameState(Vector3 playerPosition)
    {
        // TODO: Implement actual save functionality
        // This should save all relevant game state, including:
        // - Player position
        // - Player health
        // - Inventory
        // - Quest progress
        // - World state (e.g., which enemies have been defeated, which items collected)
        PlayerPrefs.SetFloat("PlayerPosX", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerPosY", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerPosZ", playerPosition.z);
        PlayerPrefs.Save();
        Debug.Log("Game state saved at position: " + playerPosition);
    }

    public void LoadGame(int saveIndex)
    {
        if (saveIndex < savePositions.Count)
        {
            Vector3 loadPosition = savePositions[saveIndex];
            LoadGameState(loadPosition);
            Debug.Log("Game loaded from save at " + loadPosition);
        }
    }

    private void LoadGameState(Vector3 playerPosition)
    {
        // TODO: Implement actual load functionality
        // This should load all saved game state, including:
        // - Player position
        // - Player health
        // - Inventory
        // - Quest progress
        // - World state
        float x = PlayerPrefs.GetFloat("PlayerPosX", playerPosition.x);
        float y = PlayerPrefs.GetFloat("PlayerPosY", playerPosition.y);
        float z = PlayerPrefs.GetFloat("PlayerPosZ", playerPosition.z);
        Vector3 loadedPosition = new Vector3(x, y, z);
        // TODO: Move player to loadedPosition
        Debug.Log("Game state loaded for position: " + loadedPosition);
    }
}