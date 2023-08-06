using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] float gameSpeedStart = 10f;
    [SerializeField] float gravity = 15f;

    [Header("Difficulty settings")]
    [SerializeField] float startingMinSpawnInterval = 1.5f;
    [SerializeField] float startingMaxSpawnInterval = 2.5f;
    [SerializeField] int difficultyUpThreshold = 15;
    [Tooltip("GameSpeed increases by this amount upon difficulty up")]
    [SerializeField] float gameSpeedIncrement = 1f;
    [Tooltip("Min spawn interval decreases by this amount upon difficulty up")]
    [SerializeField] float minSpawnDecrement = 0.1f;
    [Tooltip("Max spawn interval decreases by this amount upon difficulty up")]
    [SerializeField] float maxSpawnDecrement = 0.2f;
    public UnityEvent difficultyUpEvent;

    [Header("Object references")]
    [SerializeField] ObstacleSpawner obstacleSpawner; 
    [SerializeField] GameObject scorePanel;
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject inputPanel;
    [SerializeField] TextMeshProUGUI highScoreText;

    [Header("Other settings")]
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
        AudioManager.Initialize(2);

        Time.timeScale = 0;
        Physics.gravity = new Vector3(0, -gravity, 0);

        if (Instance == null)
        {
            Instance = this;
        }

        minIntervalCurrent = startingMinSpawnInterval;
        maxIntervalCurrent = startingMaxSpawnInterval;
        gameSpeedCurrent = gameSpeedStart;
    }

    private void Start()
    {
        ObjectPool.Initialize(objectPoolSize);

        if (debugMode)
        {
            Debug.LogWarning("Debug mode active, only certain shapes are spawned");
        }
        
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

        if (!AudioManager.GetMusicPlayer(0).IsPlaying)
        {
            AudioManager.PlayMusicFadeIn(0, AudioClipName.Music, 0.5f, 1, 0.2f);
        }
        else
        {
            AudioManager.FadeInMusic(0, 0.5f, 1f, 0);
        }
    }

    public void OnStartPressed()
    {
        Time.timeScale = 1;
        scorePanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        inputPanel.SetActive(true);
        scoreText.text = score.ToString();
        AudioManager.PlayMusicFadeIn(1, AudioClipName.WindyBackground, 0.2f, 2, 0); 
        obstacleSpawner.StartSpawnCoroutine();

        AudioManager.PlaySfx(AudioClipName.Confirm);
    }

    public void RestartLevel()
    {
        StartCoroutine(RestartCoroutine());
    }

    private IEnumerator RestartCoroutine()
    {
        SaveScore();
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 1;
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
        if (score % difficultyUpThreshold == 0)
        {
            gameSpeedCurrent += gameSpeedIncrement;
            minIntervalCurrent -= minSpawnDecrement;
            maxIntervalCurrent -= maxSpawnDecrement;

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
