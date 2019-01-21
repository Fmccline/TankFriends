using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LastManStandingMode : MonoBehaviour, IGameMode
{
    public float m_InvincibleDuration = 2.5f;

    public void StartRound(TankManager[] tanks)
    {
        foreach (var tank in tanks)
        {
            tank.m_Score = tank.m_RoundScore;
            StartCoroutine(Respawn(tank));
        }
    }

    public TankManager GetRoundWinner(TankManager[] tanks)
    {
        TankManager winner = null;
        foreach (var tank in tanks)
        {
            if (tank.m_Instance.activeSelf && winner == null)
            {
                winner = tank;
            }
            else if (tank.m_Instance.activeSelf && winner != null)
            {
                return null;
            }
        }
        return winner;
    }

    // returns true if <= 1 tanks left
    public bool IsEndOfRound(TankManager[] tanks)
    {
        int numTanksLeft = 0;

        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }

    private IEnumerator Respawn(TankManager tank)
    {
        tank.Respawn();
        tank.EnableInvincible();
        float frequency = 0.1f;
        for (float t = 0f; t < m_InvincibleDuration; t += frequency)
        {
            yield return new WaitForSeconds(frequency);
            tank.CycleInvincibleColor();
        }
        tank.DisableInvincible();
    }
}
