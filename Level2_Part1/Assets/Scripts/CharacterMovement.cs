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

    public float speed = 3.0F;
    public float rotateSpeed = 3.0F;

    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();

       
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //Vector3 forward = transform.TransformDirection(Vector3.forward);
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
        controller.SimpleMove(move * playerSpeed);

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Debug.Log("Press");
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        //FIX DASH TODO 
        float startTime = Time.time;
        while (startTime + dashTime > Time.time)
        {
            Vector3 moveDirection = transform.forward * dashMeters * playerSpeed;
            controller.SimpleMove(moveDirection);
            yield break;
        }
    }
}
