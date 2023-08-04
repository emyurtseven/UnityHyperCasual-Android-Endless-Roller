using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] int[] allowedLanes;

    Transform playerTransform;
    float moveSpeed = 1f;

    public int[] AllowedLanes { get => allowedLanes; set => allowedLanes = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable() 
    {
        StartCoroutine(CheckPlayerDodged());
    }

    private void OnDisable() 
    {
        StopAllCoroutines();
    }

    void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
    }

    public void OnDifficultyUpListener()
    {
        moveSpeed = GameManager.Instance.GameSpeed;
    }

    IEnumerator CheckPlayerDodged()
    {
        while (true)
        {
            if (transform.position.z < playerTransform.position.z)
            {
                AudioManager.PlaySfx(AudioClipName.ScoreUp, 0.5f);
                GameManager.Instance.ScoreUp();
                StartCoroutine(CheckOutOfGame());
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator CheckOutOfGame()
    {
        while (true)
        {
            if (transform.position.z < -10)
            {
                ObjectPool.ReturnPooledObject(gameObject.name, gameObject);
                yield break;
            }
            
            yield return new WaitForSeconds(1f);
        }
    }
}
