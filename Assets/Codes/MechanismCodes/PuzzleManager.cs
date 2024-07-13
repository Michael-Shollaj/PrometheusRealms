using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;
    public GameObject portal;
    public Statue[] statues;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        statues = FindObjectsOfType<Statue>();
        portal.SetActive(false);
    }

    public void CheckPuzzleCompletion()
    {
        foreach (Statue statue in statues)
        {
            if (!statue.IsSigilPlacedCorrectly())
            {
                return;
            }
        }
        OpenPortal();
    }

    void OpenPortal()
    {
        portal.SetActive(true);
    }
}
