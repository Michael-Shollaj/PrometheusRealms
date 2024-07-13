using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrinking : MonoBehaviour
{
    public float shrinkRate = 0.1f; // Rate at which the object shrinks per second
    public float minSize = 0.1f;    // Minimum size before the object is destroyed

    void Update()
    {
        // Reduce the scale of the object over time
        transform.localScale -= Vector3.one * shrinkRate * Time.deltaTime;

        // Check if the object's size is below the minimum size
        if (transform.localScale.x <= minSize || transform.localScale.y <= minSize || transform.localScale.z <= minSize)
        {
            // Destroy the object
            Destroy(gameObject);
        }
    }
}