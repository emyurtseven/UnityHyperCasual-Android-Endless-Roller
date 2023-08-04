using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] float gameSpeed = 10f;

    [SerializeField] ObstacleSpawner obstacleSpawner; 
    [SerializeField] GameObject scorePanel;
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject inputPanel;

    [SerializeField] int objectPoolSize = 5;
    [SerializeField] bool debugMode = false;

    int score = 0;
    TextMeshProUGUI scoreText;

    public static GameManager Instance { get; private set; }
    public float GameSpeed { get => gameSpeed; set => gameSpeed = value; }
    public bool DebugMode { get => debugMode; }

    void Awake()
    {
        Time.timeScale = 0;

        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start() 
    {
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ScoreUp()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
