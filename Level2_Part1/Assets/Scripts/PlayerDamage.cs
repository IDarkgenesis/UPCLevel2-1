using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] int weaponDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamage damagable = other.GetComponent<IDamage>();
            if(damagable != null)
            {
                damagable.Damage(weaponDamage);
            }
        }
    }
}
