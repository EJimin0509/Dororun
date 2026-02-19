using UnityEngine;

// ID에 해당하는 스킬을 수행
public class SkillExecutor : MonoBehaviour
{
    public static SkillExecutor Instance;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 스킬 수행 메서드
    /// </summary>
    /// <param name="id"></param>
    public void ExecuteActive(int id)
    {
        switch (id)
        {
            case 1:
                // id 1 액티브 스킬
                break;
            case 2:
                // id 2 액티브 스킬
                break;
            case 3:
                // id 3 액티브 스킬
                break;
            case 4:
                // id 4 액티브 스킬
                break;
        }
    }

    /// <summary>
    /// 생성된 모든 적 탄환을 지우는 스킬 메서드
    /// </summary>
    void ExecuteMapClear()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (var b in bullets) Destroy(b);
    }
}
