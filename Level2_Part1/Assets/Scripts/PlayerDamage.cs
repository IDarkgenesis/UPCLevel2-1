using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] int weaponDamage;
    [SerializeField] private BoxCollider box;
    [SerializeField] private float attackDuration = 0.2f; // Duración del ataque

    private void Start()
    {
        box.enabled = false; // Desactivamos el trigger al inicio
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamage damagable = other.GetComponent<IDamage>();
            if (damagable != null)
            {
                damagable.Damage(weaponDamage);
            }
        }
    }

    public void ActivateWeapon()
    {
        box.enabled = true;
        Invoke("DeactivateWeapon", attackDuration); // Desactiva después del tiempo definido
    }

    private void DeactivateWeapon()
    {
        box.enabled = false;
    }
}
