using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IZombieState
{
    private ZombieAIController controller;

    public AttackState(ZombieAIController zombie)
    {
        controller = zombie;
    }

    public void EnterState()
    {
        controller.StopMovement();
    }

    public void UpdateState()
    {
        if(!controller.ReachedDestination(false))
        {
            controller.ChangeToState(controller.FollowState);
        }
    }

    public void ExitState() { }

    public void GetHit() { }

    public void OnTriggerEnter(Collider col) { }

    public void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            controller.Attack();
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            controller.ChangeToState(controller.WanderState);
        }
    }
}
