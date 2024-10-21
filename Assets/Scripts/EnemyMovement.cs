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

        // 이동 방향으로 회전
        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * enemy.speed);
        }
        // 웨이포인트에 도달하면 다음 웨이포인트로 이동
        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
    }
    void GetNextWaypoint()
    {
        if (wavepointIndex >= WayPoints.points.Length - 1)
        {
            EndPath();// 마지막 웨이포인트에 도달하면 적을 파괴
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
