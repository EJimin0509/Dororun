using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class ResultUI : MonoBehaviour
{
    /// <summary>
    /// 서포터 수집 구조체
    /// </summary>
    [System.Serializable]
    public struct SupporterData
    {
        public string supporterID; // 서포터 ID
        public Sprite supporterSprite; // 서포터 sprite
    }

    /// <summary>
    /// 서포터 스프라이트 경로
    /// </summary>
    [Header("Supporter Data Library")]
    public List<SupporterData> supporterLibrary;

    /// <summary>
    /// 배경 UI 배치
    /// </summary>
    [Header("Panels")]
    public GameObject victoryPanel; // 승리
    public GameObject failedPanel; // 패배
    public GameObject resultParent; // UI 부모

    /// <summary>
    /// 결과 수치 표시
    /// </summary>
    [Header("Reward Texts")]
    public TextMeshProUGUI creditText; // 크래디트
    public TextMeshProUGUI jewelText; // 쥬얼
    public Transform supporterContainer; // 서포터 이미지 부모
    public GameObject supporterImagePrefab; // UI 프리팹

    void Start()
    {
        resultParent.SetActive(false); // 시작할 땐 숨김
    }

    // UI 활성화 메서드 -> RewardManager에서 호출
    public void ShowResult(bool isVictory, int credit, int jewel)
    {
        resultParent.SetActive(true); // 활성화

        // 상태 처리
        victoryPanel.SetActive(isVictory); // 승리
        failedPanel.SetActive(!isVictory); // 패배

        // 텍스트 업데이트
        creditText.text = credit.ToString();
        jewelText.text = jewel.ToString();

        // 서포터 표시
        DisplaySupporters();

        Time.timeScale = 0f; // 시간 정지
    }

    // 서포터 표시 메서드
    void DisplaySupporters()
    {
        // 프리팹 확인
        if (supporterImagePrefab == null)
        {
            Debug.LogError("ResultUI: supporterImagePrefab not connected");
            return;
        }

        // 기존에 생성된 아이콘 제거
        foreach (Transform child in supporterContainer) Destroy(child.gameObject);

        List<string> collectedID = RewardManager.Instance.collectedSupporterIDs; // 수집한 서포터 ID 리스트

        // 리스트가 비어있다면 함수 종료
        if (collectedID == null || collectedID.Count == 0)
        {
            Debug.Log("no supporters are collected");
            return;
        }

        foreach (string id in collectedID)
        {
            var data = supporterLibrary.Find(s => s.supporterID == id); // 라이브러리에서 ID 찾기

            if (data.supporterSprite != null)
            {
                GameObject newIcon = Instantiate(supporterImagePrefab, supporterContainer);
                newIcon.GetComponent<Image>().sprite = data.supporterSprite;
            }
            else Debug.Log($"couldnt find supporter sprite: {id}");
        }

    }

    // 스테이지 선택 화면으로 돌아가기
    public void OnClickBack()
    {
        Time.timeScale = 1f; // 시간 재생

        SceneManager.LoadScene("Main"); // Lobby 구현이 안되어 있으므로 Main으로 진행
    }

    // 다시 시도 -> 무조건 Run 씬 부터
    public void OnClickRertry()
    {
        Time.timeScale = 1f;

        // 데이터 리셋
        if (RewardManager.Instance != null)
        {
            RewardManager.Instance.ResetStageData();
        }

        string currentScene = SceneManager.GetActiveScene().name; // 현재 실행중인 씬 이름
        string stageBaseName = currentScene.Split('_')[0]; // 스테이지 번호 추출
        PlayerHealth.CurrentHP = 100; // 플레이서 체력 초기화
        SceneManager.LoadScene($"{stageBaseName}_Run"); // Run 씬으로 이동
    }
}
