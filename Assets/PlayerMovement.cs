using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("PlayerMovement")]
    public Rigidbody2D rb;
    public float movespeed = 5f;
    private Vector2 movement;
    public PlayerInput playerInput;
    public Animator animator;

    [Header("PlayerHealth")]
    public float MaxHealth;
    [SerializeField] private float health;

    //Event
    public event Action OnPlayerDied;

    public float _health
    {
        get { return health; }
        set
        {
            health = Mathf.Clamp(value, 0, MaxHealth);
        }
    }

    private void Start()
    {
        //Initalize State
        health = MaxHealth;
    }

    public void OnMove(InputAction.CallbackContext callback)
    {
        movement = callback.ReadValue<Vector2>();

        float rightValue = movement.x > 0.01f ? movement.x : 0f;
        float leftValue = movement.x < -0.01f ? movement.x : 0f;

        animator.SetFloat("Right", rightValue);
        animator.SetFloat("Left", leftValue);
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * movespeed * Time.fixedDeltaTime);
        //rb.linearVelocity = movement * movespeed;
    }

    public void TakeDamage(float dam)
    {
        health -= dam;
        
        if (health <= 0)
        {
            Debug.Log("Player Died");
            OnPlayerDied?.Invoke();
        }
    }
}
