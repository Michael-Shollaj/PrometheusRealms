using UnityEngine;

public class Statue : MonoBehaviour
{
    public string requiredSigilTag;
    private GameObject placedSigil;
    public GameObject SigilEffect;

    public bool CanPlaceSigil(GameObject sigil)
    {
        return sigil.CompareTag(requiredSigilTag) && placedSigil == null;
    }

    public void PlaceSigil(GameObject sigil)
    {
        if (CanPlaceSigil(sigil))
        {
            placedSigil = sigil;
            placedSigil.transform.SetParent(transform);
            placedSigil.transform.localPosition = Vector3.zero;
            Instantiate(SigilEffect, transform.position, Quaternion.identity);

            // Disable the sigil's collider to prevent further interaction
            Collider2D sigilCollider = placedSigil.GetComponent<Collider2D>();
            if (sigilCollider != null)
            {
                sigilCollider.enabled = false;
            }
        }
    }

    public bool IsSigilPlacedCorrectly()
    {
        return placedSigil != null;
    }

    public bool HasSigil()
    {
        return placedSigil != null;
    }
}