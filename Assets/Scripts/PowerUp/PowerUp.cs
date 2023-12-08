using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerUp : MonoBehaviour
{
    public PowerUpEffect powerUpEffect;
    private Rigidbody2D rb;  // Rigidbody2D component for handling physics
    public float fallSpeed = 5.0f;
    public float destroyDelay = 20.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // Set gravity scale to simulate constant falling velocity
        rb.gravityScale = 0.0f;  // Adjust this value to control the falling speed
        rb.velocity = new Vector2(0, -fallSpeed);

        Invoke("DestroyPowerUp", destroyDelay);
    }

    
    // trigger power up on collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        //Debug.Log("trigger with " + collision.gameObject.name);
        if (collision.gameObject.name == "Sprite")
        {
            //Debug.Log("trigger with Sprite");
            Destroy(gameObject);
            powerUpEffect.Apply(collision.gameObject);
        }
        

    }
    private void DestroyPowerUp()
    {
        Destroy(gameObject);
    }

}