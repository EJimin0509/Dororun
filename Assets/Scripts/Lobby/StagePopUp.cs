using UnityEngine;
using UnityEngine.SceneManagement;

public class StagePopUp : MonoBehaviour
{
    private int targetStageIndex; // 목표 스테이지 번호

    /// <summary>
    /// 팝업이 열려고, 스테이지 번호 전달 메서드
    /// </summary>
    /// <param name="stageIndex"></param>
    public void OpenPopup(int stageIndex)
    {
        targetStageIndex = stageIndex;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// YES 버튼 메서드
    /// </summary>
    public void OnClickYes()
    {
        string sceneName = "Stage" + targetStageIndex + "_Run"; // 러닝 씬 Stage#_Run
        SceneManager.LoadScene(sceneName);
    }


    /// <summary>
    /// NO 버튼 메서드
    /// </summary>
    public void OnClickNo()
    {
        gameObject.SetActive(false);
    }
}
