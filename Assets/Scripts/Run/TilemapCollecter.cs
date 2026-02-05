using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapItemCollector : MonoBehaviour
{
    public Tilemap[] itemTilemap; // 아이템이 그려진 타일맵 레이어 연결

    void Update()
    {
        // 플레이어 발밑 좌표 확인
        Vector3Int cellPosition = itemTilemap[0].WorldToCell(transform.position);

        // 타일 처리
        foreach (Tilemap tilemap in itemTilemap)
        {
            TileBase tile = tilemap.GetTile(cellPosition);

            if (tile != null)
            {
                RewardManager.Instance.CollectItem(tilemap.tag); // 오브젝트 tag 처리
                
                tilemap.SetTile(cellPosition, null); // 제거
            }
        }
    }
}