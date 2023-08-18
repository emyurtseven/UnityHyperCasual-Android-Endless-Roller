using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages obstacle movement. Attached to obstacle objects in the scene. 
/// </summary>
public class Obstacle : MonoBehaviour
{
    [SerializeField] int[] allowedLanes;   // which one of the three lanes can this obstacle spawn from?

    Transform playerTransform;
    float moveSpeed = 1f;

    public int[] AllowedLanes { get => allowedLanes; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    /// <summary>
    /// OnEnable is used because obstacles are pooled and never destroyed 
    /// </summary>
    private void OnEnable() 
    {
        StartCoroutine(FadeInObject());
        StartCoroutine(CheckPlayerDodged());
    }

    private void OnDisable() 
    {
        StopAllCoroutines();
    }

    void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);  // move our obstacle towards player
    }

    /// <summary>
    /// Listener for difficulty up event which is invoked from GameManager
    /// </summary>
    public void OnDifficultyUpListener()
    {
        moveSpeed = GameManager.Instance.GameSpeed;
    }

    /// <summary>
    /// Checks if player has gone past this obstacle.
    /// </summary>
    IEnumerator CheckPlayerDodged()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);
        while (true)
        {
            if (transform.position.z < playerTransform.position.z)
            {
                AudioManager.PlaySfx(AudioClipName.ScoreUp, 0.5f);
                GameManager.Instance.ScoreUp();
                StartCoroutine(CheckOutOfGame());
                yield break;
            }

            yield return delay;
        }
    }

    /// <summary>
    /// Checks if obstacle has left the screen and is ready to return to object pool.
    /// </summary>
    IEnumerator CheckOutOfGame()
    {
        WaitForSeconds delay = new WaitForSeconds(1f);
        while (true)
        {
            if (transform.position.z < -10)
            {
                // return this obstacle to pool
                ObjectPool.ReturnPooledObject(gameObject.name, gameObject);
                yield break;
            }
            
            yield return delay;
        }
    }

    /// <summary>
    /// Adds a fade in effect for newly spawned obstacles.
    /// </summary>
    private IEnumerator FadeInObject()
    {
        float alpha = 0;
        Color originalColor = transform.GetChild(0).GetComponent<MeshRenderer>().materials[0].color;

        while (alpha <= 1)
        {
            alpha += 0.01f;
            Color color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            foreach (Transform piece in transform)
            {
                piece.gameObject.GetComponent<MeshRenderer>().materials[0].color = color;
            }

            yield return null;
        }
    }
}
