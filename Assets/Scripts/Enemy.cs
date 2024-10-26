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

//    // 카메라 참조 변수
//    private Camera mainCamera;

//    void Start()
//    {
//        speed = startSpeed;
//        health = startHealth;

//        // 카메라 참조 초기화
//        mainCamera = Camera.main;
//    }

//    void Update()
//    {
//        // 빌보딩(Billboarding) - 체력바를 카메라 방향으로 회전시킴 (Z축 회전 고정)
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

    // 스턴 관련 변수
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
        // 빌보딩 - 체력바가 카메라를 향하도록 회전
        Vector3 direction = mainCamera.transform.position - healthBar.transform.position;
        direction.y = direction.z = 0;
        healthBar.transform.parent.LookAt(mainCamera.transform.position - direction);

        // 스턴 상태일 때 스턴 지속 시간을 줄여나감
        if (isStunned)
        {
            stunDuration -= Time.deltaTime;
            if (stunDuration <= 0f)
            {
                isStunned = false;
                speed = startSpeed; // 스턴이 끝나면 원래 속도로 복귀
            }
            else
            {
                speed = 0f; // 스턴 상태일 때 이동 멈춤
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
        speed = 0f; // 스턴 상태일 때 적의 이동을 멈춤
    }

    public void Slow(float pct)
    {
        // 스턴 상태가 아닐 때만 슬로우 효과 적용
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