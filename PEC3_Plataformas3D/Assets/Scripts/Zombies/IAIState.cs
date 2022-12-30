using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIState
{
    void EnterState();
    void UpdateState();
    void ExitState();

    void OnTriggerEnter();
    void OnTriggerStay();
    void OnTriggerExit();

    void GetHit();
}
