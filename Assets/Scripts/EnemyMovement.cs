using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private int wavepointIndex = 0;

    private Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();

        target = WayPoints.points[0];
    }

    void Update()
    {
        Move();
        enemy.speed = enemy.startSpeed;
    }
    void Move()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

        // �̵� �������� ȸ��
        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * enemy.speed);
        }
        // ��������Ʈ�� �����ϸ� ���� ��������Ʈ�� �̵�
        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
    }
    void GetNextWaypoint()
    {
        if (wavepointIndex >= WayPoints.points.Length - 1)
        {
            EndPath();// ������ ��������Ʈ�� �����ϸ� ���� �ı�
            return;
        }
        wavepointIndex++;
        target = WayPoints.points[wavepointIndex];
    }
    void EndPath()
    {
        PlayerStats.Lives--;
        Destroy(gameObject);
    }
}
