using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    [Header("Current Stage Rewards")]
    public int currentCredit = 0; // 현재 크래디트
    public int currentJewel = 0; // 현재 쥬얼
    public ResultUI resultUI; // 결과 UI

    [Header("Stage Stop")]
    //public PlayerController RunController; // 러닝 스테이지 플레이어 컨트롤러
    public BossPlayerController BossController; // 보스 스테이지 플레이어 컨트롤러


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
    public void CollectItem(string itemTag)
    {
        // Tag 비교
        switch (itemTag)
        {
            case "Credit":
                currentCredit++; break;
            case "Jewel":
                currentJewel++; break;
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
