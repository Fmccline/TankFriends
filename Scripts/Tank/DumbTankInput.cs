using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbTankInput : ITankInput
{
    private float angularSpeed = 0f;

    public float GetMovementInput(TankMovement tankMovement)
    {
        return 0.8f;
    }

    private GameObject GetClosestTank(GameObject[] tanks, GameObject myTank)
    {
        var closestTank = tanks[0];
        var distanceToClosestTank = Vector3.Distance(closestTank.transform.position, myTank.transform.position);
        for (int i = 1; i < tanks.Length; ++i)
        {
            if (tanks[i] == myTank)
                continue;

            var distanceToTank = Vector3.Distance(tanks[i].transform.position, myTank.transform.position);
            if (distanceToTank < distanceToClosestTank)
            {
                distanceToClosestTank = distanceToTank;
                closestTank = tanks[i];
            }
        }
        return closestTank;
    }

    private float GetAngleBetweenTanks(GameObject myTank, GameObject targetTank)
    {
        Vector2 targetPostion = new Vector2(targetTank.transform.position.x, targetTank.transform.position.z);
        Vector2 myPosition = new Vector2(myTank.transform.position.x, myTank.transform.position.z);
        Vector2 myDirection = new Vector2(myTank.transform.forward.x, myTank.transform.forward.z).normalized;

        Vector2 directionToOpponent = new Vector2(targetPostion.x - myPosition.x, targetPostion.y - myPosition.y).normalized;
        var angleBetweenTanks = Vector2.SignedAngle(myDirection, directionToOpponent);
        Debug.Log("Angle between tanks: " + angleBetweenTanks.ToString());
        return angleBetweenTanks;
    }

    public float GetTurnInput(TankMovement tankMovement)
    {
        var tanks = GameObject.FindGameObjectsWithTag("Player");
        if (tanks.Length <= 1)
            return 0f;
        
        var myTank = tankMovement.gameObject;
        var targetTank = GetClosestTank(tanks, myTank);
        var angleBetweenTanks = GetAngleBetweenTanks(myTank, targetTank);

        if (angleBetweenTanks > 40f && angleBetweenTanks > 0)
            return -1.0f;
        else if (angleBetweenTanks > 20f && angleBetweenTanks > 0)
            return -0.5f;
        else if (angleBetweenTanks > 0f)
            return -0.1f;
        else if (angleBetweenTanks > -20f)
            return 0.1f;
        else if (angleBetweenTanks > -40f)
            return 0.5f;
        else 
            return 1.0f;
    }

    public bool IsChargingShot(TankShooting tankShooting)
    {
        return true;
    }

    public bool IsFiringShot(TankShooting tankShooting)
    {
        return false;
    }
}
