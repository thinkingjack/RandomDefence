using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public List<TurretBluePrint> turretBlueprints; // ���� Ÿ�� �������Ʈ�� ������ ����Ʈ
    public Button selectRandomButton; // �ͷ� ���� ��ư�� ������ ����
    public Button combineButton; // �ռ� ��ư�� ������ ����
    public Button sellButton; // �Ǹ� ��ư�� ������ ����

    private BuildManager buildManager;
    private bool isButtonActive = true;
    private bool isCombiningModeActive = false;
    private bool isSellingModeActive = false;

    private Color defaultColor = Color.white; // ��ư�� �⺻ ����
    private Color selectedColor = Color.yellow; // ���õ� ��ư�� ����
    private float defaultColorMultiplier = 1f; // �⺻ ColorMultiplier
    private float disabledColorMultiplier = 5f; // ��Ȱ��ȭ�� ��ư�� ColorMultiplier

    public static Shop instance; // Shop�� �ν��Ͻ��� �����ϱ� ���� ����

    private Button lastClickedButton; // ���������� Ŭ���� ��ư�� ����

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        buildManager = BuildManager.instance;
        selectRandomButton.onClick.AddListener(() => OnButtonClicked(selectRandomButton));
        combineButton.onClick.AddListener(() => OnButtonClicked(combineButton));
        sellButton.onClick.AddListener(() => OnButtonClicked(sellButton));
        SetButtonsActive(true, false, false); // �ʱ⿡�� ��� ��ư�� Ȱ��ȭ��
    }

    void Update()
    {
        // ESC Ű�� ������ ��ư�� Ȱ��ȭ
        if (!isButtonActive && Input.GetKeyDown(KeyCode.Escape))
        {
            SetButtonsActive(true, false, false); // ��ư�� Ȱ��ȭ�ϰ� �ռ� ��� �� �Ǹ� ��带 ��Ȱ��ȭ
            buildManager.CancelBuildMode(); // ���� ��� ���
            ResetButtonColors(); // ��� ��ư�� ������ �⺻ �������� �ʱ�ȭ
            ResetColorMultipliers(); // ��� ��ư�� ColorMultiplier�� �ʱ�ȭ
        }
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    private void OnButtonClicked(Button clickedButton)
    {
        if (!isButtonActive || isCombiningModeActive || isSellingModeActive) return; // ��ư�� ��Ȱ��ȭ�Ǿ��ų� �ռ� ��� �Ǵ� �Ǹ� ����� ���� �ƹ� ���۵� ���� ����

        // ��ư Ŭ���� ���� ����� �ٸ��� ó��
        if (clickedButton == selectRandomButton)
        {
            OnSelectRandomButtonClicked();
        }
        else if (clickedButton == combineButton)
        {
            OnCombineButtonClicked();
        }
        else if (clickedButton == sellButton)
        {
            OnSellButtonClicked();
        }

        // Ŭ���� ��ư�� ��������� �����ϰ� ���������� Ŭ���� ��ư���� ����
        SetButtonColor(clickedButton, selectedColor);
        lastClickedButton = clickedButton;
    }

    // ���� Ÿ���� �����ϰ� �Ǽ��ϴ� �Լ�
    private void OnSelectRandomButtonClicked()
    {
        TurretBluePrint randomTurret = GetRandomTurretBluePrint();
        if (randomTurret != null)
        {
            buildManager.SelectTurretToBuild(randomTurret);
            Debug.Log("Random turret selected: " + randomTurret.prefab.name);

            // ��ư ��Ȱ��ȭ
            SetButtonsActive(false, isCombiningModeActive, isSellingModeActive);
        }
    }

    // �ռ� ��ư Ŭ�� �� ȣ��
    private void OnCombineButtonClicked()
    {
        buildManager.StartCombiningMode();
        isCombiningModeActive = true;

        // ��ư ��Ȱ��ȭ
        SetButtonsActive(false, true, false);
    }

    // �Ǹ� ��ư Ŭ�� �� ȣ��
    private void OnSellButtonClicked()
    {
        buildManager.StartSellingMode();
        isSellingModeActive = true;

        // ��ư ��Ȱ��ȭ
        SetButtonsActive(false, false, true);
    }

    // ���� Ÿ�� �������Ʈ�� ��ȯ�ϴ� �Լ�
    private TurretBluePrint GetRandomTurretBluePrint()
    {
        if (turretBlueprints == null || turretBlueprints.Count == 0)
        {
            Debug.LogWarning("No turret blueprints available!");
            return null;
        }

        int randomIndex = Random.Range(0, turretBlueprints.Count);
        return turretBlueprints[randomIndex];
    }

    // Ư�� ��ư�� ������ �����ϴ� �Լ�
    private void SetButtonColor(Button button, Color color)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = color;
        }
    }

    // ��� ��ư�� ������ �⺻ �������� �ǵ����� �Լ�
    private void ResetButtonColors()
    {
        SetButtonColor(selectRandomButton, defaultColor);
        SetButtonColor(combineButton, defaultColor);
        SetButtonColor(sellButton, defaultColor);
    }

    // ��ư ���� ���� �Լ� (�ܺο��� ȣ���)
    public void ResetButtonColor()
    {
        if (lastClickedButton != null)
        {
            SetButtonColor(lastClickedButton, defaultColor);
            lastClickedButton = null; // ������ Ŭ���� ��ư �ʱ�ȭ
        }
    }

    // ��ư Ȱ��ȭ �� ��Ȱ��ȭ ���� �Լ�
    public void SetButtonsActive(bool isActive, bool isCombiningMode, bool isSellingMode)
    {
        SetButtonInteractable(selectRandomButton, isActive && !isCombiningMode && !isSellingMode);
        SetButtonInteractable(combineButton, isActive && !isCombiningMode && !isSellingMode);
        SetButtonInteractable(sellButton, isActive && !isCombiningMode && !isSellingMode);
        isButtonActive = isActive;
        isCombiningModeActive = isCombiningMode;
        isSellingModeActive = isSellingMode;
    }

    // ��ư�� ��ȣ�ۿ� ���� ���θ� �����ϰ�, ��Ȱ��ȭ�� ��� ColorMultiplier�� �����ϴ� �Լ�
    private void SetButtonInteractable(Button button, bool isInteractable)
    {
        button.interactable = isInteractable;

        ColorBlock colorBlock = button.colors;
        colorBlock.colorMultiplier = isInteractable ? defaultColorMultiplier : disabledColorMultiplier;
        button.colors = colorBlock;
    }

    // ��� ��ư�� ColorMultiplier�� �⺻������ �ǵ����� �Լ�
    private void ResetColorMultipliers()
    {
        SetColorMultiplier(selectRandomButton, defaultColorMultiplier);
        SetColorMultiplier(combineButton, defaultColorMultiplier);
        SetColorMultiplier(sellButton, defaultColorMultiplier);
    }

    // Ư�� ��ư�� ColorMultiplier�� �����ϴ� �Լ�
    private void SetColorMultiplier(Button button, float multiplier)
    {
        ColorBlock colorBlock = button.colors;
        colorBlock.colorMultiplier = multiplier;
        button.colors = colorBlock;
    }

    // �ռ� ��� ���� �� ȣ��
    public void EndCombiningMode()
    {
        isCombiningModeActive = false;
        SetButtonsActive(true, false, false); // ��ư�� �ٽ� Ȱ��ȭ
        ResetButtonColors(); // ��� ��ư�� ������ �⺻ �������� �ʱ�ȭ
        ResetColorMultipliers(); // ��� ��ư�� ColorMultiplier�� �ʱ�ȭ
        ResetButtonColor(); // ������ Ŭ���� ��ư�� ���� ����
    }

    // �Ǹ� ��� ���� �� ȣ��
    public void EndSellingMode()
    {
        isSellingModeActive = false;
        SetButtonsActive(true, false, false); // ��ư�� �ٽ� Ȱ��ȭ
        ResetButtonColors(); // ��� ��ư�� ������ �⺻ �������� �ʱ�ȭ
        ResetColorMultipliers(); // ��� ��ư�� ColorMultiplier�� �ʱ�ȭ
        ResetButtonColor(); // ������ Ŭ���� ��ư�� ���� ����
    }
}