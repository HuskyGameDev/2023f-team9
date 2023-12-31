using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public float movementSpeed; // horizontal movement speed
    [SerializeField] public int jumpHeight; // number of blocks the player can jump
    private bool canJump = false; // checks if the user can jump or not
    private const float AIR_DRAG = 0.9f; // how much slower the player is while in the air (0.9 is 90% speed)

    // Components
    private TilemapCollider2D tilemapCollider; // the board's tilemap collider
    private new Rigidbody2D rigidbody;
    private EdgeCollider2D bottomEdgeCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private InputAction moveAction;
    private InputAction jumpAction;

    //power up
    private bool isSpeedUpActive = false;
    private float originalMovementSpeed;
    private float speedUpTimer = 0.0f;
    private bool isJumpUpActive = false;
    private int originalJumpHeight;
    private float jumpUpTimer = 0.0f;

    void Awake()
    {
        moveAction = GameManager.Instance.inputActions.Runner.Move;
        jumpAction = GameManager.Instance.inputActions.Runner.Jump;

        originalMovementSpeed = movementSpeed;
        originalJumpHeight = jumpHeight;
    }

    void Start()
    {
        rigidbody = this.GetComponentInChildren<Rigidbody2D>();
        bottomEdgeCollider = this.GetComponentInChildren<EdgeCollider2D>();
        animator = this.GetComponentInChildren<Animator>();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        animator.SetBool("Falling", false);
        try
        {
            tilemapCollider = FindAnyObjectByType<TilemapCollider2D>();
        }
        catch
        {
            // do nothing, it doesn't matter if there is no tilemap for now
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!moveAction.inProgress)
        {
            animator.SetBool("Running", false);
        }
        if (moveAction.inProgress)
        {
            animator.SetBool("Running", true);
            float direction = moveAction.ReadValue<float>();
            spriteRenderer.flipX = direction < 0;
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
            if (animator.GetBool("Falling"))
            {
                animator.SetBool("Falling", false);
            }
        }
        if (canJump && jumpAction.triggered)
        {
            animator.SetTrigger("Jump");
            StartCoroutine(Jump());
        }

        //Animations
        if (!animator.GetBool("Falling") && rigidbody.velocity.y < 0)
        {
            animator.SetBool("Falling", true);
        }

        //power up
        if (isSpeedUpActive)
        {

            speedUpTimer -= Time.deltaTime;

            //Debug.Log("Speed Up Timer: " + speedUpTimer);

            if (speedUpTimer <= 0.0f)
            {
                // Speed-up duration has expired, reset the speed
                isSpeedUpActive = false;
                movementSpeed = originalMovementSpeed; // Reset the speed
                //Debug.Log("Speed Up Expired");
            }
        }
        if (isJumpUpActive)
        {
            jumpUpTimer -= Time.deltaTime;

            if (jumpUpTimer <= 0f)
            {
                isJumpUpActive = false;
                jumpHeight = originalJumpHeight;
            }
        }
    }

    private IEnumerator Jump()
    {
        float targetVelocity = Mathf.Sqrt((jumpHeight * 10 + 1) * 2 * rigidbody.gravityScale);
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, targetVelocity);
        yield return new WaitForFixedUpdate(); // prevents accidental double jump
        canJump = false;
    }

    // method to apply the speed-up effect
    public void ApplySpeedUp(float speedMultiplier, float duration)
    {
        movementSpeed += speedMultiplier;
        isSpeedUpActive = true;
        speedUpTimer = duration;
    }

    public void ApplyJumpUp(int jumpAdder, float duration)
    {
        jumpHeight += jumpAdder;
        isJumpUpActive = true;
        jumpUpTimer = duration;
    }
}
