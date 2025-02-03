using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour, IDamage
{

    [SerializeField] private AudioClip destroyObjectClip;

    public void Damage(float damage)
    {
        AudioSource.PlayClipAtPoint(destroyObjectClip, transform.position, 1f);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
