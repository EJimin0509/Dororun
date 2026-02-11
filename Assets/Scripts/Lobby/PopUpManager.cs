using UnityEngine;
using UnityEngine.InputSystem;

public class PopUpManager : MonoBehaviour
{
    // 팝업창 제어
    public void Open() => gameObject.SetActive(true); // 열기
    public void Close() => gameObject.SetActive(false); // 닫기 -> ESC 키 및 팝업 바깥 배경 버튼에 연결

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.escapeKey.wasPressedThisFrame) Close(); // ESC 누르면 Close
    }
}
