using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemTile : Tile
{
    public enum ItemType { Credit, Jewel, Supporter }

    [Header("Item Settings")]
    public ItemType itemType; // 아이템 종류 (크레딧, 쥬얼, 서포터)

    [Header("Supporter Only")]
    public string supporterID; // 서포터 ID
}