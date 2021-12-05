using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawned : MonoBehaviour
{
    public GameObject enemyPrefab; 

    public void Spawn()
    {
        var enemy = Instantiate(enemyPrefab, transform);
        enemy.transform.rotation = Quaternion.Euler(0, 0, 0);
        enemy.transform.parent = transform.parent;
    }
}
