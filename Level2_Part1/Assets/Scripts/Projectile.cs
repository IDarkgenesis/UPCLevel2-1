using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 1f;

    private void Start()
    {
        Destroy(gameObject, lifetime); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            IDamage damagable = other.GetComponent<IDamage>();
            if (damagable != null)
            {
                damagable.Damage(damage);
            }
            Destroy(gameObject,0.1f); 
        }
    }
}
