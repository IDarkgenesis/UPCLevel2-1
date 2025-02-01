using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
enum ENEMYTYPE { ARCHER, MELEE}
public class FollowPlayer : MonoBehaviour, IDamage
{
    [SerializeField] private Player target;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float attackRange = 2f; 
    [SerializeField] private float shootRange = 8f;   
    [SerializeField] private float tackleSpeed = 5f;
    [SerializeField] private GameObject projectilePrefab; 
    [SerializeField] private Transform shootPoint;  
    [SerializeField] private float shootCooldown = 1.5f; 
    [SerializeField] private ENEMYTYPE type;
    [SerializeField] private float enemyHealthPoints;
    [SerializeField] private float currentHealthPoints;
    [SerializeField] private ParticleSystem dustParticles;

    private Vector3 originalScale;
    private bool canShoot = true;
    private bool isTackling = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<Player>();
        enemyHealthPoints = currentHealthPoints;
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isTackling) return; 

        float distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToPlayer > attackRange)
        {
            agent.destination = target.transform.position;
        }

        if (distanceToPlayer <= attackRange && !isTackling && type == ENEMYTYPE.MELEE)
        {
            StartCoroutine(AttackMelee()); 
        }
        else if (distanceToPlayer <= shootRange && canShoot && type == ENEMYTYPE.ARCHER)
        {
            StartCoroutine(ShootProjectile()); 
        }

        if(currentHealthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator AttackMelee()
    {
        isTackling = true;
        agent.isStopped = true;

        // Squash antes del ataque
        transform.DOScale(new Vector3(originalScale.x * 1.6f, originalScale.y * 0.8f, originalScale.z * 0.5f), 0.2f)
            .OnComplete(() =>
            {
                transform.DOScale(originalScale, 0.1f);
            });

        yield return new WaitForSeconds(0.3f);

        if (dustParticles != null)
        {
            Instantiate(dustParticles, transform.position, Quaternion.identity);
        }

        float tackleDuration = 0.5f;
        float elapsedTime = 0f;
        Vector3 tackleDirection = (target.transform.position - transform.position).normalized;

        while (elapsedTime < tackleDuration)
        {
            target.GetComponent<CharacterController>().Move(tackleDirection * tackleSpeed * Time.deltaTime);
            IDamage damagable = target.GetComponent<IDamage>();
            if (damagable != null)
            {
                damagable.Damage(5);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        agent.isStopped = false;
        isTackling = false;
    }

    IEnumerator ShootProjectile()
    {
        canShoot = false;

        // Squash antes de disparar
        transform.DOScale(new Vector3(originalScale.x * 1.2f, originalScale.y * 0.8f, originalScale.z * 1.2f), 0.2f)
            .OnComplete(() =>
            {
                transform.DOScale(originalScale, 0.1f);
            });

        yield return new WaitForSeconds(0.2f);

        Vector3 direction = (target.transform.position - shootPoint.position).normalized;

      
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.LookRotation(direction));

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * 10f;
        }

        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    public void Damage(float damage)
    {
        currentHealthPoints = enemyHealthPoints - damage;
        enemyHealthPoints = currentHealthPoints;

    }
}
