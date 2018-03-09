using System;
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
    [HideInInspector] public int m_EnemiesKilled { get { return m_Shooting.m_EnemiesDestroyed; } set { m_Shooting.m_EnemiesDestroyed = value; } }
    [HideInInspector] public int m_Deaths { get { return m_Health.m_Deaths; } set { m_Health.m_Deaths = value; } }
    [HideInInspector] public int m_Score = 0;
    [HideInInspector] public int m_RoundScore = 0;

    private TankMovement m_Movement;       
    private TankShooting m_Shooting;
    private GameObject m_CanvasGameObject;
    private TankHealth m_Health;

    public void Setup()
    {
        m_Movement = m_Instance.GetComponent<TankMovement>();
        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_Health = m_Instance.GetComponent<TankHealth>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        m_Movement.m_PlayerNumber = m_PlayerNumber;
        m_Shooting.m_PlayerNumber = m_PlayerNumber;

        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }
    }

    public void RestartRound()
    {
        Respawn();
        m_Deaths = 0;
        m_Score = 0;
        m_EnemiesKilled = 0;
    }

    public void DisableControl()
    {
        m_Health.m_CanTakeDamage = false;
        m_Movement.enabled = false;
        m_Shooting.enabled = false;
        SetDisabledColors();
        m_CanvasGameObject.SetActive(false);
    }

    private void SetDisabledColors()
    {
        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            Color invincibleColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            renderers[i].material.color = invincibleColor;
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
        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }
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

        DisableControl();
        m_CanvasGameObject.SetActive(true);
    }
}
