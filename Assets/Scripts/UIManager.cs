using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject scorePanel;
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject inputPanel;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] Color buttonPressedColor;

    public Image jumpButton;
    public Image leftButton;
    public Image rightButton;

    TextMeshProUGUI scoreText;

    public static UIManager Instance { get; private set; }

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void DisplayHighScore(int highScore)
    {
        highScoreText.text = "High Score: " + highScore.ToString();
    }

    public void DisplayScore(int score)
    {
        scoreText = scorePanel.GetComponent<TextMeshProUGUI>();
        scoreText.text = score.ToString();
    }

    public void OnStartPressed()
    {
        scorePanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        inputPanel.SetActive(true);
    }

    public void ModifyMoveButtonColors(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<Vector2>().x;

        if (value > 0)
        {
            rightButton.color = buttonPressedColor;
            leftButton.color = Color.white;
        }
        else if (value < 0)
        {
            leftButton.color = buttonPressedColor;
            rightButton.color = Color.white;
        }
        else
        {
            leftButton.color = Color.white;
            rightButton.color = Color.white;
        }
    }

    public void ModifyJumpButtonColor(bool isGrounded)
    {
        if (isGrounded)
        {
            jumpButton.color = Color.white;
        }
        else
        {
            jumpButton.color = buttonPressedColor;
        }
    }
}
