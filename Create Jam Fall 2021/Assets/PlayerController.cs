using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject hammerProjectile;
    public float shootDistance = 1f;
    public Transform shootTransform;

    public float moveSpeed = 5f;
    public float damage = 1f;

    Rigidbody2D rb;

    Vector2 movement;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D (Collider2D collision)
    {
        if (collision.gameObject.tag == "Hammer")
        {
            if (collision.gameObject.GetComponent<HammerProjectile>().returning)
                Destroy(collision.gameObject);
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

        if (Input.GetMouseButtonDown(0))
        {
            var hammer = Instantiate(hammerProjectile, shootTransform);
            hammer.transform.parent = null;
            hammer.GetComponent<HammerProjectile>().moveToPos = transform.position + (new Vector3(dir.x, dir.y, 0).normalized * shootDistance);
            hammer.GetComponent<HammerProjectile>().spawnPoint = shootTransform;
            hammer.GetComponent<HammerProjectile>().damage = damage;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
