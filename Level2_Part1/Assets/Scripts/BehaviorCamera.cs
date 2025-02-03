using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorCamera : MonoBehaviour
{
    public Camera orthoCamera;
    public Transform lookoutTarget;
    public GameObject lookoutPrefab;
    public float zoomOutSize = 10f;
    [SerializeField] float zoomInSize = 10f;
    public float moveSpeed = 2f;
    public float zoomSpeed = 2f;
    public KeyCode lookoutKey = KeyCode.L;

    [SerializeField]private float originalSize;
    private Vector3 originalPosition;
    private bool isLookingOut = false;

    void Start()
    {
        if (orthoCamera == null)
            orthoCamera = Camera.main;

        originalSize = orthoCamera.orthographicSize;
        originalPosition = orthoCamera.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isLookingOut=true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isLookingOut = false;
            StartCoroutine(SmoothZoomIn());  
        }

    }

    void Update()
    {
       

        if (isLookingOut)
        {
            orthoCamera.orthographicSize = Mathf.Lerp(orthoCamera.orthographicSize, zoomOutSize, Time.deltaTime * zoomSpeed);
            orthoCamera.transform.position = Vector3.Lerp(orthoCamera.transform.position, new Vector3(lookoutPrefab.transform.position.x, orthoCamera.transform.position.y, lookoutPrefab.transform.position.z), Time.deltaTime * moveSpeed);
            orthoCamera.nearClipPlane = -100;
        }
       

    }

    IEnumerator SmoothZoomIn()
    {
        float elapsedTime = 0f;
        float startSize = orthoCamera.orthographicSize;

        while (elapsedTime < 1f)
        {
            orthoCamera.orthographicSize = Mathf.Lerp(startSize, zoomInSize, elapsedTime * zoomSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        orthoCamera.orthographicSize = zoomInSize;
    }
}
