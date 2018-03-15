using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbTankInput : ITankInput
{
    private float angularSpeed = 0f;
    private UnityEngine.Random acceleration = new UnityEngine.Random();
    public float GetMovementInput(string axisName)
    {
        return 1.0f;
    }

    public float GetTurnInput(string axisName)
    {
        if (angularSpeed > 1)
            angularSpeed = (UnityEngine.Random.value - 0.5f) * 2f;
        else if (angularSpeed < -1)
            angularSpeed = (UnityEngine.Random.value - 0.5f) * 2f;
        else
            angularSpeed += UnityEngine.Random.value - 0.5f;

        return angularSpeed;
    }

    public bool IsChargingShot(string button)
    {
        return true;
    }

    public bool IsFiringShot(string button)
    {
        return false;
    }
}
