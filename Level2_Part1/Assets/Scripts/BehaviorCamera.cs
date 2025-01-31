using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorCamera : MonoBehaviour
{
    public Camera orthoCamera;
    public Transform lookoutTarget;
    public GameObject lookoutPrefab;
    public float zoomOutSize = 10f;
    public float moveSpeed = 2f;
    public float zoomSpeed = 2f;
    public KeyCode lookoutKey = KeyCode.L;

    private float originalSize;
    private Vector3 originalPosition;
    private bool isLookingOut = false;

    void Start()
    {
        if (orthoCamera == null)
            orthoCamera = Camera.main;

        originalSize = orthoCamera.orthographicSize;
        originalPosition = orthoCamera.transform.position;
        lookoutPrefab.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(lookoutKey))
        {
            isLookingOut = !isLookingOut;
            lookoutPrefab.SetActive(isLookingOut);
        }

        if (isLookingOut)
        {
            orthoCamera.orthographicSize = Mathf.Lerp(orthoCamera.orthographicSize, zoomOutSize, Time.deltaTime * zoomSpeed);
            orthoCamera.transform.position = Vector3.Lerp(orthoCamera.transform.position, new Vector3(lookoutTarget.position.x, orthoCamera.transform.position.y, lookoutTarget.position.z), Time.deltaTime * moveSpeed);
        }
        else
        {
            orthoCamera.orthographicSize = Mathf.Lerp(orthoCamera.orthographicSize, originalSize, Time.deltaTime * zoomSpeed);
            orthoCamera.transform.position = Vector3.Lerp(orthoCamera.transform.position, originalPosition, Time.deltaTime * moveSpeed);
        }
    }
}
