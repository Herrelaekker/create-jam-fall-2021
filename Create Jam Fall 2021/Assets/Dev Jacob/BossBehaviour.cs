using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : EnemyBehaviour
{
    string curState = "Charge";
    public float[] lockOnTimePerLevel = { 3f ,2f, 1f};
    float lockOnTimer = 0f;
    public float[] waitTimeBeforeChargePerLevel = { 1f, 0.75f, 0.5f };
    float waitTimer = 0f;

    Vector3 dir;
    public float chargeSpeed = 10f;

    public int aggressionLevel = 0;
    public int[] agressionChange = {30, 10};

    public int[] chargesPerLevel = { 3,4,5};
    private int curCharges = 0;

    public int[] projectilesPerLevel = { 25, 40, 60 };
    private int curProjectiles = 0;

    public float spawnTimer = 0f;
    public float[] timeBeforeSpawning = { 0.3f, .2f, .1f };
    public Transform[] spawnPoints;
    public GameObject projectilePrefab;
    public float rotationSpeed;

    public Animator anim;

    private void Start()
    {
        health = startHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        slider.gameObject.SetActive(true);
        slider.value = 1;
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
        if (aggressionLevel != 2)
        {
            if (health <= agressionChange[aggressionLevel])
                aggressionLevel++;
        }

    }

    private void FixedUpdate()
    {

        switch (curState)
        {
            case "Charge":
                anim.SetBool("Shoot", false);

                if (lockOnTimer < lockOnTimePerLevel[aggressionLevel])
                {
                    anim.SetBool("Charge", false);
                    dir = player.transform.position - transform.position;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

                    Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    body.transform.rotation = Quaternion.Slerp(body.transform.rotation, lookRotation, 10);
                    lockOnTimer += Time.fixedDeltaTime;
                }
                else
                {
                    anim.SetBool("Charge", true);
                    if (waitTimer >= waitTimeBeforeChargePerLevel[aggressionLevel])
                    {
                        rb.MovePosition(rb.position + new Vector2(dir.x, dir.y).normalized * chargeSpeed * Time.fixedDeltaTime);

                    }
                    else
                        waitTimer += Time.fixedDeltaTime;
                }

                break;
            case "Shoot":
                anim.SetBool("Charge", false);
                anim.SetBool("Shoot", true);
                body.transform.Rotate(0.0f, 0.0f, Time.fixedDeltaTime*rotationSpeed, Space.Self);

                if (spawnTimer >= timeBeforeSpawning[aggressionLevel])
                {
                    for (int i = 0; i < spawnPoints.Length; i++)
                    {
                        var enemy = Instantiate(projectilePrefab, spawnPoints[i]);

                        Vector3 dir = new Vector2(spawnPoints[i].up.x, spawnPoints[i].up.y);
                        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

                        enemy.GetComponent<EnemyProjectile>().dir =( new Vector2(dir.x, dir.y)).normalized;
                        enemy.transform.parent = null;
                    }

                    spawnTimer = 0f;
                    curProjectiles += 1;

                    if (curProjectiles >= projectilesPerLevel[aggressionLevel])
                    {
                        curProjectiles = 0;
                        ChangeState();
                    }
                }
                else
                {
                    spawnTimer += Time.fixedDeltaTime;
                }

                break;
        }



        //rb.MovePosition(rb.position + new Vector2(dir.x, dir.y).normalized * moveSpeed * Time.fixedDeltaTime);
    }



}
