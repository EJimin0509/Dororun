using UnityEngine;

public class WorldScroller : MonoBehaviour
{
    /// <summary>
    /// 胶农费 力绢 script
    /// </summary>
    public float scrollSpeed = 12f; // 胶农费 加档

    void Update()
    {
        transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);
    }
}
