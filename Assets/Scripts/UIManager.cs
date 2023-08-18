using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// Manages UI elements and functionalities.
/// </summary>
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

    // add a static access to this class
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

    /// <summary>
    /// Since movement "buttons" are actually just images in this project, 
    /// this function imitates a pressed visual effect, even when other input devices are used.
    /// </summary>
    public void ModifyMoveButtonColors(InputAction.CallbackContext context)
    {
        // read input from PlayerInput component
        float value = context.ReadValue<Vector2>().x;

        // modify right or left button images according to input
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

    /// <summary>
    /// Modifies jump "button" image when player is in air or not.
    /// </summary>
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
