using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathmatchMode : MonoBehaviour, IGameMode {

    public float m_SpawnDelay = 2.0f;
    private TankManager[] m_Tanks;

    public TankManager GetRoundWinner(TankManager[] tanks)
    {
        TankManager winner = m_Tanks[0];
        bool draw = false;
        for (int i = 1; i < m_Tanks.Length; ++i)
        {  
            if (m_Tanks[i].m_Score > winner.m_Score)
            {
                winner = m_Tanks[i];
                draw = false;
            }
            else if (m_Tanks[i].m_Score == winner.m_Score)
            {
                draw = true;
            }
        }
        return draw ? null : winner;
    }

    public bool IsEndOfRound(TankManager[] tanks)
    {
        EnableTanks(tanks);
        return false;
    }

    private void EnableTanks(TankManager[] tanks)
    {
        foreach (var tank in tanks)
        {
            if (tank.m_EnemiesKilled - tank.m_Deaths <= 0)
            {
                tank.m_EnemiesKilled = 0;
                tank.m_Deaths = 0;
            }
            tank.m_Score = tank.m_EnemiesKilled - tank.m_Deaths;
            if (tank.m_Instance.activeSelf == false)
            {
                StartCoroutine(Respawn(tank));
            }
        }
    }

    private IEnumerator Respawn(TankManager tank)
    {
        if (tank.m_Score > 0)
            tank.m_Score--;

        tank.Respawn();
        yield return new WaitForSeconds(m_SpawnDelay);
        tank.EnableInvincible();
        yield return new WaitForSeconds(m_SpawnDelay);
        tank.DisableInvincible();
    }
}
