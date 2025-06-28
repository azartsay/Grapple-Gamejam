using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private InputSystem_Actions inputActions;
    private Vector2 moveInput;

    private float verticalVelocity = 0f;
    private bool isGrounded = true;

    private void Awake() {

        inputActions = new InputSystem_Actions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => Jump();

        inputActions.Enable();
    }

    private void Update() {
        Vector3 dir = new Vector3(moveInput.x, 0f, 0f);
        transform.position += (Vector3)dir * Time.deltaTime * speed;
    }

    private void Jump() {
        if (isGrounded) {
            verticalVelocity = jumpForce;
            isGrounded = false;
        }
    }

    private void OnDisable() {
        inputActions.Disable();
    }

    
}
    