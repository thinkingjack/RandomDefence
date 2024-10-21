using System.Collections;
using System.Collections.Generic;
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

    // 카메라 참조 변수
    private Camera mainCamera;

    void Start()
    {
        speed = startSpeed;
        health = startHealth;

        // 카메라 참조 초기화
        mainCamera = Camera.main;
    }

    void Update()
    {
        // 빌보딩(Billboarding) - 체력바를 카메라 방향으로 회전시킴 (Z축 회전 고정)
        Vector3 direction = mainCamera.transform.position - healthBar.transform.position;
        direction.y = direction.z = 0; 
        healthBar.transform.parent.LookAt(mainCamera.transform.position - direction);
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

    public void Slow(float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    void Die()
    {
        PlayerStats.Money += value;
        Destroy(gameObject);
    }
}