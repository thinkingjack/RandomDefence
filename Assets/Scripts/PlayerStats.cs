using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    public int startMoney = 400;

    public static int Lives;
    public int startLives = 20;

    public Button gambleButton; // ��ư ����
    public Text resultText;     // ��� �޽����� ǥ���� Text ����

    void Start()
    {
        Money = startMoney;
        Lives = startLives;
        resultText.text = ""; // �ʱ⿡�� ��� �޽����� �����
    }

    // ���� �޼���
    public void AttemptGamble()
    {
        if (Money >= 20)
        {
            Money -= 20;
            int gambleResult = Random.Range(0, 2);

            if (gambleResult == 1)
            {
                Money += 40;
                resultText.text = "�¸��ϼ̽��ϴ�! 20���� ȹ���߽��ϴ�."; // �¸� �޽���
                resultText.color = new Color(0, 1, 0, 0.5f); // �ʷϻ� (���� 50%)
            }
            else
            {
                resultText.text = "�й��ϼ̽��ϴ�... 20���� �Ҿ����ϴ�."; // �й� �޽���
                resultText.color = new Color(1, 0, 0, 0.5f); // ������ (���� 50%)
            }

            // ��ư�� ��Ȱ��ȭ�ϰ� 3�� �� �ٽ� Ȱ��ȭ�ϴ� �ڷ�ƾ ����
            StartCoroutine(ButtonCooldown());
        }
        else
        {
            resultText.text = "���� �����մϴ�..";
            resultText.color = new Color(1, 1, 0, 0.5f); // ����� (���� 50%)
        }
    }

    // 3�� �Ŀ� ��ư�� �ٽ� Ȱ��ȭ�ϴ� �ڷ�ƾ
    IEnumerator ButtonCooldown()
    {
        gambleButton.interactable = false; // ��ư ��Ȱ��ȭ
        yield return new WaitForSeconds(3); // 3�� ���
        gambleButton.interactable = true;  // ��ư �ٽ� Ȱ��ȭ
        resultText.text = ""; // �޽��� �ʱ�ȭ
    }
}