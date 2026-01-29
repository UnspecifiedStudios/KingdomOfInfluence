using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    None,
    Aggressive,
    Retaliative,
    Passive
}

public class EnemyNav : MonoBehaviour
{
    // inspector values
    [Header("Enemy FOV Settings")]
    public float fovRadius;
    [Range(0, 360)]
    public float fovAngle;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;

    [Header("Enemy Type Settings")]
    [Tooltip("Aggressive - Enemy attacks player when it sees the player or is attacked\n" +
             "Retaliative - Enemy attacks player when enemy gets attacked\n" +
             "Passive - Enemy won't attack player")]
    public EnemyType enemyType; 



    [Header("Enemy Nav Settings")]
    [Tooltip("The transform destination for the NavAgent to move to (Player's Capsule)")]
    public Transform targetDestination;

    [Tooltip("A radius that exists around the target the NavAgent navigates to")]
    public float radiusToReach = 5f; 

    [Tooltip("How far away from the radius the NavAgent accepts as arrived")]
    public float outerRadTolerance = 1f;

    [Tooltip("How far in the radius the NavAgent accepts as arrived")]
    public float innerRadTolerance = 2f;

    [Tooltip("How often the NavAgent will refresh where to move")]
    public float navDestRefreshTime = 0.5f;

    [Header("Enemy Stats")]
    public float maxHealth = 100;
    public float currentHealth = 100;

    // private values
    private NavMeshAgent agent;
    private float innerRadius;
    private float outerRadius;
    private bool currentlyNavigating = false;

    void Awake()
    {
        // calculate radii tolerances
        innerRadius = radiusToReach - innerRadTolerance;
        outerRadius = radiusToReach + outerRadTolerance;
    }
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // turn off steering rotation, allow manual rotation
        agent.updateRotation = false;
        StartCoroutine(FOVRoutine());
    }

    void Update()
    {   
        // if the enemy is passive OR if enemy cant see the player and is currently navigating
        if (enemyType == EnemyType.Passive || (!canSeePlayer && !agent.pathPending && agent.remainingDistance > agent.stoppingDistance))
        {
            RotateToNavDirection();
        }
        // otherwise, if the enemy can see the player and either is aggressive or is retaliative and has lost health 
        else if (canSeePlayer && (enemyType == EnemyType.Aggressive || enemyType == EnemyType.Retaliative && currentHealth < maxHealth))
        {
            // start navigating to the player (only call this once, use bool to prevent) 
            if (!currentlyNavigating)
            {
                StartCoroutine(PlayerNavRecalculate(navDestRefreshTime));
            }
            // rotate to face player
            Vector3 rotateDirection = targetDestination.position - transform.position;
            rotateDirection.y = 0f;  // remove y axis rotation
            if (rotateDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(rotateDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
            }
        }
           
    }

    public void NavToPlayerRadius()
    {
        Vector3 distanceToPlayer = targetDestination.position - transform.position;
        float sqrDistToPlayer = distanceToPlayer.sqrMagnitude;

        // if enemy is farther away
        if (sqrDistToPlayer > (outerRadius*outerRadius))
        {
            agent.stoppingDistance = radiusToReach;
            agent.SetDestination(targetDestination.position);
        }
        // if enemy is too close
        else if (sqrDistToPlayer < (innerRadius*innerRadius))
        {
            Vector3 awayMoveVector = targetDestination.position - transform.position;
            Vector3 directionVector = -awayMoveVector.normalized;
            Vector3 retreatPosition = transform.position + (directionVector * (radiusToReach / 2));

            agent.stoppingDistance = 0.1f;
            agent.SetDestination(retreatPosition);
        }
        
        StartCoroutine(PlayerNavRecalculate(navDestRefreshTime));
    }

    public IEnumerator PlayerNavRecalculate(float timeToWait)
    {
        currentlyNavigating = true;
        yield return new WaitForSeconds(timeToWait);
        if (canSeePlayer)
        {
            NavToPlayerRadius();
        }
        else
        {
            currentlyNavigating = false;
        }
        
    }

    IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        // TODO: make this not an infinite loop.. maybe check if enemy is like.. awake?? 
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, fovRadius, targetMask);
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < fovAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

    private void RotateToNavDirection()
    {
        // rotate towards movement 
        Vector3 rotateDirection = agent.velocity;
        rotateDirection.y = 0f;
        if (rotateDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rotateDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }
}
