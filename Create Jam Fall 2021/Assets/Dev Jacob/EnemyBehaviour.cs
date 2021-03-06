using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    public float startHealth = 5f;
    public float health;
    public Slider slider;
    public GameObject player;
    public Rigidbody2D rb;

    public float moveSpeed = 1f;
    public float damage = 15;
    public GameObject body;
    public bool invulnerable = false;

    private void Start()
    {
        health = startHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        slider.gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector3 dir = player.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

        Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        body.transform.rotation = Quaternion.Slerp(body.transform.rotation, lookRotation, 10);

        rb.MovePosition(rb.position + new Vector2(dir.x, dir.y).normalized * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damageTaken)
    {
        if (!invulnerable)
        {
            health -= damageTaken;

            if (!slider.gameObject.activeSelf)
                slider.gameObject.SetActive(false);

            if (health <= 0)
            {
                Destroy(slider.gameObject);
                Destroy(transform.gameObject);
            }
            else
                slider.value = health / startHealth;
        }
    }
}
