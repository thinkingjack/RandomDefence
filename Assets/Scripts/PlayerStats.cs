using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    public int startMoney = 400;

    public static int Lives;
    public int startLives = 20;

    public Button gambleButton; // 버튼 참조
    public Text resultText;     // 결과 메시지를 표시할 Text 참조

    void Start()
    {
        Money = startMoney;
        Lives = startLives;
        resultText.text = ""; // 초기에는 결과 메시지를 비워둠
    }

    // 도박 메서드
    public void AttemptGamble()
    {
        if (Money >= 20)
        {
            Money -= 20;
            int gambleResult = Random.Range(0, 2);

            if (gambleResult == 1)
            {
                Money += 40;
                resultText.text = "승리하셨습니다! 20원을 획득했습니다."; // 승리 메시지
                resultText.color = new Color(0, 1, 0, 0.5f); // 초록색 (투명도 50%)
            }
            else
            {
                resultText.text = "패배하셨습니다... 20원을 잃었습니다."; // 패배 메시지
                resultText.color = new Color(1, 0, 0, 0.5f); // 빨간색 (투명도 50%)
            }

            // 버튼을 비활성화하고 3초 후 다시 활성화하는 코루틴 실행
            StartCoroutine(ButtonCooldown());
        }
        else
        {
            resultText.text = "돈이 부족합니다..";
            resultText.color = new Color(1, 1, 0, 0.5f); // 노란색 (투명도 50%)
        }
    }

    // 3초 후에 버튼을 다시 활성화하는 코루틴
    IEnumerator ButtonCooldown()
    {
        gambleButton.interactable = false; // 버튼 비활성화
        yield return new WaitForSeconds(3); // 3초 대기
        gambleButton.interactable = true;  // 버튼 다시 활성화
        resultText.text = ""; // 메시지 초기화
    }
}