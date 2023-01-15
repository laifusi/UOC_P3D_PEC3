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

    public void UpdateState()
    {
        if(controller.NoOneToAttack())
            controller.ChangeToState(controller.WanderState);
    }

    public void ExitState() { }

    public void GetHit() { }

    public void OnTriggerEnter() { }

    public void OnTriggerStay()
    {
        if (controller.FollowingPedestrian())
        {
            controller.ZombifyPedestrian();
            controller.ChangeToState(controller.WanderState);
        }
        else
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
    }

    public void OnTriggerExit()
    {
        controller.ChangeToState(controller.WanderState);
    }
}
