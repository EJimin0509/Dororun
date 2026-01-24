using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapItemCollector : MonoBehaviour
{
    public Tilemap itemTilemap; // 아이템이 그려진 타일맵 레이어 연결

    void Update()
    {
        // 플레이어 발밑 좌표 확인
        Vector3Int cellPosition = itemTilemap.WorldToCell(transform.position);

        TileBase clickedTile = itemTilemap.GetTile(cellPosition); // 타일 확인

        if (clickedTile != null)
        {
            ProcessItem(clickedTile, cellPosition);
        }
    }
    void ProcessItem(TileBase tile, Vector3Int position)
    {
        // 1. 가져온 타일이 우리가 만든 ItemTile인지 확인
        ItemTile itemTile = tile as ItemTile;

        if (itemTile != null)
        {
            // 2. 이름이 아닌 '타입'으로 분기 처리
            switch (itemTile.itemType)
            {
                case ItemTile.ItemType.Credit:
                    RewardManager.Instance.CollectItem("Credit");
                    break;

                case ItemTile.ItemType.Jewel:
                    RewardManager.Instance.CollectItem("Jewel");
                    break;

                case ItemTile.ItemType.Supporter:
                    RewardManager.Instance.CollectSupporter(itemTile.supporterID);
                    break;
            }

            // 3. 수집 후 타일 제거
            itemTilemap.SetTile(position, null);
        }
    }
}