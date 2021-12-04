using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    public float startHealth = 5f;
    private float health;
    public Slider slider;
    public GameObject player;
    public Rigidbody2D rb;

    public float moveSpeed = 1f;
    public float damage = 15;
    public Transform body;

    private void Start()
    {
        health = startHealth;
    }

    private void Update()
    {
        Vector3 dir = player.transform.position - body.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

        Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(body.rotation, lookRotation, 10);

        rb.MovePosition(rb.position + new Vector2(dir.x, dir.y).normalized * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damageTaken)
    {
        health -= damageTaken;
        print("AV! health Left: " + health);
        if (health <= 0)
            Destroy(body.gameObject);
        else
            slider.value = health / startHealth;
    }
}
