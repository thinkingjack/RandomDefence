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
    public bool isCombining = false; // 터렛 합성 모드 상태
    public bool isSelling = false; // 터렛 판매 모드 상태
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

        
        // 선택한 블루프린트의 설치 비용을 가져옴
        int installCost = turretToBuild.prefab.GetComponent<Turret>().InstallCost;

        if (PlayerStats.Money < installCost)
        {
            Debug.Log("돈이 부족합니다");
            return;
        }

        // 설치 비용 지불
        PlayerStats.Money -= installCost;



        GameObject turret = Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);

        turret.transform.SetParent(node.transform); // 터렛의 부모를 현재 노드로 설정
        node.turret = turret;
        node.isOccupied = true;

        builtTurrets.Add(turret);

        GameObject effect = Instantiate(buildEffect, node.GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 0.6f);


        turretToBuild = null; // 타워를 생성한 후 선택된 타워 블루프린트를 초기화

        // 버튼을 다시 활성화하도록 Shop에 알림
        if (Shop.instance != null)
        {
            Shop.instance.SetButtonsActive(true, false, false);
            Shop.instance.ResetButtonColor(); // 클릭된 버튼의 색상을 초기화
        }
    }

    public void SelectTurretToBuild(TurretBluePrint turret)
    {
        turretToBuild = turret;
        isCombining = false; // 터렛 합성 모드 해제
        isSelling = false; // 터렛 판매 모드 해제
    }

    // 합성 모드 시작
    public void StartCombiningMode()
    {
        turretToBuild = null; // 터렛 생성 모드를 해제
        isCombining = true; // 터렛 합성 모드 활성화
        isSelling = false; // 판매 모드 해제

        // 터렛 생성 버튼과 합성 버튼을 비활성화
        if (Shop.instance != null)
        {
            Shop.instance.SetButtonsActive(false, true, false);
        }
    }

    // 판매 모드 시작
    public void StartSellingMode()
    {
        turretToBuild = null; // 터렛 생성 모드를 해제
        isCombining = false; // 터렛 합성 모드 해제
        isSelling = true; // 터렛 판매 모드 활성화

        // 터렛 생성 버튼과 합성 버튼을 비활성화
        if (Shop.instance != null)
        {
            Shop.instance.SetButtonsActive(false, false, true);
        }
    }

    // 빌드 모드 취소
    public void CancelBuildMode()
    {
        turretToBuild = null;
        isCombining = false;
        isSelling = false;

        // 버튼을 다시 활성화하도록 Shop에 알림
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

        // 선택된 노드의 터렛 컴포넌트를 가져옴
        Turret selectedTurret = node.turret.GetComponent<Turret>();
        if (selectedTurret == null)
        {
            return;
        }
        // 선택된 터렛의 블루프린트를 가져옴
        TurretBluePrint selectedBlueprint = selectedTurret.blueprint;

        // 다음 레벨 터렛 블루프린트를 랜덤으로 선택
        TurretBluePrint nextLevelBlueprint = TurretSetup.instance.GetRandomNextLevelTurret(selectedBlueprint.level);
        if (nextLevelBlueprint == null)
        {
            return;
        }

        // 현재 선택된 터렛과 같은 블루프린트를 가진 다른 노드들을 찾음
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
                    // 터렛의 블루프린트가 선택된 블루프린트와 동일한지 확인
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

        // 기존 터렛을 제거하고 다음 레벨의 새로운 터렛을 생성
        Vector3 upgradePosition = node.turret.transform.position;
        Destroy(node.turret);
        builtTurrets.Remove(node.turret);  // builtTurrets 리스트에서 제거
        GameObject newTurretObj = Instantiate(nextLevelBlueprint.prefab, upgradePosition, Quaternion.identity);
        Turret newTurret = newTurretObj.GetComponent<Turret>();
        newTurret.blueprint = nextLevelBlueprint;
        node.turret = newTurretObj;

        // 새로운 터렛의 부모를 현재 노드로 설정
        newTurretObj.transform.SetParent(node.transform);

        // 동일한 블루프린트를 가진 다른 터렛 중 하나를 제거
        Node otherNode = nodesWithSameBlueprint[Random.Range(0, nodesWithSameBlueprint.Count)];
        if (otherNode.turret != null)
        {
            builtTurrets.Remove(otherNode.turret);  // builtTurrets 리스트에서 제거
            otherNode.RemoveTurret();
        }

        // 새로 생성된 터렛을 builtTurrets 리스트에 추가
        builtTurrets.Add(newTurretObj);

        // 업그레이드 효과를 생성
        GameObject effect = Instantiate(buildEffect, upgradePosition, Quaternion.identity);
        Destroy(effect, 0.6f);


        // 합성 모드를 종료
        isCombining = false;

        // 버튼을 다시 활성화하도록 Shop에 알림
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


        // 터렛의 판매 가격을 가져옴
        int sellPrice = turret.SellPrice;

        PlayerStats.Money += sellPrice; // 판매 가격만큼 금액 반환



        // builtTurrets 리스트에서 터렛 제거
        if (builtTurrets.Contains(node.turret))
        {
            builtTurrets.Remove(node.turret);
        }

        Destroy(node.turret);
        node.turret = null;
        node.isOccupied = false;

        // 판매 효과를 생성
        GameObject effect = Instantiate(buildEffect, node.GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 0.6f);


        // 판매 모드를 종료
        isSelling = false;

        // 버튼을 다시 활성화하도록 Shop에 알림
        if (Shop.instance != null)
        {
            Shop.instance.EndSellingMode();
        }
    }
}