using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages player collision with obstacles.
/// </summary>
public class PlayerCollision : MonoBehaviour
{
    [SerializeField] GameObject impactEffect;
    
    bool isInvulnerable = false;
    int obstacleLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.DebugMode)
        {
            isInvulnerable = true;
        }
        obstacleLayerMask = LayerMask.NameToLayer("Obstacle");
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if (isInvulnerable)
            return;

        if (collision.gameObject.layer == obstacleLayerMask)
        {
            Instantiate(impactEffect, collision.GetContact(0).point, Quaternion.identity);
            GameOver();
        }
    }

    /// <summary>
    /// Stops the game and music, calls restart.
    /// </summary>
    /// <param name="collision"></param>
    private void GameOver()
    {
        AudioManager.PlaySfx(AudioClipName.Impact);
        Time.timeScale = 0;
        AudioManager.FadeOutMusic(0, 0.3f, 0.1f, 0);
        AudioManager.StopMusic(1);
        GameManager.Instance.RestartLevel();
    }
}
