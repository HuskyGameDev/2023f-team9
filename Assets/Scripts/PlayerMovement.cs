using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed; // horizontal movement speed
    [SerializeField] private int jumpHeight; // number of blocks the player can jump

    // Components
    private new Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponentInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Movement Controls
    public void OnMove(InputValue value)
    {
        Debug.Log("move");
    }

    public void OnJump()
    {
        float targetVelocity = Mathf.Sqrt((jumpHeight * 10 + 1) * 2 * rigidbody.gravityScale);
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, targetVelocity);
    }
}
