using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
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

    private void OnCollisionEnter(Collision other) 
    {
        if (isInvulnerable)
            return;

        if (other.gameObject.layer == obstacleLayerMask)
        {
            AudioManager.PlaySfx(AudioClipName.Impact);
            Time.timeScale = 0;
            AudioManager.FadeOutMusic(0, 0.2f, 0.1f, 0);
            GameManager.Instance.RestartLevel();
        }
    }
}
