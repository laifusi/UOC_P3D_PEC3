using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAIController : MonoBehaviour
{
    [SerializeField] private float attackDistance;
    [SerializeField] private float timeBetweenFollowRecalculation = 1;
    [SerializeField] private float wanderRadius = 5;
    [SerializeField] private float moveSpeed = 0.2f;
    [SerializeField] private float runSpeed = 3f;

    public WanderState WanderState { get; private set; }    // Wander state
    public FollowState FollowState { get; private set; }    // Follow state
    public AttackState AttackState { get; private set; }    // AttackState state

    private IZombieState currentState;  // IZombieState for the current state
    private NavMeshAgent navMeshAgent;  // NavMeshAgent component
    private Vector3 playerPos;
    private float timeSinceFollowUpdate;
    private Animator animator;
    private bool canAttack = true;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        WanderState = new WanderState(this);
        FollowState = new FollowState(this);
        AttackState = new AttackState(this);

        ChangeToState(WanderState);
    }

    private void Update()
    {
        timeSinceFollowUpdate += Time.deltaTime;

        currentState?.UpdateState();
    }

    public void ChangeToState(IZombieState state)
    {
        currentState?.ExitState();
        currentState = state;
        currentState?.EnterState();
    }

    public void StartMove()
    {
        animator.SetBool("Running", false);
        animator.SetBool("Walking", true);
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = moveSpeed;
    }

    public void StartRun()
    {
        animator.SetBool("Running", true);
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = runSpeed;
    }

    public void RandomizeDestination()
    {
        float randomX = transform.position.x + UnityEngine.Random.Range(-wanderRadius, wanderRadius);
        float randomZ = transform.position.z + UnityEngine.Random.Range(-wanderRadius, wanderRadius);
        Vector3 randomPosition = new Vector3(randomX, wanderRadius, randomZ);

        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPosition, out hit, wanderRadius, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }
    }

    public bool ReachedDestination(bool isWander)
    {
        if (isWander)
        {
            return navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
        }
        else
        {
            return Vector3.Distance(playerPos, transform.position) <= attackDistance;
        }
    }

    public void SetFollowDestination()
    {
        timeSinceFollowUpdate = 0;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(playerPos, out hit, wanderRadius, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }
    }

    public bool ShouldRecalculateDestination()
    {
        return timeSinceFollowUpdate >= timeBetweenFollowRecalculation;
    }

    public void StopMovement()
    {
        animator.SetBool("Running", false);
        animator.SetBool("Walking", false);
        navMeshAgent.SetDestination(transform.position);
        navMeshAgent.isStopped = true;
    }

    public bool ShouldAttack => canAttack;

    public void EnableAttack(int canAttack)
    {
        this.canAttack = canAttack == 0 ? false : true;
    }

    public void LookAtPlayer()
    {
        Vector3 lookDirection = playerPos - transform.position;

        transform.rotation = Quaternion.FromToRotation(Vector3.forward, new Vector3(lookDirection.x, 0, lookDirection.z));
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerPos = other.transform.position;
            currentState.OnTriggerEnter();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPos = other.transform.position;
            currentState.OnTriggerStay();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPos = Vector3.zero;
            currentState.OnTriggerExit();
        }
    }
}
