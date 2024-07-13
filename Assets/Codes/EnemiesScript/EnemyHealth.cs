using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public GameObject deathEffect;
    [SerializeField] private AudioClip hurtSound;
    public SpriteRenderer[] bodyParts;
    public Color hurtColor;




    void Update()
    {

        if (health <= 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }



    public void TakeDamage(int damage)
    {
        StartCoroutine(Flash());
         health -= damage;
         SoundFXManager.instance.PlaySoundFXClip(hurtSound, transform, 1f);
    }

    IEnumerator Flash()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].color = hurtColor; 
        }

        yield return new WaitForSeconds(0.05f);

        for(int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].color = Color.white;
        }
    }
}