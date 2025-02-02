using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour, IDamage
{

    [SerializeField] private float strikeRange = 5f;
    [SerializeField] private float dashRange = 15f;
    [SerializeField] private float distanceToPlayer = 0;
    [SerializeField] private float maximumHealthPoints = 10;
    [SerializeField] private float currentHealthPoints = 10;
    [SerializeField] private State currentState = State.Idle;
    [SerializeField] private float dashTrackingDuration = 2f;
    [SerializeField] private float baseSpeed = 3.5f;
    [SerializeField] private float dashSpeed = 10f;

    private Player player;
    private NavMeshAgent agent;
    private Vector3 currentTarget;
    private bool canAttack = true;
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
                }
            }
        }

        if (currentState == State.Idle && canAttack)
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer <= strikeRange)
            {

            }
            else if (distanceToPlayer <= dashRange)
            {
                currentState = State.Dashing;
                StartCoroutine(DashAttack());
            }
        }
        else if (currentState == State.Idle && !canAttack) FaceTarget();

        if (currentHealthPoints <= 0) Destroy(gameObject);
    }
    private void FaceTarget()
    {
        Vector3 direction = (currentTarget - transform.position).normalized;
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

        while (elapsedTime < dashTrackingDuration)
        {
            FaceTarget();
            elapsedTime += Time.deltaTime;
        }

        FaceTarget();
        currentTarget = player.transform.position;
        agent.speed = dashSpeed;
        agent.SetDestination(currentTarget);

        yield return null;
    }

    void ResetCanAttack()
    {
        canAttack = true;
    }

    public void Damage(float damage)
    {
        currentHealthPoints -= damage;
    }
}
