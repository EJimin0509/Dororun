using UnityEngine;

[CreateAssetMenu(fileName = "SupporterData", menuName = "Scriptable Objects/SupporterData")]
public class SupporterData : ScriptableObject
{
    public int ID; // 서포터 ID
    public Sprite Icon; // 서포터 아이콘
    public float ActiveDuration; // 스킬 활성화 시간
    public int Cooldown; // 쿨타임
}
