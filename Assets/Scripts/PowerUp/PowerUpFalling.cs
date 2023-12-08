using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpFalling : MonoBehaviour
{
    float wait;
    public GameObject powerUpObject;

    void Awake()
    {
        wait = Random.Range(3.0f, 10.0f);
        InvokeRepeating("Fall", wait, wait);
    }

    void Fall()
    {
        Instantiate(powerUpObject, new Vector3(Random.Range(-4, 4), 10, 0), Quaternion.identity);
    }
}
