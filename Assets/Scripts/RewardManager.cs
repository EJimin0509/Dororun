using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    [Header("Current Stage Rewards")]
    public int currentCredit = 0; // 현재 크래디트
    public int currentJewel = 0; // 현재 쥬얼
    public List<int> collectedSupportersIDs; // 수집한 서포터 ID 리스트
    public List<Sprite> collectedSupporterSprites; // 수집한 서포터 스프라이트 리스트
    public ResultUI resultUI; // 결과 UI

    [Header("Stage Stop")]
    //public PlayerController RunController; // 러닝 스테이지 플레이어 컨트롤러
    public BossPlayerController BossController; // 보스 스테이지 플레이어 컨트롤러

    [Header("Stage Info")]
    public int CurrentStageIndex; // 스테이지 번호


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else Destroy(gameObject);
    }

    private void Start()
    {
        collectedSupportersIDs = new List<int>();
        collectedSupporterSprites = new List<Sprite>();
    }

    public void ResetStageData()
    {
        // 기록 초기화
        currentCredit = 0;
        currentJewel = 0;
        collectedSupportersIDs.Clear();
        collectedSupporterSprites.Clear();
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

    /// <summary>
    /// 서포터 수집 시 호출
    /// </summary>
    /// <param name="id"></param>
    /// <param name="sprite"></param>
    public void CollectSupporter(int id, Sprite sprite)
    {
        // 서포터 수집 로직
        if (!collectedSupportersIDs.Contains(id)) // 중복 수집 방지
        {
            collectedSupportersIDs.Add(id); // ID 추가
            collectedSupporterSprites.Add(sprite); // 스프라이트 추가
            Debug.Log($"Supporter {id} collected");
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

        // 수집한 서포터 저장
        foreach (int id in collectedSupportersIDs)
        {
            PlayerPrefs.SetInt("Supporter_" + id, 1); // 수집 완료 표시
        }

        // 스테이지 클리어 저장
        int currentMax = PlayerPrefs.GetInt("MaxClearedStage", 0);

        if (CurrentStageIndex > currentMax)
        {
            PlayerPrefs.SetInt("MaxClearedStage", CurrentStageIndex);
            PlayerPrefs.Save();
        }

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
