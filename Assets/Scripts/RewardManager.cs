using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    [Header("Current Stage Rewards")]
    public int currentCredit = 0; // 현재 크래디트
    public int currentJewel = 0; // 현재 쥬얼
    public List<string> collectedSupporterIDs = new List<string>(); // 서포터ID 저장 리스트
    public ResultUI resultUI; // 결과 UI

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else Destroy(gameObject);
    }

    public void ResetStageData()
    {
        // 기록 초기화
        currentCredit = 0;
        currentJewel = 0;
    }

    // 아이템 획득 시 호출
    public void CollectItem(string itemTag, string supporterID = "")
    {
        // Tag 비교
        switch (itemTag)
        {
            case "Credit":
                currentCredit++; break;
            case "Jewel":
                currentJewel++; break;
            case "Supoorter":
                if (!string.IsNullOrEmpty(supporterID) && !collectedSupporterIDs.Contains(supporterID))
                {
                   collectedSupporterIDs.Add(supporterID);
                    Debug.Log($"Supporter: {supporterID}");
                } break;
        }
    }

    // 최종 정산 함수
    public void FinalizeReward(bool isVictory)
    {
        float multiplier = isVictory ? 1.5f : 1.0f; // 승리시, 1.5배 보상

        int finalCredit = Mathf.RoundToInt(currentCredit * multiplier); // 최종 크래디트
        int finalJewel = Mathf.RoundToInt(currentJewel * multiplier); // 최종 쥬얼

        // 총 재화에 더함
        int totalCredit = PlayerPrefs.GetInt("TotalCredit", 0) + finalCredit;
        PlayerPrefs.SetInt("TotalCredit", totalCredit);

        int totalJewel = PlayerPrefs.GetInt("TotalJewel", 0) + finalJewel;
        PlayerPrefs.SetInt("TotalJewel", totalJewel);

        // UI 호출
        if (resultUI != null)
        {
            resultUI.ShowResult(isVictory, finalCredit, finalJewel);
        }

        // 서포터는 획득 시 영구 소장
        foreach (string id in collectedSupporterIDs)
        {
            PlayerPrefs.SetInt("Supporter_" + id, 1);
        }

        PlayerPrefs.Save();
        Debug.Log($"result: {isVictory}, credit: {finalCredit}, jewel: {finalJewel}");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;// 씬 로드 시 실행
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 스크립트 비활성화 시 해제
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        resultUI = GameObject.FindFirstObjectByType<ResultUI>(); // 현재 씬의 ResultUI 연결
    }
}
