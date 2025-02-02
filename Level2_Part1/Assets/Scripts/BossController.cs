using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BossController : MonoBehaviour, IDamage
{
    [SerializeField] private AudioClip damageSoundClip;

    [SerializeField] private float strikeRange = 8f;
    [SerializeField] private float maximumHealthPoints = 10;
    [SerializeField] private float currentHealthPoints = 10;

    [SerializeField] private float dashTrackingDuration = 2f;
    [SerializeField] private float baseSpeed = 3.5f;
    [SerializeField] private float dashSpeed = 25f;

    [SerializeField] private float strikeTrackingDuration = 3f;
    [SerializeField] private float strikeSpeed = 15f;
    [SerializeField] private float dashRange = 15f; 
    
    private Player player;
    private NavMeshAgent agent;
    private Vector3 currentTarget;
    private bool canAttack = true;
    private float distanceToPlayer = 0;
    private State currentState = State.Idle;

    MeshRenderer slashMeshRenderer;
    BoxCollider slashCollider;

    MeshRenderer dashXZRenderer;
    MeshRenderer dashYZRenderer;
    BoxCollider dashCollider;
    enum State
    {
        Idle,
        Dashing,
        Striking
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        agent = GetComponent<NavMeshAgent>();
        currentHealthPoints = maximumHealthPoints;
        currentState = State.Idle;

        GameObject slashAttackCObject = gameObject.transform.Find("SlashAttack").gameObject;
        slashMeshRenderer = slashAttackCObject.GetComponent<MeshRenderer>();
        slashMeshRenderer.enabled = false;

        slashCollider = slashAttackCObject.GetComponent<BoxCollider>();
        slashCollider.enabled = false;

        dashXZRenderer = gameObject.transform.Find("DashSpriteXZ").gameObject.GetComponent<MeshRenderer>();
        dashYZRenderer = gameObject.transform.Find("DashSpriteYZ").gameObject.GetComponent<MeshRenderer>();
        dashCollider = gameObject.transform.Find("DashHitbox").gameObject.GetComponent<BoxCollider>();

        dashXZRenderer.enabled = false;
        dashYZRenderer.enabled = false;
        dashCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && (currentState == State.Dashing || currentState == State.Striking))
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    currentState = State.Idle;
                    Invoke(nameof(ResetCanAttack), 2f);
                    agent.speed = baseSpeed;

                    dashXZRenderer.enabled = false;
                    dashYZRenderer.enabled = false;
                    dashCollider.enabled = false;
                }
            }
        }

        if (currentState == State.Idle && canAttack)
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer <= strikeRange)
            {
                currentState = State.Striking;
               
                StartCoroutine(StrikeAttack());
            }
            else if (distanceToPlayer <= dashRange)
            {
                currentState = State.Dashing;
                StartCoroutine(DashAttack());
            }
        }
        else if (currentState == State.Idle && !canAttack) FacePlayerSmooth();

        if (currentHealthPoints <= 0) Destroy(gameObject);
    }
    private void FaceTarget()
    {
        Vector3 direction = (currentTarget - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
        transform.rotation = lookRotation;
    }

    private void FacePlayerSmooth()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, strikeRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, dashRange);
    }

    IEnumerator DashAttack()
    {
        canAttack = false;

        float elapsedTime = 0f;

        // Time spent "tracking" the player
        while (elapsedTime < dashTrackingDuration)
        {
            FacePlayer();
            elapsedTime += Time.deltaTime;
        }

        currentTarget = player.transform.position;
        agent.speed = dashSpeed;
        FaceTarget();
        agent.SetDestination(currentTarget);

        dashXZRenderer.enabled = true;
        dashYZRenderer.enabled = true;
        dashCollider.enabled = true;

        yield return null;
    }

    IEnumerator StrikeAttack()
    {
        canAttack = false;

        float elapsedTime = 0f;

        while (elapsedTime < strikeTrackingDuration) elapsedTime += Time.deltaTime;

        currentTarget = player.transform.position;
        agent.speed = strikeSpeed;
        FaceTarget();
        agent.SetDestination(currentTarget);

        yield return new WaitForSeconds(0.15f);

        slashCollider.enabled = true;
        slashMeshRenderer.enabled = true;

        yield return new WaitForSeconds(0.15f);

        slashCollider.enabled = false;
        slashMeshRenderer.enabled = false;

        yield return new WaitForSeconds(0.15f);

        slashCollider.enabled = true;
        slashMeshRenderer.enabled = true;

        yield return new WaitForSeconds(0.15f);

        slashCollider.enabled = false;
        slashMeshRenderer.enabled = false;

        yield return null;
    }

    void ResetCanAttack()
    {
        canAttack = true;
    }

    public void Damage(float damage)
    {
        AudioSource.PlayClipAtPoint(damageSoundClip, transform.position, 1f);
        currentHealthPoints -= damage;
    }
}
