using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed; // horizontal movement speed
    [SerializeField] private int jumpHeight; // number of blocks the player can jump
    private bool canJump = true; // checks if the user can jump or not
    private const float MAX_VELOCITY = 5f; // the maximum horizontal velocity the player can have
    private const float SLOWDOWN_SPEED = 0.99f; // how fast to slow the player down if they pass the max velocity (smaller is faster but less smooth)

    // Components
    private new Rigidbody2D rigidbody;
    private InputAction moveAction;
    private InputAction jumpAction;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponentInChildren<Rigidbody2D>();
        moveAction = GameManager.Instance.inputActions.Runner.Move;
        jumpAction = GameManager.Instance.inputActions.Runner.Jump;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(rigidbody.velocity.x) > MAX_VELOCITY)
        {
            rigidbody.velocity *= new Vector2(SLOWDOWN_SPEED, 1);
        }
        else if (moveAction.inProgress)
        {
            rigidbody.AddForce(new Vector2(moveAction.ReadValue<float>(), 0));
        }
        if (canJump && jumpAction.triggered)
        {
            float targetVelocity = Mathf.Sqrt((jumpHeight * 10 + 1) * 2 * rigidbody.gravityScale);
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, targetVelocity);
            canJump = false;
        }
        else if (rigidbody.velocity.y == 0)
        {
            canJump = true;
        }
    }
}
