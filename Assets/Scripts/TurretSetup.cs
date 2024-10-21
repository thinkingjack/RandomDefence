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

    // ���� ������ Ÿ�� �������Ʈ�� �������� ��ȯ�ϴ� �Լ�
    public TurretBluePrint GetRandomNextLevelTurret(int currentLevel)
    {
        // ���� ������ �ش��ϴ� ��� TurretBluePrint�� ã��
        List<TurretBluePrint> nextLevelTurrets = new List<TurretBluePrint>();

        foreach (TurretBluePrint blueprint in turretBluePrints)
        {
            if (blueprint.level == currentLevel + 1)
            {
                nextLevelTurrets.Add(blueprint);
            }
        }

        // ���� ���� �ͷ��� ���� ���
        if (nextLevelTurrets.Count == 0)
        {
            Debug.LogWarning("No next level turret found for level: " + (currentLevel + 1));
            return null;
        }

        // ���� ���� �ͷ� �� �������� ����
        int randomIndex = Random.Range(0, nextLevelTurrets.Count);
        Debug.Log($"Selected next level turret: {nextLevelTurrets[randomIndex].prefab.name} at level {nextLevelTurrets[randomIndex].level}");

        return nextLevelTurrets[randomIndex];
    }

    // ���� ������ ���� �������� ���� �������Ʈ�� ��ȯ�ϴ� �Լ�
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