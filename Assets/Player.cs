
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D collider2D;
    private Vector2 movement;

    [Header("PlayerSettings")]
    public float movespeed = 8;
    public float jumpPower = 6;

    [Header("Jump Buffer / GroundTimer")]
    public float jumpbufferTime = 0.12f;
    public float jumpbufferCounter;
    public float groundTime = 0.12f;
    public float groundTimeCounter;

    [Header("Ground Check")]
    public bool isGrounded = false;
    public LayerMask groundlayer;
    //RayCast

    [Header("Gravity")]
    public float addGravity = 1.2f;
    public void OnMove(InputAction.CallbackContext callback)
    {
        movement = callback.ReadValue<Vector2>();
    }
    private void Update()
    {
        PlayerControl();
    }
    private void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        Vector2 dir = new Vector2(movement.x * movespeed, rb.linearVelocity.y);
        rb.linearVelocity = dir;
    }

    void PlayerControl()
    {

    }
}
