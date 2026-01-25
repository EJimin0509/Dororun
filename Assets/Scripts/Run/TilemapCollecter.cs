using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapItemCollector : MonoBehaviour
{
    public Tilemap[] itemTilemap; // 아이템이 그려진 타일맵 레이어 연결

    void Start()
    {
        // 이미 수집한 서포터 제거
        foreach (Tilemap map in itemTilemap) 
        {
            if (map.CompareTag("Supporter"))
            {
                RemoveAlreadyCollected(map);
            }
        }
    }

    // 이미 수집한 서포터 처리 메서드
    void RemoveAlreadyCollected(Tilemap map)
    {
        BoundsInt bounds = map.cellBounds; // 현재 타일맵 범위

        foreach (var pos in bounds.allPositionsWithin)
        {
            TileBase tile = map.GetTile(pos);
            if (tile != null)
            {
                if (RewardManager.Instance.collectedSupporterIDs.Contains(tile.name))
                {
                    map.SetTile(pos, null); // 사라짐
                }
            }
        }
    }
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
                RewardManager.Instance.CollectItem(tilemap.tag, tilemap.name); // 오브젝트 tag 처리
                
                tilemap.SetTile(cellPosition, null); // 제거
            }
        }
    }
}