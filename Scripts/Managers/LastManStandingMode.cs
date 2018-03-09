using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LastManStandingMode : MonoBehaviour, IGameMode
{
    public TankManager GetRoundWinner(TankManager[] tanks)
    {
        TankManager winner = null;
        foreach (var tank in tanks)
        {
            if (tank.m_Instance.activeSelf && winner == null)
            {
                winner = tank;
            }
            else if (winner != null)
            {
                return null;
            }
        }
        winner.m_Score++;
        return winner;
    }

    public bool IsEndOfRound(TankManager[] tanks)
    {
        return OneTankLeft(tanks);
    }

    private bool OneTankLeft(TankManager[] tanks)
    {
        int numTanksLeft = 0;

        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }
}
