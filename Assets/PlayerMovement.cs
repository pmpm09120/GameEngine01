using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float movespeed = 5f;
    private Vector2 movement;
    public PlayerInput playerInput;

    public void OnMove(InputAction.CallbackContext callback)
    {
        movement = callback.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * movespeed * Time.fixedDeltaTime);
        //rb.linearVelocity = movement * movespeed;
    }
}
