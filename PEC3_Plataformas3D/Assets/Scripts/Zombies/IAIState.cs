using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
