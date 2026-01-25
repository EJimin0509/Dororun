using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossStageManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject bossStartPanel; // 컨테이너
    public GameObject warningText; // 워닝 텍스트 이미지
    public GameObject warningTop; // 워닝 위에 위치할 이미지

    [Header("Looping Background")]
    public RawImage loopImage; // 루프 스크롤 될 이미지
    public float scrollSpeed = 0.5f; // 스크롤 스피드

    private bool isScrolling = false; // 스크롤 여부

    void Start()
    {
        // 시작 시 시간 정지
        Time.timeScale = 0f;
        StartCoroutine(StartBossIntro());
    }

    void Update()
    {
        // 시간 정지 중에도 배경은 움직여야 함
        if (isScrolling && loopImage != null)
        {
            Rect rect = loopImage.uvRect;
            // 오른쪽으로 스크롤
            rect.x -= scrollSpeed * Time.unscaledDeltaTime;
            loopImage.uvRect = rect;
        }
    }

    // 보스전 시작 UI
    IEnumerator StartBossIntro()
    {
        bossStartPanel.SetActive(true);

        // 3개의 이미지 동시 활성화
        warningText.SetActive(true);
        warningTop.SetActive(true);
        loopImage.gameObject.SetActive(true);

        isScrolling = true;

        // 대기
        yield return new WaitForSecondsRealtime(3f);

        // 연출 종료
        isScrolling = false;
        bossStartPanel.SetActive(false);

        // 재생
        Time.timeScale = 1f;
    }
}