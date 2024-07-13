using UnityEngine;

public class Flip : MonoBehaviour
{
    public Transform player;
    public bool isFlipped = false;

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;

        if (transform.position.x > player.position.x && isFlipped)
        {
            flipped.x *= -1f;
            transform.localScale = flipped;
            isFlipped = false;
            Debug.Log("Flipped to face left");
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            flipped.x *= -1f;
            transform.localScale = flipped;
            isFlipped = true;
            Debug.Log("Flipped to face right");
        }
    }

}
