using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesctructableObject : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        Destroy(gameObject);
    }
}
