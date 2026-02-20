using UnityEngine;

// 각 서포터의 스킬 수치와 쿨타임 정보를 관리
// 씬에 존재하거나 Resources에서 참조할 예정

public class SupporterDB : MonoBehaviour
{
    public static SupporterDB Instance;
    public SupporterData[] allSuporters; // 모든 서포터

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        allSuporters = Resources.LoadAll<SupporterData>("");
    }

    public SupporterData GetSupporter(int id)
    {
        foreach (var s in allSuporters) if (s.ID == id) return s;
        return null;
    }
}
