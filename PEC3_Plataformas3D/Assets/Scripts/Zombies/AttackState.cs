using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IAIState
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

    public void UpdateState() { }

    public void ExitState() { }

    public void GetHit() { }

    public void OnTriggerEnter() { }

    public void OnTriggerStay()
    {
        controller.LookAtPlayer();

        if (!controller.ReachedDestination(false))
        {
            controller.ChangeToState(controller.FollowState);
        }
        else if (controller.ShouldAttack)
        {
            controller.Attack();
        }
    }

    public void OnTriggerExit()
    {
        controller.ChangeToState(controller.WanderState);
    }
}
