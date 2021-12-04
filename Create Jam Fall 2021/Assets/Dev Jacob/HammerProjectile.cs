using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerProjectile : MonoBehaviour
{
    public float rotationSpeed = 1f;
    public float speed = 1f;
    public float timeBeforeReturning = 1f;
    private float timer = 0;
    Rigidbody2D rb;

    public Transform spawnPoint;
    public Vector3 moveToPos;
    public bool returning = false;
    Vector2 dir;

    public float damage;

    List<GameObject> enemiesHit = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            for (int i = 0; i < enemiesHit.Count; i++)
                if (enemiesHit[i] == collision.gameObject)
                    return;
            enemiesHit.Add(collision.gameObject);
            collision.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(damage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(new Vector3(0,0,rotationSpeed), Space.Self);

        if (timer >= timeBeforeReturning)
        {
            dir = new Vector2(spawnPoint.position.x, spawnPoint.position.y) - new Vector2(transform.position.x, transform.position.y);

            if (!returning)
            {
                returning = true;
                enemiesHit = new List<GameObject>();

            }
        }
        else
        {
            dir = new Vector2(moveToPos.x, moveToPos.y) - new Vector2(transform.position.x, transform.position.y);
            timer += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + dir * speed * Time.fixedDeltaTime);
    }
}
