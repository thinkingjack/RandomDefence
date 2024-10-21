using System.Collections.Generic;
using UnityEngine;

public class TurretSetup : MonoBehaviour
{
    public static TurretSetup instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public List<TurretBluePrint> turretBluePrints;

    // 다음 레벨의 타워 블루프린트를 랜덤으로 반환하는 함수
    public TurretBluePrint GetRandomNextLevelTurret(int currentLevel)
    {
        // 다음 레벨에 해당하는 모든 TurretBluePrint를 찾기
        List<TurretBluePrint> nextLevelTurrets = new List<TurretBluePrint>();

        foreach (TurretBluePrint blueprint in turretBluePrints)
        {
            if (blueprint.level == currentLevel + 1)
            {
                nextLevelTurrets.Add(blueprint);
            }
        }

        // 다음 레벨 터렛이 없는 경우
        if (nextLevelTurrets.Count == 0)
        {
            Debug.LogWarning("No next level turret found for level: " + (currentLevel + 1));
            return null;
        }

        // 다음 레벨 터렛 중 랜덤으로 선택
        int randomIndex = Random.Range(0, nextLevelTurrets.Count);
        Debug.Log($"Selected next level turret: {nextLevelTurrets[randomIndex].prefab.name} at level {nextLevelTurrets[randomIndex].level}");

        return nextLevelTurrets[randomIndex];
    }

    // 같은 레벨과 같은 프리펩을 가진 블루프린트를 반환하는 함수
    public List<TurretBluePrint> GetSameLevelAndPrefabBluePrints(TurretBluePrint blueprint)
    {
        List<TurretBluePrint> matchingBlueprints = new List<TurretBluePrint>();

        foreach (TurretBluePrint bp in turretBluePrints)
        {
            if (bp.level == blueprint.level && bp.prefab == blueprint.prefab)
            {
                matchingBlueprints.Add(bp);
            }
        }

        return matchingBlueprints;
    }
}