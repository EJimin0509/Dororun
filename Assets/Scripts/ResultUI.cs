using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ResultUI : MonoBehaviour
{
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

        // 수집한 서포터 표시
        for (int i = 0; i < RewardManager.Instance.collectedSupporterSprites.Count; i++)
        {
            
        }

        Time.timeScale = 0f; // 시간 정지
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
