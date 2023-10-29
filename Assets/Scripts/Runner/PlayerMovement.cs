using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed; // horizontal movement speed
    [SerializeField] private int jumpHeight; // number of blocks the player can jump
    private bool canJump = false; // checks if the user can jump or not
    private const float AIR_DRAG = 0.9f; // how much slower the player is while in the air (0.9 is 90% speed)

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
            tilemapCollider = FindAnyObjectByType<TilemapCollider2D>();
        }
        catch
        {
            // do nothing, it doesn't matter if there is no tilemap for now
            Debug.LogError("PlayerMovement could not find board tilemap");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moveAction.inProgress)
        {
            float direction = moveAction.ReadValue<float>();
            if (canJump)
            {
                // speed on ground
                rigidbody.velocity = new Vector2(direction * movementSpeed, rigidbody.velocity.y);
            }
            else
            {
                // speed in air
                rigidbody.velocity = new Vector2(direction * movementSpeed * AIR_DRAG, rigidbody.velocity.y);
            }
        }
        else if (canJump)
        {
            // no ice effect when on ground
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }

        if (tilemapCollider && bottomEdgeCollider.IsTouching(tilemapCollider))
        {
            canJump = true;
        }
        if (canJump && jumpAction.triggered)
        {
            StartCoroutine(Jump());
        }
    }

    private IEnumerator Jump()
    {
        float targetVelocity = Mathf.Sqrt((jumpHeight * 10 + 1) * 2 * rigidbody.gravityScale);
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, targetVelocity);
        yield return new WaitForFixedUpdate(); // prevents accidental double jump
        canJump = false;
    }
}
