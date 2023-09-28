using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed; // horizontal movement speed
    [SerializeField] private int jumpHeight; // number of blocks the player can jump
    private float movementDirection; // 0 for stationary, -1 for left, 1 for right (float so it will work with joystick)
    private const float MAX_VELOCITY = 5f; // the maximum horizontal velocity the player can have
    private const float SLOWDOWN_SPEED = 0.99f; // how fast to slow the player down if they pass the max velocity (smaller is faster but less smooth)

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
        if(Mathf.Abs(rigidbody.velocity.x) > MAX_VELOCITY)
        {
            rigidbody.velocity *= new Vector2(SLOWDOWN_SPEED, 1);
        } else if(movementDirection != 0)
        {
            rigidbody.AddForce(new Vector2(movementDirection, 0));
        }
    }

    // Movement Controls
    public void OnMove(InputValue value)
    {
        movementDirection = value.Get<float>();
    }

    public void OnJump()
    {
        float targetVelocity = Mathf.Sqrt((jumpHeight * 10 + 1) * 2 * rigidbody.gravityScale);
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, targetVelocity);
    }
}
