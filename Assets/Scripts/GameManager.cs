using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent difficultyUpEvent;

    [SerializeField] float gameSpeedDefault = 10f;
    [SerializeField] float gravity = 15f;
    [SerializeField] float minSpawnIntervalDefault = 1.5f;
    [SerializeField] float maxSpawnIntervalDefault = 2.5f;

    [SerializeField] ObstacleSpawner obstacleSpawner; 
    [SerializeField] GameObject scorePanel;
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject inputPanel;
    [SerializeField] TextMeshProUGUI highScoreText;

    [SerializeField] int objectPoolSize = 5;
    [SerializeField] bool debugMode = false;

    int score = 0;
    int highScore;
    TextMeshProUGUI scoreText;

    float gameSpeedCurrent;
    float minIntervalCurrent;
    float maxIntervalCurrent;

    public static GameManager Instance { get; private set; }
    public float GameSpeed { get => gameSpeedCurrent; }
    public bool DebugMode { get => debugMode; }
    public float MinSpawnInterval { get => minIntervalCurrent; }
    public float MaxSpawnInterval { get => maxIntervalCurrent; }

    void Awake()
    {
        Time.timeScale = 0;
        Physics.gravity = new Vector3(0, -gravity, 0);

        if (Instance == null)
        {
            Instance = this;
        }

        minIntervalCurrent = minSpawnIntervalDefault;
        maxIntervalCurrent = maxSpawnIntervalDefault;
        gameSpeedCurrent = gameSpeedDefault;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("highScore"))
        {
            highScore = PlayerPrefs.GetInt("highScore");
        }
        else
        {
            highScore = 0;
        }

        highScoreText.text = "High Score: " + highScore.ToString();
        scoreText = scorePanel.GetComponent<TextMeshProUGUI>();
        ObjectPool.Initialize(objectPoolSize);
    }

    public void OnStartPressed()
    {
        Time.timeScale = 1;
        scorePanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        inputPanel.SetActive(true);
        scoreText.text = score.ToString();
        AudioManager.PlayMusicFadeIn(AudioClipName.WindyBackground, 0.2f, 4, 0); 
        obstacleSpawner.StartSpawnCoroutine();
    }

    public void RestartLevel()
    {
        SaveScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SaveScore()
    {
        if (score > highScore)
        {
            PlayerPrefs.SetInt("highScore", score);
        }
    }

    public void ScoreUp()
    {
        score++;
        scoreText.text = score.ToString();

        CheckDifficultyUp();
    }

    private void CheckDifficultyUp()
    {
        if (score % 15 == 0)
        {
            gameSpeedCurrent += 1f;
            minIntervalCurrent -= 0.1f;
            maxIntervalCurrent -= 0.2f;

            difficultyUpEvent.Invoke();
        }
    }

    private void OnApplicationQuit()
    {
        SaveScore();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        SaveScore();
    }
}
