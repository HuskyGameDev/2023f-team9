using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed; // horizontal movement speed
    [SerializeField] private int jumpHeight; // number of blocks the player can jump
    private bool canJump = false; // checks if the user can jump or not
    private const float MAX_VELOCITY = 5f; // the maximum horizontal velocity the player can have
    private const float SLOWDOWN_SPEED = 0.99f; // how fast to slow the player down if they pass the max velocity (smaller is faster but less smooth)

    // Components
    private TilemapCollider2D tilemapCollider; // the board's tilemap collider
    private new Rigidbody2D rigidbody;
    private EdgeCollider2D bottomEdgeCollider;
    private InputAction moveAction;
    private InputAction jumpAction;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponentInChildren<Rigidbody2D>();
        bottomEdgeCollider = this.GetComponentInChildren<EdgeCollider2D>();
        moveAction = GameManager.Instance.inputActions.Runner.Move;
        jumpAction = GameManager.Instance.inputActions.Runner.Jump;

        try
        {
            tilemapCollider = GameObject.FindGameObjectWithTag("Board").GetComponentInChildren<TilemapCollider2D>();
        }
        catch
        {
            // do nothing, it doesn't matter if there is no tilemap for now
        }
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

        if (tilemapCollider && bottomEdgeCollider.IsTouching(tilemapCollider))
        {
            canJump = true;
        }
        if (canJump && jumpAction.triggered)
        {
            canJump = false;
            float targetVelocity = Mathf.Sqrt((jumpHeight * 10 + 1) * 2 * rigidbody.gravityScale);
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, targetVelocity);
        }
    }
}
