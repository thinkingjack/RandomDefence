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

    // ī�޶� ���� ����
    private Camera mainCamera;

    void Start()
    {
        speed = startSpeed;
        health = startHealth;

        // ī�޶� ���� �ʱ�ȭ
        mainCamera = Camera.main;
    }

    void Update()
    {
        // ������(Billboarding) - ü�¹ٸ� ī�޶� �������� ȸ����Ŵ (Z�� ȸ�� ����)
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