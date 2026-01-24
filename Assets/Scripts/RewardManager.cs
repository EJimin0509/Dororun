using UnityEngine;
using System.Collections.Generic;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    [Header("Current Stage Rewards")]
    public int currentCredit = 0; // 현재 크래디트
    public int currentJewel = 0; // 현재 쥬얼
    public List<string> collectedSupporterIDs = new List<string>(); // 서포터ID 저장 리스트

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 아이템 획득 시 호출
    public void CollectItem(ItemTile.ItemType type, string id = "")
    {
        if (type == ItemTile.ItemType.Credit) currentCredit++;
        else if (type == ItemTile.ItemType.Jewel) currentJewel++;
        else if (type == ItemTile.ItemType.Supporter)
        {
            if (!collectedSupporterIDs.Contains(id)) collectedSupporterIDs.Add(id);
            Debug.Log($"Supporter ID: {id}");
        }
    }

    // 최종 정산 함수
    public void FinalizeReward(bool isVictory)
    {
        float multiplier = isVictory ? 1.5f : 1f; // 승리시, 1.5배 보상

        int finalCredit = Mathf.RoundToInt(currentCredit * multiplier); // 최종 크래디트
        int finalJewel = Mathf.RoundToInt(currentJewel * multiplier); // 최종 쥬얼

        // 총 재화에 더함
        int totalCredit = PlayerPrefs.GetInt("TotalCredit", 0) + finalCredit;
        PlayerPrefs.SetInt("TotalCredit", totalCredit);

        int totalJewel = PlayerPrefs.GetInt("TotalJewel", 0) + finalJewel;
        PlayerPrefs.SetInt("TotalJewel", totalJewel);

        // 서포터는 획득 시 영구 소장
        foreach (string id in collectedSupporterIDs)
        {
            PlayerPrefs.SetInt("Supporter_" + id, 1);
        }

        PlayerPrefs.Save();
        Debug.Log($"result: {isVictory}, credit: {finalCredit}, jewel: {finalJewel}");
    }
}
