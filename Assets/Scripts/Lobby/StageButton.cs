using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    public int StageIndex; // 스테이지 번호
    public GameObject LockViusal; // 잠금 이미지

    private Button btn; // 버튼

    private void Awake()
    {
        btn = GetComponent<Button>();
    }

    /// <summary>
    /// 스테이지 해금 여부에 따라 버튼 상태 설정 메서드
    /// </summary>
    /// <param name="isUnlocked"></param>
    public void SetStageStatus(bool isUnlocked)
    {
        if (btn == null) btn = GetComponent<Button>();

        btn.interactable = isUnlocked; // 해금 여부에 따라 클릭 가능/불가능

        if(LockViusal != null)
        {
            LockViusal.SetActive(!isUnlocked); // 잠금 비주얼 켜기/끄기
        }
    }

    public void OnClickStage()
    {
        Object.FindAnyObjectByType<LobbyManager>().RequestStageStart(StageIndex);
    }
}
