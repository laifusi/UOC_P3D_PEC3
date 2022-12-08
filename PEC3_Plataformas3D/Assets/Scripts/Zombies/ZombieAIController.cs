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

    public WanderState WanderState { get; private set; }    // Wander state
    public FollowState FollowState { get; private set; }    // Follow state
    public AttackState AttackState { get; private set; }    // AttackState state

    private IZombieState currentState;  // IZombieState for the current state
    private NavMeshAgent navMeshAgent;  // NavMeshAgent component
    private Transform player;
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

    public void RandomizeDestination()
    {
        animator.SetBool("Walking", true);
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
            return navMeshAgent.remainingDistance <= attackDistance;
        }
    }

    public void SetFollowDestination()
    {
        timeSinceFollowUpdate = 0;
        navMeshAgent.SetDestination(player.position);
    }

    public bool ShouldRecalculateDestination()
    {
        return timeSinceFollowUpdate >= timeBetweenFollowRecalculation;
    }

    public void StopMovement()
    {
        animator.SetBool("Walking", false);
        navMeshAgent.isStopped = true;
    }

    public void EnableAttack(int canAttack)
    {
        this.canAttack = canAttack == 0 ? false : true;
    }

    public bool ShouldAttack => canAttack;

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.transform;
            currentState.OnTriggerEnter(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentState.OnTriggerStay(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            currentState.OnTriggerExit(other);
        }
    }
}
