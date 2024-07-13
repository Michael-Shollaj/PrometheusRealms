using UnityEngine;

public class PlayerSigil : MonoBehaviour
{
    public float interactDistance = 2f;
    private GameObject carriedSigil = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // E key for interaction
        {
            if (carriedSigil == null)
            {
                TryPickUpSigil();
            }
            else
            {
                TryPlaceSigil();
            }
        }
    }

    void TryPickUpSigil()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactDistance);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("RedSigil") || hit.CompareTag("GreenSigil") || hit.CompareTag("BlueSigil") || hit.CompareTag("OrangeSigil") || hit.CompareTag("PurpleSigil"))
            {
                // Check if the sigil is not already placed on a statue
                Statue parentStatue = hit.GetComponentInParent<Statue>();
                if (parentStatue == null || !parentStatue.HasSigil())
                {
                    carriedSigil = hit.gameObject;
                    carriedSigil.transform.SetParent(transform);
                    carriedSigil.transform.localPosition = Vector3.zero;
                    break;
                }
            }
        }
    }

    void TryPlaceSigil()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactDistance);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Statue"))
            {
                Statue statue = hit.GetComponent<Statue>();
                if (statue && !statue.HasSigil() && statue.CanPlaceSigil(carriedSigil))
                {
                    statue.PlaceSigil(carriedSigil);
                    carriedSigil = null;
                    PuzzleManager.Instance.CheckPuzzleCompletion();
                    break;
                }
            }
        }
    }
}