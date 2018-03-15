using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public int m_GameModeInt = 0;
    public int m_NumRoundsToWin = 5;        
    public float m_StartDelay = 3f;         
    public float m_EndDelay = 3f; 
    public CameraControl m_CameraControl;   
    public Text m_MessageText;
    public Text m_TimeText;
    public Text m_ScoreText;
    public GameObject m_TankPrefab;
    public float m_MaxRoundTime = 300f;
    public int m_NumTanks;
    public int m_Humans;
    public int m_DumbAI;
    public float m_SpawnRadius;
    public IGameMode m_GameMode;

    private int m_RoundNumber;              
    private WaitForSeconds m_StartWait;     
    private WaitForSeconds m_EndWait;
    //private WaitForSeconds m_SpawnDelay;
    private WaitForSeconds m_InvincibleDelay;
    private TankManager m_RoundWinner;
    private TankManager m_GameWinner;
    private TankManager[] m_Tanks;
    private float m_Time = 0f;

    public void Update()
    {
        m_Time += Time.deltaTime;
    }

    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);
        m_InvincibleDelay = new WaitForSeconds(2f);

        switch (m_GameModeInt)
        {
            case 1:
                m_GameMode = GetComponentInChildren<DeathmatchMode>();
                break;
            default:
                m_GameMode = GetComponentInChildren<LastManStandingMode>();
                break;
        }

        SpawnAllTanks();
        SetCameraTargets();


        float scale = 50f;
        var scorePosition = m_ScoreText.GetComponent<RectTransform>().position;
        m_ScoreText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_Tanks.Length * scale);
        m_ScoreText.GetComponent<RectTransform>().position = new Vector3(scorePosition.x + 100f, scorePosition.y - m_Tanks.Length * scale / 2f, scorePosition.z);

        StartCoroutine(GameLoop());
    }

    public IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (m_GameWinner != null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();

        m_CameraControl.SetStartPositionAndSize();

        m_RoundNumber++;
        m_MessageText.text = "Round " + m_RoundNumber;

        m_Time = 0f;

        m_ScoreText.text = GetScoreText();
        m_TimeText.text = GetTimeText();

        yield return m_StartWait;
        m_GameMode.StartRound(m_Tanks);
    }

    private IEnumerator RoundPlaying()
    {
        m_MessageText.text = string.Empty;
        
        m_Time = 0f;
        while (!m_GameMode.IsEndOfRound(m_Tanks) && m_Time < m_MaxRoundTime)
        {
            m_ScoreText.text = GetScoreText();
            m_TimeText.text = GetTimeText();
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        DisableTankControl();

        m_RoundWinner = null;

        m_RoundWinner = m_GameMode.GetRoundWinner(m_Tanks);

        if (m_RoundWinner != null)
        {
            m_RoundWinner.m_RoundScore++;
        }

        m_GameWinner = GetGameWinner();

        string message = EndMessage();
        m_TimeText.text = "";
        m_MessageText.text = message;

        yield return m_EndWait;
    }

    private void SpawnAllTanks()
    {
        m_NumTanks = (m_NumTanks > 8) ? 8 : m_NumTanks;
        m_Tanks = new TankManager[m_NumTanks];
        Color[] colors = { Color.blue, Color.red, Color.green, Color.magenta, Color.cyan, Color.black, new Color(0.5f, 0.5f, 0.5f), new Color(155f/255f, 76f/255f, 0f) };
        for (int i = 0; i < m_Tanks.Length; ++i)
        {
            float rotation = i * Mathf.PI * 2 / m_NumTanks;

            Vector2 direction = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation)) * m_SpawnRadius;
            Vector3 spawnPosition = new Vector3(direction.y, 0f, direction.x);
            Quaternion spawnRotation = new Quaternion { eulerAngles = new Vector3(0f, Mathf.Rad2Deg * rotation, 0f) };
            m_Tanks[i] = new TankManager()
            {
                m_SpawnPosition = spawnPosition,
                m_SpawnRotation = spawnRotation,
                m_PlayerColor = colors[i],
                m_Instance = Instantiate(m_TankPrefab, spawnPosition, spawnRotation) as GameObject,
                m_PlayerNumber = i + 1,
                m_TankUserType = (m_Humans-- > 0) ? 0 : 1,
            };
            m_Tanks[i].Setup();
        }
    }

    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[m_Tanks.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = m_Tanks[i].m_Instance.transform;
        }

        m_CameraControl.m_Targets = targets;
    }

    private string GetTimeText()
    {
        return "Time: " + ((int)m_MaxRoundTime - (int)m_Time).ToString();
    }

    private string GetScoreText()
    {
        string scoreText = "Score\n";
        foreach (var tank in m_Tanks)
        {
            scoreText += "Player " + tank.m_PlayerNumber.ToString() + ": " + tank.m_Score.ToString() + "\n";
        }
        return scoreText;
    }

    private TankManager GetGameWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_RoundScore == m_NumRoundsToWin)
                return m_Tanks[i];
        }

        return null;
    }


    private string EndMessage()
    {
        string message = "DRAW!";

        string roundOrGame = (m_GameWinner != null) ? "GAME!" : "ROUND!";
        if (m_RoundWinner != null)
            message = m_RoundWinner.m_ColoredPlayerText + " HAS WON THE " + roundOrGame;

        message += "\n\n\n\n";

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_RoundScore + " WINS\n";
        }

        return message;
    }


    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].Reset();
        }
    }

    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].DisableControl();
        }
    }
}