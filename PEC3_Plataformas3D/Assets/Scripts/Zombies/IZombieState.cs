using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IZombieState
{
    void EnterState();
    void UpdateState();
    void ExitState();

    void OnTriggerEnter(Collider col);
    void OnTriggerStay(Collider col);
    void OnTriggerExit(Collider col);

    void GetHit();
}
