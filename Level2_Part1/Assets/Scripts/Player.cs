using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamage
{
    [SerializeField] float playerSpeed = 5f;
    [SerializeField] float healthPoints = 1000000;
    [SerializeField] float currentHealth;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float dashForce = 3f;
    [SerializeField] float gravity = -30f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] float acceleration = 15f;
    [SerializeField] float friction = 2f;
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    bool isMoving;
    bool isAttacking;
    float maxFallSpeed = -50f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        currentHealth = healthPoints;
    }

    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        bool dashButton = gameInput.GetDash();
        isAttacking = gameInput.GetAttack();

        if (moveDir != Vector3.zero)
        {
            playerSpeed = Mathf.Min(maxSpeed, playerSpeed + acceleration * Time.deltaTime);
        }
        else
        {
            
            playerSpeed = Mathf.Max(0, playerSpeed - friction * Time.deltaTime);
        }

        Vector3 move = moveDir * playerSpeed;
        controller.Move(move * Time.deltaTime);

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

       
        velocity.y += gravity * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, maxFallSpeed);

        controller.Move(velocity * Time.deltaTime);
        isMoving = moveDir != Vector3.zero;

        if (moveDir != Vector3.zero)
        {
            gameObject.transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);
        }


        if (dashButton && moveDir != Vector3.zero)
        {
            controller.Move(moveDir * dashForce);
        }

      
          
        
    }

    public bool IsWalking()
    {
        return isMoving;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
    

    public void Damage(float damage)
    {
         currentHealth = healthPoints - damage;
        healthPoints = currentHealth;
       
    }
}
