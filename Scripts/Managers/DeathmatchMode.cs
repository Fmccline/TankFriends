using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathmatchMode : MonoBehaviour, IGameMode {

    public float m_InvincibleDuration = 2.5f; // Default seconds to wait before tank can attack and take damage again
    private int[] m_TankKills;
    private int[] m_TankDeaths;

    public void StartRound(TankManager[] tanks)
    {
        foreach (var tank in tanks)
        {
            StartCoroutine(Respawn(tank));
        }
    }

    public TankManager GetRoundWinner(TankManager[] tanks)
    {
        TankManager winner = tanks[0];
        bool draw = false;
        for (int i = 1; i < tanks.Length; ++i)
        {  
            if (tanks[i].m_Score > winner.m_Score)
            {
                winner = tanks[i];
                draw = false;
            }
            else if (tanks[i].m_Score == winner.m_Score)
            {
                draw = true;
            }
        }
        return draw ? null : winner;
    }

    public bool IsEndOfRound(TankManager[] tanks)
    {
        SetScore(tanks);
        EnableTanks(tanks);
        return false;
    }

    private void SetScore(TankManager[] tanks)
    {
        // Set kills and deaths if not correctly set yet
        if (m_TankDeaths == null || m_TankKills == null || 
            m_TankDeaths.Length != tanks.Length || m_TankKills.Length != tanks.Length)
        {
            m_TankKills = new int[tanks.Length];
            m_TankDeaths = new int[tanks.Length];
            for (int i = 0; i < tanks.Length; ++i)
            {
                m_TankKills[i] = tanks[i].M_Kills;
                m_TankDeaths[i] = tanks[i].M_Deaths;
            }
        }
        // Set scores
        for (int i = 0; i < tanks.Length; ++i)
        {
            if (m_TankKills[i] < tanks[i].M_Kills)
            {
                tanks[i].m_Score += tanks[i].M_Kills - m_TankKills[i];
                m_TankKills[i] = tanks[i].M_Kills;
            }
            if (m_TankDeaths[i] < tanks[i].M_Deaths)
            {
                tanks[i].m_Score -= tanks[i].M_Deaths - m_TankDeaths[i];
                m_TankKills[i] = tanks[i].M_Kills;
            }
            if (tanks[i].m_Score < 0)
                tanks[i].m_Score = 0;
        }
    }

    private void EnableTanks(TankManager[] tanks)
    {
        foreach (var tank in tanks)
        {
            if (tank.M_Kills - tank.M_Deaths <= 0)
            {
                tank.M_Kills = 0;
                tank.M_Deaths = 0;
            }
            tank.m_Score = tank.M_Kills - tank.M_Deaths;
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
}
