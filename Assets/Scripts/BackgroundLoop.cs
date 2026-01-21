using UnityEngine;
/// <summary>
/// Background를 Loop 시키는 스크립트
/// </summary>
public class BackgroundLoop : MonoBehaviour
{
    private float width; // 배경의 너비

    void Awake()
    {
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        width = backgroundCollider.size.x; // 배경의 너비를 가져옴
    }

    void Update()
    {
        // 원점에서 -width 위치에 도달하면 재배치
        if (transform.position.x <= -width)
        {
            Reposition();
        }
    }

    // 재배치 메서드
    private void Reposition()
    {
        Vector2 offset = new Vector2(width * 2f, 0);
        transform.position = (Vector2)transform.position + offset; // 오른쪽 width * 2만큼 이동
    }
}
