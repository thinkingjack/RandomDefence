using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public TurretDatabase turretDatabase; //ScriptableObject 참조
    public Button selectRandomButton; // 터렛 생성 버튼을 참조할 변수
    public Button combineButton; // 합성 버튼을 참조할 변수
    public Button sellButton; // 판매 버튼을 참조할 변수

    private BuildManager buildManager;
    private bool isButtonActive = true;
    private bool isCombiningModeActive = false;
    private bool isSellingModeActive = false;

    private Color defaultColor = Color.white; // 버튼의 기본 색상
    private Color selectedColor = Color.yellow; // 선택된 버튼의 색상
    private float defaultColorMultiplier = 1f; // 기본 ColorMultiplier
    private float disabledColorMultiplier = 5f; // 비활성화된 버튼의 ColorMultiplier

    public static Shop instance; // Shop의 인스턴스를 참조하기 위한 변수

    private Button lastClickedButton; // 마지막으로 클릭된 버튼을 저장

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
        SetButtonsActive(true, false, false); // 초기에는 모든 버튼이 활성화됨
    }

    void Update()
    {
        // PC / Editor ESC
        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelMode();
            }
        }

        // Android 뒤로가기 버튼
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelMode();
            }
        }
    }

    private void CancelMode()
    {
        buildManager.CancelBuildMode();           // 빌드/합성/판매 모드 취소
        SetButtonsActive(true, false, false);     // 버튼 활성화 초기화
        ResetButtonColors();
        ResetColorMultipliers();
    }
    // 버튼 클릭 시 호출되는 함수
    private void OnButtonClicked(Button clickedButton)
    {
        if (!isButtonActive || isCombiningModeActive || isSellingModeActive) return; // 버튼이 비활성화되었거나 합성 모드 또는 판매 모드일 때는 아무 동작도 하지 않음

        // 버튼 클릭에 따라 기능을 다르게 처리
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

        // 클릭된 버튼을 노란색으로 설정하고 마지막으로 클릭된 버튼으로 저장
        SetButtonColor(clickedButton, selectedColor);
        lastClickedButton = clickedButton;
    }

    // 랜덤 타워를 선택하고 건설하는 함수
    private void OnSelectRandomButtonClicked()
    {
        int desiredLevel = 1; // 원하는 레벨 설정
        TurretBluePrint randomTurret = GetRandomTurretBluePrint(desiredLevel);
        if (randomTurret != null)
        {
            buildManager.SelectTurretToBuild(randomTurret);
            Debug.Log("Random turret selected: " + randomTurret.prefab.name + " at level " + randomTurret.level);

            SetButtonsActive(false, isCombiningModeActive, isSellingModeActive);
        }
    }

    // 합성 버튼 클릭 시 호출
    private void OnCombineButtonClicked()
    {
        buildManager.StartCombiningMode();
        isCombiningModeActive = true;

        // 버튼 비활성화
        SetButtonsActive(false, true, false);
    }

    // 판매 버튼 클릭 시 호출
    private void OnSellButtonClicked()
    {
        buildManager.StartSellingMode();
        isSellingModeActive = true;

        // 버튼 비활성화
        SetButtonsActive(false, false, true);
    }
    // 랜덤 타워 블루프린트를 반환하는 함수
    private TurretBluePrint GetRandomTurretBluePrint(int desiredLevel)
    {
        if (turretDatabase == null || turretDatabase.turretList.Count == 0)
        {
            Debug.LogWarning("No turret blueprints available!");
            return null;
        }

        // 원하는 레벨에 해당하는 블루프린트만 필터링
        List<TurretBluePrint> levelFiltered = new List<TurretBluePrint>();
        foreach (var bp in turretDatabase.turretList)
        {
            if (bp.level == desiredLevel)
            {
                levelFiltered.Add(bp);
            }
        }

        if (levelFiltered.Count == 0)
        {
            Debug.LogWarning("No turret blueprints found for level: " + desiredLevel);
            return null;
        }

        int randomIndex = Random.Range(0, levelFiltered.Count);
        return levelFiltered[randomIndex];
    }
    // 특정 버튼의 색상을 설정하는 함수
    private void SetButtonColor(Button button, Color color)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = color;
        }
    }

    // 모든 버튼의 색상을 기본 색상으로 되돌리는 함수
    private void ResetButtonColors()
    {
        SetButtonColor(selectRandomButton, defaultColor);
        SetButtonColor(combineButton, defaultColor);
        SetButtonColor(sellButton, defaultColor);
    }

    // 버튼 색상 복원 함수 (외부에서 호출됨)
    public void ResetButtonColor()
    {
        if (lastClickedButton != null)
        {
            SetButtonColor(lastClickedButton, defaultColor);
            lastClickedButton = null; // 마지막 클릭된 버튼 초기화
        }
    }

    // 버튼 활성화 및 비활성화 설정 함수
    public void SetButtonsActive(bool isActive, bool isCombiningMode, bool isSellingMode)
    {
        SetButtonInteractable(selectRandomButton, isActive && !isCombiningMode && !isSellingMode);
        SetButtonInteractable(combineButton, isActive && !isCombiningMode && !isSellingMode);
        SetButtonInteractable(sellButton, isActive && !isCombiningMode && !isSellingMode);
        isButtonActive = isActive;
        isCombiningModeActive = isCombiningMode;
        isSellingModeActive = isSellingMode;
    }

    // 버튼의 상호작용 가능 여부를 설정하고, 비활성화된 경우 ColorMultiplier를 변경하는 함수
    private void SetButtonInteractable(Button button, bool isInteractable)
    {
        button.interactable = isInteractable;

        ColorBlock colorBlock = button.colors;
        colorBlock.colorMultiplier = isInteractable ? defaultColorMultiplier : disabledColorMultiplier;
        button.colors = colorBlock;
    }

    // 모든 버튼의 ColorMultiplier를 기본값으로 되돌리는 함수
    private void ResetColorMultipliers()
    {
        SetColorMultiplier(selectRandomButton, defaultColorMultiplier);
        SetColorMultiplier(combineButton, defaultColorMultiplier);
        SetColorMultiplier(sellButton, defaultColorMultiplier);
    }

    // 특정 버튼의 ColorMultiplier를 설정하는 함수
    private void SetColorMultiplier(Button button, float multiplier)
    {
        ColorBlock colorBlock = button.colors;
        colorBlock.colorMultiplier = multiplier;
        button.colors = colorBlock;
    }

    // 합성 모드 종료 시 호출
    public void EndCombiningMode()
    {
        isCombiningModeActive = false;
        SetButtonsActive(true, false, false); // 버튼을 다시 활성화
        ResetButtonColors(); // 모든 버튼의 색상을 기본 색상으로 초기화
        ResetColorMultipliers(); // 모든 버튼의 ColorMultiplier를 초기화
        ResetButtonColor(); // 마지막 클릭된 버튼의 색상 복원
    }

    // 판매 모드 종료 시 호출
    public void EndSellingMode()
    {
        isSellingModeActive = false;
        SetButtonsActive(true, false, false); // 버튼을 다시 활성화
        ResetButtonColors(); // 모든 버튼의 색상을 기본 색상으로 초기화
        ResetColorMultipliers(); // 모든 버튼의 ColorMultiplier를 초기화
        ResetButtonColor(); // 마지막 클릭된 버튼의 색상 복원
    }
}