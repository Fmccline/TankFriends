using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankInputFactory
{
    public static ITankInput MakeTankWithInput(TankInput.Type type)
    {
        if (type == TankInput.Type.Human)
        {
            return new HumanTankInput();
        }
        else if (type == TankInput.Type.DumbAI)
        {
            return new DumbTankInput();
        }
        else
        {
            throw new System.ArgumentException("Invalid tank type!");
        }
    }
}