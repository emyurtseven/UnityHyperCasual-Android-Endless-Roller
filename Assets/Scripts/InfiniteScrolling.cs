using UnityEngine;

public class InfiniteScrolling : MonoBehaviour
{
    public float scrollSpeed = 1.0f;
    private Renderer groundRenderer;

    private void Start()
    {
        groundRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        float offset = Time.time * scrollSpeed;
        groundRenderer.material.SetTextureOffset("_MainTex", new Vector2(0, -offset));
    }
}