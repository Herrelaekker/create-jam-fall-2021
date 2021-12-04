using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTypeEnemy : EnemyBehaviour
{
    public float TimeBeforeSpawning = 5f;
    private float spawnTimer = 0f;

    public GameObject enemySpawned;
    public Transform[] spawnPoints;

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = player.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

        Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        body.transform.rotation = Quaternion.Slerp(body.transform.rotation, lookRotation, 10);

        rb.MovePosition(rb.position + new Vector2(dir.x, dir.y).normalized * moveSpeed * Time.fixedDeltaTime);

        if (spawnTimer >= TimeBeforeSpawning)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                var enemy = Instantiate(enemySpawned, spawnPoints[i]);
                enemy.transform.rotation = Quaternion.Euler(0,0,0);
                enemy.transform.parent = null;
            }

            spawnTimer = 0f;
        }
        else
        {
            spawnTimer += Time.deltaTime;
        }
    }
}
