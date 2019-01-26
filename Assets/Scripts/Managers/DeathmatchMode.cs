using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathmatchMode : MonoBehaviour, IGameMode {

    public float m_InvincibleDuration = 2.5f; // Default seconds to wait before tank can attack and take damage again

    public void StartRound(TankManager[] tanks)
    {
        foreach (var tank in tanks)
        {
            StartCoroutine(Respawn(tank));
        }
    }

    public TankManager GetRoundWinner(TankManager[] tanks)
    {
        bool draw = false;
        int mostKills = 0;
        TankManager winner = null;
        foreach (var tank in tanks)
        {
            Score score = tank.m_TankScore;
            if (score.GetKills() > mostKills)
            {
                draw = false;
                winner = tank;
            }
            else if (score.GetKills() == mostKills)
            {
                draw = true;
            }
        }
        return draw ? null : winner;
    }

    public bool IsEndOfRound(TankManager[] tanks)
    {
        RespawnTanks(tanks);
        return false;
    }

    private void RespawnTanks(TankManager[] tanks)
    {
        foreach (var tank in tanks)
        {
            if (tank.m_Instance.activeSelf == false)
            {
                StartCoroutine(Respawn(tank));
            }
        }
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

    public List<float> GetRoundScores(TankManager[] tanks)
    {
        List<float> scores = new List<float>();
        foreach (var tank in tanks)
        {
            Score tankScore = tank.m_TankScore;
            int kills = tankScore.GetKills();
            int deaths = tankScore.GetDeaths();
            int score = kills - deaths;
            scores.Add(score);
        }
        return scores;
    }
}
