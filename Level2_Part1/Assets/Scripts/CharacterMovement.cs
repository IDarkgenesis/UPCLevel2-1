using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]private float playerSpeed = 2.0f;
    [SerializeField]private float dashMeters = 3.0f;
    [SerializeField] private float dashTime;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Makes the Dash
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dash());
        }
       
    }

    private IEnumerator Dash()
    {
        float startTime = Time.time;
        while (startTime + dashTime > Time.time)
        {
            Vector3 moveDirection = transform.forward * dashMeters;
            controller.Move(moveDirection * Time.deltaTime * playerSpeed);
            yield break;
        }
    }
}
