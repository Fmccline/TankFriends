using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Assets.Scripts.Factories;

public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;        
    public float m_StartDelay = 3f;         
    public float m_EndDelay = 3f;
    public CameraControl m_CameraControl;   
    public Text m_MessageText;
    public Text m_TimeText;
    public Text m_ScoreText;
    public GameObject m_TankPrefab;
    public TankManagerSpawner m_TankManagerSpawner;
    public float m_MaxRoundTime = 300f;
    public IGameMode m_GameMode;
    public enum GameMode { DeathMatch = 0, LastManStanding = 1 };
    public GameMode m_GameModeType = GameMode.DeathMatch;

    private int m_RoundNumber;              
    private WaitForSeconds m_StartWait;     
    private WaitForSeconds m_EndWait;
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
        StartGame();
    }

    public void StartGame()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        switch (m_GameModeType)
        {
            case GameMode.DeathMatch:
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
            m_RoundWinner.m_TankScore.AddWin();
        }

        m_GameWinner = GetGameWinner();

        string message = EndMessage();
        m_TimeText.text = "";
        m_MessageText.text = message;

        yield return m_EndWait;
    }

    private void SpawnAllTanks()
    {
        m_Tanks = m_TankManagerSpawner.SpawnTankManagers();
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
        var scores = m_GameMode.GetRoundScores(m_Tanks);
        for (int i = 0; i < m_Tanks.Length; ++i)
        {
            var tank = m_Tanks[i];
            scoreText += "Player " + tank.m_PlayerNumber.ToString() + ": " + scores[i].ToString() + "\n";
        }
        return scoreText;
    }

    private TankManager GetGameWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_TankScore.GetWins() == m_NumRoundsToWin)
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
            message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_TankScore.GetWins() + " WINS\n";
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