using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAIController : MonoBehaviour
{
    public WanderState WanderState { get; private set; }    // Wander state
    public FollowState FollowState { get; private set; }    // Follow state
    public AttackState AttackState { get; private set; }    // AttackState state

    private IZombieState currentState;  // IZombieState for the current state
    private NavMeshAgent navMeshAgent;  // NavMeshAgent component

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        WanderState = new WanderState(this);
        FollowState = new FollowState(this);
        AttackState = new AttackState(this);

        ChangeToState(WanderState);
    }

    public void ChangeToState(IZombieState state)
    {
        currentState.ExitState();
        currentState = state;
        currentState.EnterState();
    }

    public void StopMovement()
    {
        throw new NotImplementedException();
    }

    public void SetFollowDestination()
    {
        throw new NotImplementedException();
    }

    public void RandomizeDestination()
    {
        throw new NotImplementedException();
    }

    public bool ShouldRecalculateDestination()
    {
        throw new NotImplementedException();
    }

    public void Attack()
    {
        throw new NotImplementedException();
    }

    public bool ReachedDestination(bool isWander)
    {
        throw new NotImplementedException();
    }
}
