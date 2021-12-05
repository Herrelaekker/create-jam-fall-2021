using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float damage = 1f;
    public float timeBeforeHammerBack = 1f;
    public float moveSpeed = 5f;
    public float shootDistance = 3f;
    public float startHealth = 100f;
    public float health = 100f;
    public float hammerRadius = 1f;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeStats(float a, float b, float c, float d, float e, float f, float g)
    {
        damage = a;
        timeBeforeHammerBack = b;
        moveSpeed = c;
        shootDistance = d;

        startHealth = e;
        health = f;
        hammerRadius = g;
    }


}
