using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Scrolls the ground texture like a treadmill to give a moving effect.
/// </summary>
public class GroundScrolling : MonoBehaviour
{
    public float scrollSpeed = 1.0f;
    private Renderer groundRenderer;

    float ratio;

    private void Start()
    {
        groundRenderer = GetComponent<Renderer>();
        ratio = (transform.localScale.z * 4f) / 5f;    // this formula is based solely on trial and error
        scrollSpeed = GameManager.Instance.GameSpeed / ratio;  // scroll speed is dependent on game speed
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
    
    /// <summary>
    /// Increases scroll speed when game speed goes up.
    /// </summary>
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