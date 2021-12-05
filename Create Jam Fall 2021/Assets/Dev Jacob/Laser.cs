using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //public Vector2 dir;
    public float damage = 15f;

    public float lifeTime = 1f;
    private float lifeTimer = 0f;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    private void FixedUpdate()
    {
        if (lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
        }
        else
            lifeTimer += Time.fixedDeltaTime;
    }
}
