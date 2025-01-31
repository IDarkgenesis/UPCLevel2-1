using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float playerSpeed;
    [SerializeField] float dashDistance;
    [SerializeField] float gravity = -9.8f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField]private GameInput gameInput;
    [SerializeField] float acceleration;
    [SerializeField] float maxAcceleration;
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    bool isMoving;
  
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
       
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        bool dashButton = gameInput.GetDash();
       
        Vector3 move = moveDir * playerSpeed;
        controller.Move(move * Time.deltaTime);
       
        if (playerSpeed < maxAcceleration)
        {
            playerSpeed += acceleration * Time.deltaTime;
        }
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

       
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        isMoving = moveDir != Vector3.zero;
        if (moveDir != Vector3.zero)
        {
            gameObject.transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed);
        }

        if (dashButton && moveDir != Vector3.zero) 
        {
            controller.Move(moveDir * dashDistance); 
        }
    }

    public bool IsWalking()
    {
        return isMoving;
    }
}
