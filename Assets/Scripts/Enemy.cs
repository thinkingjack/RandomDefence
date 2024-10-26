//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class Enemy : MonoBehaviour
//{
//    public float startSpeed = 10f;

//    [HideInInspector]
//    public float speed;

//    public float startHealth = 100;
//    private float health;

//    public int value = 50;

//    [Header("Unity Stuff")]
//    public Image healthBar;

//    // ī�޶� ���� ����
//    private Camera mainCamera;

//    void Start()
//    {
//        speed = startSpeed;
//        health = startHealth;

//        // ī�޶� ���� �ʱ�ȭ
//        mainCamera = Camera.main;
//    }

//    void Update()
//    {
//        // ������(Billboarding) - ü�¹ٸ� ī�޶� �������� ȸ����Ŵ (Z�� ȸ�� ����)
//        Vector3 direction = mainCamera.transform.position - healthBar.transform.position;
//        direction.y = direction.z = 0; 
//        healthBar.transform.parent.LookAt(mainCamera.transform.position - direction);
//    }

//    public void TakeDamage(float amount)
//    {
//        health -= amount;

//        healthBar.fillAmount = health / startHealth;

//        if (health <= 0f)
//        {
//            Die();
//        }
//    }

//    public void Slow(float pct)
//    {
//        speed = startSpeed * (1f - pct);
//    }

//    void Die()
//    {
//        PlayerStats.Money += value;
//        Destroy(gameObject);
//    }
//}
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 10f;
    [HideInInspector]
    public float speed;

    public float startHealth = 100;
    private float health;

    public int value = 50;

    [Header("Unity Stuff")]
    public Image healthBar;

    private Camera mainCamera;

    // ���� ���� ����
    private bool isStunned = false;
    private float stunDuration = 0f;

    void Start()
    {
        speed = startSpeed;
        health = startHealth;
        mainCamera = Camera.main;
    }

    void Update()
    {
        // ������ - ü�¹ٰ� ī�޶� ���ϵ��� ȸ��
        Vector3 direction = mainCamera.transform.position - healthBar.transform.position;
        direction.y = direction.z = 0;
        healthBar.transform.parent.LookAt(mainCamera.transform.position - direction);

        // ���� ������ �� ���� ���� �ð��� �ٿ�����
        if (isStunned)
        {
            stunDuration -= Time.deltaTime;
            if (stunDuration <= 0f)
            {
                isStunned = false;
                speed = startSpeed; // ������ ������ ���� �ӵ��� ����
            }
            else
            {
                speed = 0f; // ���� ������ �� �̵� ����
            }
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startHealth;

        if (health <= 0f)
        {
            Die();
        }
    }

    public void ApplyStun(float duration)
    {
        isStunned = true;
        stunDuration = duration;
        speed = 0f; // ���� ������ �� ���� �̵��� ����
    }

    public void Slow(float pct)
    {
        // ���� ���°� �ƴ� ���� ���ο� ȿ�� ����
        if (!isStunned)
        {
            speed = startSpeed * (1f - pct);
        }
    }

    void Die()
    {
        PlayerStats.Money += value;
        Destroy(gameObject);
    }
}