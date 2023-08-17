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
        StartCoroutine(FadeInObject());
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

    IEnumerator CheckOutOfGame()
    {
        WaitForSeconds delay = new WaitForSeconds(1f);
        while (true)
        {
            if (transform.position.z < -10)
            {
                ObjectPool.ReturnPooledObject(gameObject.name, gameObject);
                yield break;
            }
            
            yield return delay;
        }
    }

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
