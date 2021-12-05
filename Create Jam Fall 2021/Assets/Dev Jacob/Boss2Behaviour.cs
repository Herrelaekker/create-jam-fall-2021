using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Behaviour : EnemyBehaviour
{
    string curState = "Spawn";
    public float[] lockOnTimePerLevel = { 3f, 2f, 1f };
    float lockOnTimer = 0f;
    public float[] waitTimeBeforeChargePerLevel = { 1f, 0.75f, 0.5f };
    float waitTimer = 0f;

    Vector3 dir;

    public int aggressionLevel = 0;
    public int[] agressionChange = { 30, 10 };

    public int[] chargesPerLevel = { 3, 4, 5 };
    public float[] laserLifeTime = { 1f,.75f,.5f};
    public float[] laserWidth = { 3f,4f,5f};
    private int curCharges = 0;
    private float laserLifeTimer = 0f;
    bool waitForLaser = false;

    public float[] projectilesPerLevel = { 50,50,50};
    private float curProjectiles = 0f;

    public float spawnTimer = 0f;
    public float[] timeBeforeSpawning = { 0.3f, .2f, .1f };
    public Transform[] spawnPoints;
    public GameObject laserPrefab;
    public GameObject projectilePrefab;
    public float rotationSpeed;

    public Animator anim;

    bool chargedUp = false;
    public Sprite chargedUpSprite;
    public Sprite normalSprite;
    public SpriteRenderer se;

    private void Start()
    {
        health = startHealth;
        player = GameObject.FindGameObjectWithTag("Player");
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
            case "Laser":
                curState = "Spawn";
                break;

            case "Spawn":
                curState = "Laser";
                break;
        }

        

        if (aggressionLevel != 2)
        {
            if (health <= agressionChange[aggressionLevel])
                aggressionLevel++;

        }

        if (aggressionLevel >= 2)
            curState = "Both";

    }

    private void FixedUpdate()
    {

        switch (curState)
        {
            case "Laser":
                if (lockOnTimer < lockOnTimePerLevel[aggressionLevel])
                {
                    anim.enabled = false;
                    se.sprite = normalSprite;
                    dir = player.transform.position - transform.position;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

                    Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    body.transform.rotation = Quaternion.Slerp(body.transform.rotation, lookRotation, Time.fixedDeltaTime * rotationSpeed);
                    lockOnTimer += Time.fixedDeltaTime;
                }
                else
                {
                    anim.enabled = true;
                    anim.SetBool("Laser", true);
                    anim.SetBool("Spawn", false);
                    if (waitTimer >= waitTimeBeforeChargePerLevel[aggressionLevel])
                    {


                        if (!waitForLaser)
                        {
                            var laser = Instantiate(laserPrefab, spawnPoints[0]);
                            laser.GetComponent<Laser>().lifeTime = laserLifeTime[aggressionLevel];
                            laser.transform.localScale += new Vector3(laserWidth[aggressionLevel],0,0); 
                            laser.transform.parent = null;
                            waitForLaser = true;
                        }

                        if (laserLifeTimer >= laserLifeTime[aggressionLevel])
                        {
                            waitTimer = 0;
                            lockOnTimer = 0;
                            laserLifeTimer = 0;
                            waitForLaser = false;
                            curCharges += 1;

                            anim.SetBool("Laser", false);

                            if (curCharges >= chargesPerLevel[aggressionLevel])
                            {
                                curCharges = 0;
                                anim.enabled = false;
                                se.sprite = normalSprite;
                                ChangeState();
                            }
                        }
                        else
                        {
                            laserLifeTimer += Time.fixedDeltaTime;

                        }
                    }
                    else
                        waitTimer += Time.fixedDeltaTime;
                }

                break;
            case "Spawn":
                if (spawnTimer >= timeBeforeSpawning[aggressionLevel])
                {
                    anim.enabled = true;
                    anim.SetBool("Laser", false);
                    anim.SetBool("Spawn", true);

                    if (curProjectiles % 2 == 0)
                    {
                        for (int i = 1; i < 6; i++)
                        {
                            var enemy = Instantiate(projectilePrefab, spawnPoints[i]);

                            Vector3 dir = new Vector2(spawnPoints[i].up.x, spawnPoints[i].up.y);
                            //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

                            enemy.GetComponent<HelProjectile>().dir = (new Vector2(dir.x, dir.y)).normalized;
                            enemy.transform.parent = null;
                        }
                    }
                    else
                    {
                        for (int i = 6; i < 10; i++)
                        {
                            var enemy = Instantiate(projectilePrefab, spawnPoints[i]);

                            Vector3 dir = new Vector2(spawnPoints[i].up.x, spawnPoints[i].up.y);
                            //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

                            enemy.GetComponent<HelProjectile>().dir = (new Vector2(dir.x, dir.y)).normalized;
                            enemy.transform.parent = null;
                        }
                    }

                    spawnTimer = 0f;
                    curProjectiles += 1;

                    if (curProjectiles >= projectilesPerLevel[aggressionLevel])
                    {
                        curProjectiles = 0;
                        anim.enabled = false;
                        anim.SetBool("Spawn", false);
                        se.sprite = chargedUpSprite;
                        ChangeState();
                    }
                }
                else
                {
                    spawnTimer += Time.fixedDeltaTime;
                }
                break;
            case "Both":

                if (spawnTimer >= timeBeforeSpawning[aggressionLevel])
                {
                    if (!chargedUp)
                    {
                        anim.SetBool("Spawn", true);
                        anim.SetBool("Laser", false);
                        chargedUp = true;
                    }

                    if (curProjectiles % 2 == 0)
                    {
                        for (int i = 1; i < 6; i++)
                        {
                            var enemy = Instantiate(projectilePrefab, spawnPoints[i]);

                            Vector3 dir = new Vector2(spawnPoints[i].up.x, spawnPoints[i].up.y);
                            //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

                            enemy.GetComponent<HelProjectile>().dir = (new Vector2(dir.x, dir.y)).normalized;
                            enemy.transform.parent = null;
                        }
                    }
                    else
                    {
                        for (int i = 6; i < 10; i++)
                        {
                            var enemy = Instantiate(projectilePrefab, spawnPoints[i]);

                            Vector3 dir = new Vector2(spawnPoints[i].up.x, spawnPoints[i].up.y);
                            //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

                            enemy.GetComponent<HelProjectile>().dir = (new Vector2(dir.x, dir.y)).normalized;
                            enemy.transform.parent = null;
                        }
                    }

                    spawnTimer = 0f;
                    curProjectiles += 1;
                }
                else
                {
                    spawnTimer += Time.fixedDeltaTime;
                }

                if (lockOnTimer < lockOnTimePerLevel[aggressionLevel])
                {
                    anim.enabled = false;
                    se.sprite = chargedUpSprite;
                    dir = player.transform.position - transform.position;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

                    Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    body.transform.rotation = Quaternion.Slerp(body.transform.rotation, lookRotation, Time.fixedDeltaTime * rotationSpeed);
                    lockOnTimer += Time.fixedDeltaTime;
                }
                else
                {
                    anim.enabled = true;
                    anim.SetBool("Both", true);

                    anim.SetBool("Laser", false);
                    anim.SetBool("Spawn", false);
                    if (waitTimer >= waitTimeBeforeChargePerLevel[aggressionLevel])
                    {
                        if (!waitForLaser)
                        {
                            var laser = Instantiate(laserPrefab, spawnPoints[0]);
                            laser.GetComponent<Laser>().lifeTime = laserLifeTime[aggressionLevel];
                            laser.transform.localScale += new Vector3(laserWidth[aggressionLevel], 0, 0);
                            laser.transform.parent = null;
                            waitForLaser = true;
                        }

                        if (laserLifeTimer >= laserLifeTime[aggressionLevel])
                        {
                            waitTimer = 0;
                            lockOnTimer = 0;
                            laserLifeTimer = 0;
                            waitForLaser = false;
                            curCharges += 1;

                            anim.SetBool("Both", false);

                            if (curCharges >= chargesPerLevel[aggressionLevel])
                            {
                                curCharges = 0;
                                ChangeState();
                            }
                        }
                        else
                            laserLifeTimer += Time.fixedDeltaTime;

                    }
                    else
                        waitTimer += Time.fixedDeltaTime;
                }

                break;

        }



        //rb.MovePosition(rb.position + new Vector2(dir.x, dir.y).normalized * moveSpeed * Time.fixedDeltaTime);
    }



}
