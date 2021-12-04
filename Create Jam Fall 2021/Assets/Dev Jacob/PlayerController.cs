using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public GameObject hammerProjectile;
    public float shootDistance = 1f;
    public Transform shootTransform;
    SpriteRenderer sr;

    public float moveSpeed = 5f;
    public float damage = 1f;
    public float invinsibilityTime = 1f;
    private float invinsibilityTimer = 0f;
    bool hammerOut = false;

    public float blinkTime = .2f;
    private float blinkTimer = 0f;
    bool invinsible = false;

    Rigidbody2D rb;

    Vector2 movement;
    public TMP_Text healthText;
    public float startHealth = 100;
    private float health;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = startHealth;
        healthText.text = "Health: " + health;
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D (Collider2D collision)
    {
        if (collision.gameObject.tag == "Hammer")
        {
            if (collision.gameObject.GetComponent<HammerProjectile>().returning)
            {
                Destroy(collision.gameObject);
                hammerOut = false;
            }

        }
        else if (collision.gameObject.tag == "Enemy")
        {
               TakeDamage(collision.GetComponent<EnemyBehaviour>().damage);
        }

    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = worldPosition - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg -90;

        Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 1000);

        if (Input.GetMouseButtonDown(0) && !hammerOut)
        {
            var hammer = Instantiate(hammerProjectile, shootTransform);
            hammer.transform.parent = null;
            hammer.GetComponent<HammerProjectile>().moveToPos = transform.position + (new Vector3(dir.x, dir.y, 0).normalized * shootDistance);
            hammer.GetComponent<HammerProjectile>().spawnPoint = transform;
            hammer.GetComponent<HammerProjectile>().damage = damage;
            hammerOut = true;
        }

        if (invinsible)
        {
            if (invinsibilityTimer >= invinsibilityTime)
            {
                invinsible = false;
                sr.enabled = true;
            }
            else
            {
                invinsibilityTimer += Time.deltaTime;

                if (blinkTimer >= blinkTime)
                {
                    if (sr.enabled)
                        sr.enabled = false;
                    else
                        sr.enabled = true;
                    blinkTimer = 0;
                }
                blinkTimer += Time.deltaTime;
            }
        }
    }

    public void TakeDamage(float damageTaken)
    {
        if (!invinsible)
        {
            health -= damageTaken;
            healthText.text = "Health: " + health;

            invinsibilityTimer = 0;
            invinsible = true;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
