using UnityEngine;

public class SupporterCollecter : MonoBehaviour
{
    // 같은 스크립트라도 개별적으로 적용되어야 함 -> 한 스테이지에 여러 서포터 존재 가능
    // 플레이어에 의해 수집
    // RewardManager에 수집 정보 전달
    // 오브젝트 제거
    // 다시 씬을 로드할 때, 이미 수집했다면, 제거된 상태로 시작되어야 함
    // ResultUI에 수집한 서포터의 스프라이트가 표시되어야 함
    // Lobby 씬에서 수집된 서포터는 활성화되어야 함: 초기 수집 전 스프라이트 -> 수집 완료 스프라이트 + 상호작용 가능(추후 예정)
    [Header("Supporter Info")]
    public int supporterID; // 서포터 고유 ID
    public Sprite supporterSprite; // 서포터 스프라이트

    private void Start()
    {
        if (PlayerPrefs.GetInt("Supporter_" + supporterID, 0) == 1) Destroy(gameObject); // 이미 수집된 서포터라면 제거
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(RewardManager.Instance != null)
            {
                RewardManager.Instance.CollectSupporter(supporterID, supporterSprite); // RewardManager에 수집 정보 전달
            }

            gameObject.SetActive(false); // 오브젝트 비활성화
        }
    }
}
