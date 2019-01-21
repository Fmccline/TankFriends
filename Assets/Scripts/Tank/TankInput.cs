using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITankInput
{
    float GetMovementInput(TankMovement tankMovement);
    float GetTurnInput(TankMovement tankMovement);
    bool IsChargingShot(TankShooting tankShooting);
    bool IsFiringShot(TankShooting tankShooting);
}
