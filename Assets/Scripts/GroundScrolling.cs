using UnityEngine;

public class GroundScrolling : MonoBehaviour
{
    public float scrollSpeed = 1.0f;
    private Renderer groundRenderer;

    private void Start()
    {
        groundRenderer = GetComponent<Renderer>();
        scrollSpeed = GameManager.Instance.GameSpeed / 12; 
    }

    private void Update()
    {
        float offset = Time.time * scrollSpeed;
        groundRenderer.material.SetTextureOffset("_MainTex", new Vector2(0, -offset));
    }

    public void OnDifficultyUpListener()
    {
        scrollSpeed += (1 / 12f);
    }
}