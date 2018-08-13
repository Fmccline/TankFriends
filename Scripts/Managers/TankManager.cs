﻿using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class TankManager
{
    public Color m_PlayerColor;
    public Vector3 m_SpawnPosition;
    public Quaternion m_SpawnRotation;
    //public Transform m_SpawnPoint; 
    [HideInInspector] public int m_PlayerNumber;             
    [HideInInspector] public string m_ColoredPlayerText;
    [HideInInspector] public GameObject m_Instance;
    [HideInInspector] public int M_Kills { get { return m_Shooting.m_Kills; } set { m_Shooting.m_Kills = value; } }
    [HideInInspector] public int M_Deaths { get { return m_Health.m_Deaths; } set { m_Health.m_Deaths = value; } }
    [HideInInspector] public int m_Score = 0;
    [HideInInspector] public int m_RoundScore = 0;
    public enum TankType { Human = 0, DumbAI = 1, SmartAI = 2 };
    [HideInInspector] public TankType m_TankUserType = TankType.Human;


    private ITankInput m_TankInput;
    private TankMovement m_Movement;       
    private TankShooting m_Shooting;
    private GameObject m_CanvasGameObject;
    private TankHealth m_Health;
    private Color m_InvincibleColor = new Color(255f/255f, 255f/255f, 102f/255f);
    private Color m_CurrentColor;

    public void Setup()
    {
        m_Movement = m_Instance.GetComponent<TankMovement>();
        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_Health = m_Instance.GetComponent<TankHealth>();

        if (m_TankUserType == TankType.Human)
        {
            m_TankInput = new HumanTankInput();
        }
        else if (m_TankUserType == TankType.DumbAI)
        {
            m_TankInput = new DumbTankInput();
        }
        else if (m_TankUserType == TankType.SmartAI)
        {
            var tankInput = m_Instance.AddComponent<SmartTankInput>() as SmartTankInput;
            m_TankInput = tankInput;
        }

        m_Movement.m_TankInput = m_TankInput;
        m_Shooting.m_TankInput = m_TankInput;

        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        m_Movement.m_PlayerNumber = m_PlayerNumber;
        m_Shooting.m_PlayerNumber = m_PlayerNumber;

        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }
        m_CurrentColor = m_PlayerColor;
    }

    public void DisableControl()
    {
        m_Health.m_CanTakeDamage = false;
        m_Movement.enabled = false;
        m_Shooting.enabled = false;
        m_CanvasGameObject.SetActive(false);
    }

    private void SetColor(Color c)
    {
        m_CurrentColor = c;
        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = c;
        }
    }

    public void EnableInvincible()
    {
        m_Health.m_CanTakeDamage = false;
        m_Movement.enabled = true;
        m_Shooting.enabled = false;
        m_CanvasGameObject.SetActive(true);
    } 

    public void DisableInvincible()
    {
        m_Health.m_CanTakeDamage = true;
        m_Shooting.enabled = true;
        SetColor(m_PlayerColor);
    }

    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPosition;
        m_Instance.transform.rotation = m_SpawnRotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }

    public void Respawn()
    {
        m_Instance.transform.position = m_SpawnPosition;
        m_Instance.transform.rotation = m_SpawnRotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);

        SetColor(m_PlayerColor);
        DisableControl();
        m_CanvasGameObject.SetActive(true);
    }

    public void CycleInvincibleColor()
    {
        var nextColor = (m_CurrentColor == m_PlayerColor) ? m_InvincibleColor : m_PlayerColor;
        SetColor(nextColor);
    }

    public bool IsDead()
    {
        return m_Health.m_Dead;
    }
}
