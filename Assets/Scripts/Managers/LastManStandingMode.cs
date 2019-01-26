using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LastManStandingMode : MonoBehaviour, IGameMode
{
    public float m_InvincibleDuration = 2.5f;

    public void StartRound(TankManager[] tanks)
    {
        foreach (var tank in tanks)
        {
            StartCoroutine(Respawn(tank));
        }
    }

    public List<float> GetRoundScores(TankManager[] tanks)
    {
        List<float> scores = new List<float>();
        foreach (var tank in tanks)
        {
            Score tankScore = tank.m_TankScore;
            int score = tankScore.GetWins();
            scores.Add(score);
        }
        return scores;
    }

    public TankManager GetRoundWinner(TankManager[] tanks)
    {
        TankManager winner = null;
        foreach (var tank in tanks)
        {
            if (tank.m_Instance.activeSelf)
            {
                if (winner != null)
                {
                    return null;
                }
                else
                {
                    winner = tank;
                }
            }
        }
        return winner;
    }

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
