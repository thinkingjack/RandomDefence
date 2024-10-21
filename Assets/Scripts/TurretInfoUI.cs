using UnityEngine;
using UnityEngine.UI;

public class TurretInfoUI : MonoBehaviour
{
    public GameObject turretInfoPanel;     // �ͷ� ���� �г�
    public Text damageText;                // ���ݷ� �ؽ�Ʈ
    public Text fireRateText;              // ���� �ӵ� �ؽ�Ʈ
    public Text levelText;                     // �ͷ��� ���� �ؽ�Ʈ
    public LineRenderer rangeVisualizer;   // ���� ������ �ð������� ǥ���� LineRenderer
    public GameObject rangeFillObject;     // �� ���� ä��� ������Ʈ (���� Plane)

    private Turret selectedTurret;

    void Start()
    {
        turretInfoPanel.SetActive(false);  // ���� ���� �� ���� �г� ����
        rangeVisualizer.positionCount = 0; // LineRenderer �ʱ�ȭ
        rangeFillObject.SetActive(false);  // ���� ���� ������Ʈ ����
    }

    void Update()
    {
        // ���콺 Ŭ���� �����Ͽ� Ŭ���� Ÿ���� Ȯ��
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Turret")) // Ÿ���� Ŭ������ ��
                {
                    SelectTurret(hit.transform.GetComponent<Turret>());
                }
                else
                {
                    DeselectTurret(); // �ͷ� ���� ������Ʈ Ŭ�� �� ���� ����
                }
            }
            else
            {
                DeselectTurret(); // �� ������ Ŭ������ ���� ���� ����
            }
        }
    }

    // Ÿ���� �������� �� ���� ǥ��
    public void SelectTurret(Turret turret)
    {
        selectedTurret = turret;
        ShowTurretInfo();
    }

    // Ÿ�� ���� ǥ��
    public void ShowTurretInfo()
    {
        if (selectedTurret != null)
        {
            // ���ݷ�, ���� �ӵ� ������ �ؽ�Ʈ�� �Ҵ�
            damageText.text = selectedTurret.damage.ToString();
            fireRateText.text = selectedTurret.fireRate.ToString();

            // �ͷ��� ������ �ؽ�Ʈ�� �Ҵ�
            levelText.text = "LV." + selectedTurret.blueprint.level.ToString(); // �ͷ� ���� �߰�

            // �ͷ� ���� �г� Ȱ��ȭ
            turretInfoPanel.SetActive(true);

            // ���� ������ �ð������� ǥ��
            ShowRangeFill(selectedTurret.range);  // �� �� ä���
        }
    }

    // �ͷ� ���� ����
    public void DeselectTurret()
    {
        selectedTurret = null;
        turretInfoPanel.SetActive(false);  // ���� �г� �����
        rangeVisualizer.positionCount = 0; // LineRenderer �ʱ�ȭ
        rangeFillObject.SetActive(false);  // ���� ���� ������Ʈ �����
    }


    // �ͷ��� ���� ������ �ð������� ǥ�� (MeshRenderer�� �̿��� �� �� ä���)
    private void ShowRangeFill(float range)
    {
        rangeFillObject.transform.position = selectedTurret.transform.position; // �ͷ� ��ġ�� ���� �� ��ġ ����
        rangeFillObject.transform.localScale = new Vector3(range * 2, 0.01f, range * 2); // ũ�⸦ �ͷ��� ������ �°� ����
        rangeFillObject.SetActive(true); // ���� ���� ������Ʈ Ȱ��ȭ
    }
}