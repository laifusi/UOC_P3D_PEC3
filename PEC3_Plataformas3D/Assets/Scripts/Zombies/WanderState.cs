using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : IZombieState
{
    private ZombieAIController controller;

    public WanderState(ZombieAIController zombie)
    {
        controller = zombie;
    }

    public void EnterState()
    {
        controller.RandomizeDestination();
    }

    public void UpdateState()
    {
        if(controller.ReachedDestination(true))
        {
            controller.RandomizeDestination();
        }
    }

    public void ExitState(){ }

    public void GetHit()
    {
        controller.ChangeToState(controller.FollowState);
    }

    public void OnTriggerEnter(Collider col)
    {
        controller.ChangeToState(controller.FollowState);
    }

    public void OnTriggerStay(Collider col)
    {
        controller.ChangeToState(controller.FollowState);
    }

    public void OnTriggerExit(Collider col){ }
}
