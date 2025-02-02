using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventCollision : MonoBehaviour
{
    public UnityEvent unityEvent;
    public GameObject theTrigger;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == theTrigger) unityEvent.Invoke();
    }
}
