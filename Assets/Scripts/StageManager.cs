using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class StageManager : MonoBehaviour
{
    [Header("Distance Settings")]
    public Slider distanceSlider; // 거리 슬라이더 UI
    public Transform worldTransform; // Grid 오브젝트
    public Tilemap groundTilemap; // 타일맵 연결 부분

    [Header("Scene Switch Setting")]
    public WorldScroller[] scrollers; // 모든 배경/타일맵 스크롤러 배열
    public PlayerController player; // 플레이어 컨트롤러 참조

    private float startX; // 시작 위치 X 좌표
    private bool isGoalReached = false; // 목표 도달 여부
    private bool isSequenceStarted = false; // 시퀀스 시작 여부

    void Start()
    {
        float goalDistance = CalculateMapLength(); // 맵 길이 계산
        if (worldTransform != null)
        {
            startX = worldTransform.position.x; // 시작 위치 저장

            distanceSlider.maxValue = goalDistance;
            distanceSlider.value = 0;
        }
    }

    // 맵 길이 계산 메서드
    float CalculateMapLength()
    {
        BoundsInt bounds = groundTilemap.cellBounds; // 타일맵의 경계 가져오기
        int tileCountX = bounds.size.x; // X축 타일 개수
        float cellSizeX = groundTilemap.layoutGrid.cellSize.x; // 셀 크기
        float goalDistance = tileCountX * cellSizeX; // 목표 거리 계산
        return goalDistance;
    }

    void Update()
    {
        if (isGoalReached) return; // 목표 도달 시 업데이트 중지

        float travelledDistance = startX - worldTransform.position.x; // 이동한 거리 계산
        distanceSlider.value = travelledDistance; // 슬라이더 값 업데이트

        if (travelledDistance >= distanceSlider.maxValue) // 목표 도달 여부 확인
        {
            StartExitSequence();
        }
    }

    // 종료 시퀀스 시작 메서드
    void StartExitSequence()
    {
        isSequenceStarted = true;
        foreach (var s in scrollers)
        {
            s.enabled = false; // 모든 스크롤러 비활성화
        }

        player.StartExitAnimation(); // 플레이어 종료 애니메이션 시작
    }

    // 보스 전투로 전환 메서드
    public void SwitchToBossBattle()
    {
        Debug.Log("Switching to Boss Battle Scene...");
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Stage1_Boss");
    }
}
