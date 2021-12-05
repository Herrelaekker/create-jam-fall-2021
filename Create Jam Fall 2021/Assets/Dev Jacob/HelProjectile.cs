using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelProjectile : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 1f;
    public Vector2 dir;
    public float damage = 15f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);

        }

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
    }
}
