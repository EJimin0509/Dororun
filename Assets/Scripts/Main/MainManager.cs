using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    // start
    public void OnClickStart()
    {
        // 씬 전환 시간 정상화
        Time.timeScale = 1f;

        // Stage1_Run 로드
        SceneManager.LoadScene("Stage1_Run");
    }

    // exit
    public void OnClickExit()
    {
        Debug.Log("게임 종료!");
        // 게임 종료 로직
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 실행 중일 때 종료
        #else
            Application.Quit(); // 실제 빌드된 게임에서 종료
        #endif
    }
}