using UnityEngine;

// 각 서포터의 스킬 수치와 쿨타임 정보를 관리
// 씬에 존재하거나 Resources에서 참조할 예정
[System.Serializable]
public class SupporterData
{
    public int ID; // 서포터 ID
    public Sprite Icon; // 서포터 아이콘
    public float ActiveDuration; // 스킬 활성화 시간
    public float Cooldown; // 쿨타임
}

public class SupporterDB : MonoBehaviour
{
    public static SupporterDB Instance;
    public SupporterData[] allSuporters; // 모든 서포터

    private void Awake()
    {
        Instance = this;
    }

    public SupporterData GetSupporter(int id)
    {
        foreach (var s in allSuporters) if (s.ID == id) return s;
        return null;
    }
}
