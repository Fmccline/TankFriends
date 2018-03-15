using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanTankInput : ITankInput
{
    public float GetMovementInput(string axisName)
    {
        return Input.GetAxis(axisName);
    }

    public float GetTurnInput(string axisName)
    {
        return Input.GetAxis(axisName);
    }

    public bool IsChargingShot(string button)
    {
        return Input.GetButtonDown(button) || Input.GetButton(button);
    }

    public bool IsFiringShot(string button)
    {
        return Input.GetButtonUp(button);
    }
}
