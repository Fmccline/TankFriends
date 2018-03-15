using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITankInput
{
    float GetMovementInput(string axisName);
    float GetTurnInput(string axisName);
    bool IsChargingShot(string button);
    bool IsFiringShot(string button);
    // is 
    // is charging
    // is firing
}
