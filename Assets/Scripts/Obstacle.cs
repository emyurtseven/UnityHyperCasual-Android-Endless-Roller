using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    void Start()
    {
        StartCoroutine(CheckOutOfGame());
    }

    void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
    }

    IEnumerator CheckOutOfGame()
    {
        while (true)
        {
            if (transform.position.z < -10)
            {
                Destroy(gameObject);
                yield break;
            }
            
            yield return new WaitForSeconds(1f);
        }
    }
}
