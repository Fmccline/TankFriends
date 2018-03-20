using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanTankInput : ITankInput
{
    public float GetMovementInput(TankMovement tankMovement)
    {
        return Input.GetAxis(tankMovement.m_MovementAxisName);
    }

    public float GetTurnInput(TankMovement tankMovement)
    {
        return Input.GetAxis(tankMovement.m_TurnAxisName);
    }

    public bool IsChargingShot(string button)
    {
        return Input.GetButtonDown(button) || Input.GetButton(button);
    }

    public bool IsChargingShot(TankShooting tankShooting)
    {
        return Input.GetButtonDown(tankShooting.m_FireButton) || Input.GetButton(tankShooting.m_FireButton);
    }

    public bool IsFiringShot(TankShooting tankShooting)
    {
        return Input.GetButtonUp(tankShooting.m_FireButton);
    }
}
