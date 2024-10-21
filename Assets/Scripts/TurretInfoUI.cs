using UnityEngine;
using UnityEngine.UI;

public class TurretInfoUI : MonoBehaviour
{
    public GameObject turretInfoPanel;     // 터렛 정보 패널
    public Text damageText;                // 공격력 텍스트
    public Text fireRateText;              // 공격 속도 텍스트
    public Text levelText;                     // 터렛의 레벨 텍스트
    public LineRenderer rangeVisualizer;   // 공격 범위를 시각적으로 표시할 LineRenderer
    public GameObject rangeFillObject;     // 원 안을 채우는 오브젝트 (원형 Plane)

    private Turret selectedTurret;

    void Start()
    {
        turretInfoPanel.SetActive(false);  // 게임 시작 시 정보 패널 숨김
        rangeVisualizer.positionCount = 0; // LineRenderer 초기화
        rangeFillObject.SetActive(false);  // 원형 범위 오브젝트 숨김
    }

    void Update()
    {
        // 마우스 클릭을 감지하여 클릭된 타워를 확인
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Turret")) // 타워를 클릭했을 때
                {
                    SelectTurret(hit.transform.GetComponent<Turret>());
                }
                else
                {
                    DeselectTurret(); // 터렛 외의 오브젝트 클릭 시 선택 해제
                }
            }
            else
            {
                DeselectTurret(); // 빈 공간을 클릭했을 때도 선택 해제
            }
        }
    }

    // 타워를 선택했을 때 정보 표시
    public void SelectTurret(Turret turret)
    {
        selectedTurret = turret;
        ShowTurretInfo();
    }

    // 타워 정보 표시
    public void ShowTurretInfo()
    {
        if (selectedTurret != null)
        {
            // 공격력, 공격 속도 정보를 텍스트에 할당
            damageText.text = selectedTurret.damage.ToString();
            fireRateText.text = selectedTurret.fireRate.ToString();

            // 터렛의 레벨을 텍스트에 할당
            levelText.text = "LV." + selectedTurret.blueprint.level.ToString(); // 터렛 레벨 추가

            // 터렛 정보 패널 활성화
            turretInfoPanel.SetActive(true);

            // 공격 범위를 시각적으로 표시
            ShowRangeFill(selectedTurret.range);  // 원 안 채우기
        }
    }

    // 터렛 선택 해제
    public void DeselectTurret()
    {
        selectedTurret = null;
        turretInfoPanel.SetActive(false);  // 정보 패널 숨기기
        rangeVisualizer.positionCount = 0; // LineRenderer 초기화
        rangeFillObject.SetActive(false);  // 원형 범위 오브젝트 숨기기
    }


    // 터렛의 공격 범위를 시각적으로 표시 (MeshRenderer를 이용해 원 안 채우기)
    private void ShowRangeFill(float range)
    {
        rangeFillObject.transform.position = selectedTurret.transform.position; // 터렛 위치에 맞춰 원 위치 설정
        rangeFillObject.transform.localScale = new Vector3(range * 2, 0.01f, range * 2); // 크기를 터렛의 범위에 맞게 설정
        rangeFillObject.SetActive(true); // 원형 범위 오브젝트 활성화
    }
}