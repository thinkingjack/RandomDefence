using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public GameObject buildEffect;
    private TurretBluePrint turretToBuild;
    public bool isCombining = false; // �ͷ� �ռ� ��� ����
    public bool isSelling = false; // �ͷ� �Ǹ� ��� ����
    public List<GameObject> builtTurrets = new List<GameObject>();

    public bool CanBuild { get { return turretToBuild != null; } }
    public bool HasMoney { get { return PlayerStats.Money >= (turretToBuild != null ? turretToBuild.cost : 0); } }

    public void BuildTurretOn(Node node)
    {
        if (isCombining)
        {
            TryRandomCombineTurrets(node);
            return;
        }

        if (turretToBuild == null)
        {
            return;
        }

        
        // ������ �������Ʈ�� ��ġ ����� ������
        int installCost = turretToBuild.prefab.GetComponent<Turret>().InstallCost;

        if (PlayerStats.Money < installCost)
        {
            Debug.Log("���� �����մϴ�");
            return;
        }

        // ��ġ ��� ����
        PlayerStats.Money -= installCost;



        GameObject turret = Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);

        turret.transform.SetParent(node.transform); // �ͷ��� �θ� ���� ���� ����
        node.turret = turret;
        node.isOccupied = true;

        builtTurrets.Add(turret);

        GameObject effect = Instantiate(buildEffect, node.GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 0.6f);


        turretToBuild = null; // Ÿ���� ������ �� ���õ� Ÿ�� �������Ʈ�� �ʱ�ȭ

        // ��ư�� �ٽ� Ȱ��ȭ�ϵ��� Shop�� �˸�
        if (Shop.instance != null)
        {
            Shop.instance.SetButtonsActive(true, false, false);
            Shop.instance.ResetButtonColor(); // Ŭ���� ��ư�� ������ �ʱ�ȭ
        }
    }

    public void SelectTurretToBuild(TurretBluePrint turret)
    {
        turretToBuild = turret;
        isCombining = false; // �ͷ� �ռ� ��� ����
        isSelling = false; // �ͷ� �Ǹ� ��� ����
    }

    // �ռ� ��� ����
    public void StartCombiningMode()
    {
        turretToBuild = null; // �ͷ� ���� ��带 ����
        isCombining = true; // �ͷ� �ռ� ��� Ȱ��ȭ
        isSelling = false; // �Ǹ� ��� ����

        // �ͷ� ���� ��ư�� �ռ� ��ư�� ��Ȱ��ȭ
        if (Shop.instance != null)
        {
            Shop.instance.SetButtonsActive(false, true, false);
        }
    }

    // �Ǹ� ��� ����
    public void StartSellingMode()
    {
        turretToBuild = null; // �ͷ� ���� ��带 ����
        isCombining = false; // �ͷ� �ռ� ��� ����
        isSelling = true; // �ͷ� �Ǹ� ��� Ȱ��ȭ

        // �ͷ� ���� ��ư�� �ռ� ��ư�� ��Ȱ��ȭ
        if (Shop.instance != null)
        {
            Shop.instance.SetButtonsActive(false, false, true);
        }
    }

    // ���� ��� ���
    public void CancelBuildMode()
    {
        turretToBuild = null;
        isCombining = false;
        isSelling = false;

        // ��ư�� �ٽ� Ȱ��ȭ�ϵ��� Shop�� �˸�
        if (Shop.instance != null)
        {
            Shop.instance.SetButtonsActive(true, false, false);
        }
    }

    public void TryRandomCombineTurrets(Node node)
    {
        if (node.turret == null)
        {
            Debug.Log("No turret to combine on this node");
            return;
        }

        // ���õ� ����� �ͷ� ������Ʈ�� ������
        Turret selectedTurret = node.turret.GetComponent<Turret>();
        if (selectedTurret == null)
        {
            return;
        }
        // ���õ� �ͷ��� �������Ʈ�� ������
        TurretBluePrint selectedBlueprint = selectedTurret.blueprint;

        // ���� ���� �ͷ� �������Ʈ�� �������� ����
        TurretBluePrint nextLevelBlueprint = TurretSetup.instance.GetRandomNextLevelTurret(selectedBlueprint.level);
        if (nextLevelBlueprint == null)
        {
            return;
        }

        // ���� ���õ� �ͷ��� ���� �������Ʈ�� ���� �ٸ� ������ ã��
        List<Node> nodesWithSameBlueprint = new List<Node>();
        foreach (var builtTurret in builtTurrets)
        {
            if (builtTurret == null) continue;

            Node nodeWithTurret = builtTurret.GetComponentInParent<Node>();
            if (nodeWithTurret != null && nodeWithTurret != node && nodeWithTurret.turret != null)
            {
                Turret turret = nodeWithTurret.turret.GetComponent<Turret>();
                if (turret != null)
                {
                    // �ͷ��� �������Ʈ�� ���õ� �������Ʈ�� �������� Ȯ��
                    if (turret.blueprint.prefab.name == selectedBlueprint.prefab.name)
                    {
                        nodesWithSameBlueprint.Add(nodeWithTurret);
                    }
                }
            }
        }

        if (nodesWithSameBlueprint.Count == 0)
        {
            return;
        }

        // ���� �ͷ��� �����ϰ� ���� ������ ���ο� �ͷ��� ����
        Vector3 upgradePosition = node.turret.transform.position;
        Destroy(node.turret);
        builtTurrets.Remove(node.turret);  // builtTurrets ����Ʈ���� ����
        GameObject newTurretObj = Instantiate(nextLevelBlueprint.prefab, upgradePosition, Quaternion.identity);
        Turret newTurret = newTurretObj.GetComponent<Turret>();
        newTurret.blueprint = nextLevelBlueprint;
        node.turret = newTurretObj;

        // ���ο� �ͷ��� �θ� ���� ���� ����
        newTurretObj.transform.SetParent(node.transform);

        // ������ �������Ʈ�� ���� �ٸ� �ͷ� �� �ϳ��� ����
        Node otherNode = nodesWithSameBlueprint[Random.Range(0, nodesWithSameBlueprint.Count)];
        if (otherNode.turret != null)
        {
            builtTurrets.Remove(otherNode.turret);  // builtTurrets ����Ʈ���� ����
            otherNode.RemoveTurret();
        }

        // ���� ������ �ͷ��� builtTurrets ����Ʈ�� �߰�
        builtTurrets.Add(newTurretObj);

        // ���׷��̵� ȿ���� ����
        GameObject effect = Instantiate(buildEffect, upgradePosition, Quaternion.identity);
        Destroy(effect, 0.6f);


        // �ռ� ��带 ����
        isCombining = false;

        // ��ư�� �ٽ� Ȱ��ȭ�ϵ��� Shop�� �˸�
        if (Shop.instance != null)
        {
            Shop.instance.EndCombiningMode();
        }
    }

    public void SellTurret(Node node)
    {
        if (!isSelling)
        {
            return;
        }

        if (node.turret == null)
        {
            return;
        }

        Turret turret = node.turret.GetComponent<Turret>();
        if (turret == null)
        {
            return;
        }


        // �ͷ��� �Ǹ� ������ ������
        int sellPrice = turret.SellPrice;

        PlayerStats.Money += sellPrice; // �Ǹ� ���ݸ�ŭ �ݾ� ��ȯ



        // builtTurrets ����Ʈ���� �ͷ� ����
        if (builtTurrets.Contains(node.turret))
        {
            builtTurrets.Remove(node.turret);
        }

        Destroy(node.turret);
        node.turret = null;
        node.isOccupied = false;

        // �Ǹ� ȿ���� ����
        GameObject effect = Instantiate(buildEffect, node.GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 0.6f);


        // �Ǹ� ��带 ����
        isSelling = false;

        // ��ư�� �ٽ� Ȱ��ȭ�ϵ��� Shop�� �˸�
        if (Shop.instance != null)
        {
            Shop.instance.EndSellingMode();
        }
    }
}