using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D collider2D;
    private Vector2 movement;

    [Header("PlayerSettings")]
    public int movespeed = 8;

    public void OnMove(InputAction.CallbackContext callback)
    {
        callback.ReadValue<Vector2>();
    }

    private void Update()
    {
        PlayerMoveInput();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMoveInput()
    {
        if (movement != Vector2.zero)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
        }
    }

    void PlayerMovement()
    {
        Vector2 dir = new Vector2(movement.x * movespeed * Time.fixedDeltaTime, rb.linearVelocity.y);
        rb.linearVelocity = dir;
    }
}
