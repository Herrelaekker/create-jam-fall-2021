using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : EnemyBehaviour
{
    string curState = "Shoot";
    public float lockOnTime = 3f;
    float lockOnTimer = 0f;
    public float waitTimeBeforeCharge = 1f;
    float waitTimer = 0f;

    Vector3 dir;
    public float chargeSpeed = 10f;

    public int aggressionLevel = 0;
    public int[] chargesPerLevel = { 3,4,5};
    private int curCharges = 0;

    private float spawnTimer = 0f;
    public float timeBeforeSpawning = 0.3f;
    public Transform[] spawnPoints;
    public GameObject projectilePrefab;
    public float rotationSpeed;

    private void Start()
    {
        health = startHealth;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            lockOnTimer = 0f;
            waitTimer = 0f;
            curCharges += 1;
            if (curCharges >= chargesPerLevel[aggressionLevel])
            {
                curCharges = 0;
                ChangeState();
            }
        }
    }

    private void ChangeState()
    {
        switch (curState)
        {
            case "Charge":
                curState = "Shoot";
                break;

            case "Shoot":
                curState = "Charge";
                break;

        }
    }

    private void FixedUpdate()
    {
        switch (curState)
        {
            case "Charge":
                if (lockOnTimer < lockOnTime)
                {
                    dir = player.transform.position - transform.position;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

                    Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    body.transform.rotation = Quaternion.Slerp(body.transform.rotation, lookRotation, 10);
                    lockOnTimer += Time.fixedDeltaTime;
                }
                else
                {
                    if (waitTimer >= waitTimeBeforeCharge)
                    {
                        rb.MovePosition(rb.position + new Vector2(dir.x, dir.y).normalized * chargeSpeed * Time.fixedDeltaTime);

                    }
                    else
                        waitTimer += Time.fixedDeltaTime;
                }

                break;
            case "Shoot":
                dir = transform.position;
                body.transform.Rotate(0.0f, 0.0f, Time.fixedDeltaTime*rotationSpeed, Space.Self);

                if (spawnTimer >= timeBeforeSpawning)
                {
                    for (int i = 0; i < spawnPoints.Length; i++)
                    {
                        var enemy = Instantiate(projectilePrefab, spawnPoints[i]);
                        enemy.GetComponent<EnemyProjectile>().dir = new Vector2(dir.x, dir.y).normalized;
                        enemy.transform.parent = null;
                    }

                    spawnTimer = 0f;
                }
                else
                {
                    spawnTimer += Time.deltaTime;
                }
                break;
        }


        //rb.MovePosition(rb.position + new Vector2(dir.x, dir.y).normalized * moveSpeed * Time.fixedDeltaTime);
    }

}
