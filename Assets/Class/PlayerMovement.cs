using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float movespeed = 5f;
    private Vector3 movement;
    public PlayerInput playerInput;

    int[] scores = new int[5];

    void Start() 
    {
        for (int i = 0; i < scores.Length; i++) scores[i] = i*10;
        Debug.Log(scores[0]); Debug.Log(scores[1]); Debug.Log(scores[2]); Debug.Log(scores[3]); Debug.Log(scores[4]);
    }

    public void OnMove(InputAction.CallbackContext callback)
    {
        movement = callback.ReadValue<Vector2>();
    }
    private void Update()
    {
        //rb.MovePosition(movement * movespeed * Time.deltaTime);
        transform.position += movement * movespeed * Time.deltaTime; //s
    }
}
