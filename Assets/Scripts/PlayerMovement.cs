using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private double movementSpeed;
    [SerializeField] private int jumpHeight;

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
    public void onMove(InputAction.CallbackContext context) {
        Debug.Log("move");
        Vector2 moveAmount = context.ReadValue<Vector2>();
        rigidbody.AddForce(moveAmount, ForceMode2D.Impulse);
    }

    public void onJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");
        rigidbody.AddForce(new Vector2(0, 1), ForceMode2D.Impulse);
    }
}
