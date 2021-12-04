using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float health = 5f;

    public void TakeDamage(float damageTaken)
    {
        health -= damageTaken;
        print("AV! health Left: " + health);
        if (health <= 0)
            Destroy(gameObject);
    }
}
