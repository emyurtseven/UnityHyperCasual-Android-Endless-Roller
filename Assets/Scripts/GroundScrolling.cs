using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScrolling : MonoBehaviour
{
    public float scrollSpeed = 1.0f;
    private Renderer groundRenderer;

    float ratio;

    private void Start()
    {
        groundRenderer = GetComponent<Renderer>();
        ratio = (transform.localScale.z * 4f) / 5f; 
        scrollSpeed = GameManager.Instance.GameSpeed / ratio; 
    }

    private void Update()
    {
        float offset = Time.time * scrollSpeed;
        groundRenderer.material.SetTextureOffset("_MainTex", new Vector2(0, -offset));
    }

    public void OnDifficultyUpListener()
    {
        StartCoroutine(SpeedUp());
    }
    
    IEnumerator SpeedUp()
    {
        float newSpeed = scrollSpeed += (1 / ratio);
        while (scrollSpeed < newSpeed)
        {
            scrollSpeed += Time.deltaTime;
            yield return null;
        }

        scrollSpeed = newSpeed;
    }
}