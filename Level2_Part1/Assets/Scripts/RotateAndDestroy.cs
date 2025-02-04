using System.Collections;
using UnityEngine;

public class RotateAndDestroy : MonoBehaviour
{
    public GameObject[] objectsToRotate; 
    public GameObject objectToDestroy;   
    public GameObject objRotate;
    private int rotatedObjects = 0; 
    [SerializeField] AudioClip openDoorClip;
    public void StartRotate()
    {
        if (Input.GetKeyDown(KeyCode.E)) {

            StartCoroutine(RotateObjectsOneByOne());
        }
        //StartCoroutine(RotateObjectsOneByOne());
    }

    IEnumerator RotateObjectsOneByOne()
    {
        foreach (GameObject obj in objectsToRotate)
        {
            yield return StartCoroutine(RotateObject(obj, 90f));
            rotatedObjects++;
        }

        if (rotatedObjects == objectsToRotate.Length)
        {
            AudioSource.PlayClipAtPoint(openDoorClip, transform.position, 2.5f);
            Destroy(objectToDestroy); 
        }
    }

    IEnumerator RotateObject(GameObject obj, float targetAngle)
    {
        float rotated = 0f;
        float rotationSpeed = 90f; 
        while (rotated < targetAngle)
        {
            float step = rotationSpeed * Time.deltaTime;
            obj.transform.Rotate(Vector3.up, step);
            rotated += step;
            yield return null;
        }
    }

    private void Update()
    {
        StartRotate();
    }
}
