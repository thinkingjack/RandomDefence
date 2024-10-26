//using UnityEngine;
//using System.Collections;

//public class Turret : MonoBehaviour
//{
//    private Transform target;
//    private Enemy targetEnemy;
//    private Animator animator;

//    [Header("General")]
//    public float range = 15f;
//    public int damage = 50; // 공격력 속성 추가
//    public bool isMelee = false; // 근접 공격 여부

//    [Header("Use Bullets (default)")]
//    public GameObject bulletPrefab;
//    public float fireRate = 1f;
//    private float fireCountdown = 0f;

//    [Header("Use Laser")]
//    public bool useLaser = false;
//    public int damageOverTime = 30;
//    public float slowValue = .5f;
//    public float shootDelay = 0.2f; // 총알 발사 딜레이

//    public LineRenderer lineRenderer;
//    public ParticleSystem impactEffect;

//    [Header("Unity Setup Fields")]
//    public string enemyTag = "Enemy";
//    public Transform partToRotate;
//    public float turnSpeed = 10f;
//    public Transform firePoint;

//    public TurretBluePrint blueprint;

//    public int InstallCost // 두 개의 기법을 다 써봄 연습
//    {
//        get => blueprint.cost;
//    }
//    public int SellPrice { get { return blueprint.sellPrice; } } // 두 개의 기법을 다 써봄 연습

//    void Start()
//    {
//        InvokeRepeating("UpdateTarget", 0f, 0.5f);
//        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 가져오기
//    }

//    void UpdateTarget()
//    {
//        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
//        float shortestDistance = Mathf.Infinity;
//        GameObject nearestEnemy = null;
//        foreach (GameObject enemy in enemies)
//        {
//            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
//            if (distanceToEnemy < shortestDistance)
//            {
//                shortestDistance = distanceToEnemy;
//                nearestEnemy = enemy;
//            }
//        }
//        if (nearestEnemy != null && shortestDistance <= range)
//        {
//            target = nearestEnemy.transform;
//            targetEnemy = nearestEnemy.GetComponent<Enemy>();
//        }
//        else
//        {
//            target = null;
//        }
//    }

//    void Update()
//    {
//        if (target == null)
//        {
//            if (useLaser && lineRenderer.enabled)
//            {
//                lineRenderer.enabled = false;
//                impactEffect.Stop();
//            }
//            return;
//        }

//        LookOnTarget();

//        // 근접 공격인지 원거리 공격인지 구분
//        if (isMelee)
//        {
//            if (Vector3.Distance(transform.position, target.position) <= range && fireCountdown <= 0f)
//            {
//                MeleeAttack();
//                fireCountdown = 1f / fireRate;
//            }
//        }
//        else
//        {
//            if (useLaser)
//            {
//                Laser();
//            }
//            else
//            {
//                if (fireCountdown <= 0f)
//                {
//                    StartCoroutine(ShootAfterDelay(shootDelay));
//                    fireCountdown = 1f / fireRate;
//                }
//            }
//        }

//        fireCountdown -= Time.deltaTime;
//    }

//    private IEnumerator ShootAfterDelay(float delay)
//    {
//        animator.SetTrigger("Shoot");
//        yield return new WaitForSeconds(delay);
//        Shoot();
//    }

//    private void Shoot()
//    {
//        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
//        Bullet bullet = bulletGO.GetComponent<Bullet>();

//        if (bullet != null)
//        {
//            bullet.Seek(target, damage); // 공격력을 총알에 전달
//        }
//    }

//    private void Laser()
//    {
//        if (target == null)
//            return;

//        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
//        targetEnemy.Slow(slowValue);

//        if (!lineRenderer.enabled)
//        {
//            lineRenderer.enabled = true;
//            impactEffect.Play();
//        }

//        lineRenderer.SetPosition(0, firePoint.position);
//        lineRenderer.SetPosition(1, target.position);
//        impactEffect.transform.position = target.position;
//        impactEffect.transform.right = firePoint.position - target.position;
//    }

//    private void MeleeAttack()
//    {
//        animator.SetTrigger("MeleeAttack"); // 근접 공격 애니메이션 실행
//        if (targetEnemy != null)
//        {
//            targetEnemy.TakeDamage(damage); // 근접 공격 데미지 적용
//        }
//    }

//    private void LookOnTarget()
//    {
//        Vector3 dir = target.position - transform.position;
//        Quaternion lookRotation = Quaternion.LookRotation(dir);
//        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
//        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f); // Y축만 회전
//    }
//}
using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    private Enemy targetEnemy;
    private Animator animator;

    [Header("General")]
    public float range = 15f;
    public int damage = 50;
    public bool isMelee = false;
    public bool isStunAttack = false; // 스턴 적용 여부 선택 변수
    // 스턴 지속 시간
    public float stunDuration = 1.5f; // 스턴 효과 지속 시간 설정

    [Header("Use Bullets (default)")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Use Laser")]
    public bool useLaser = false;
    public int damageOverTime = 30;
    public float slowValue = .5f;
    public float shootDelay = 0.2f;

    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public Transform firePoint;

    public TurretBluePrint blueprint;


    public int InstallCost // 두 개의 기법을 다 써봄 연습
    {
        get => blueprint.cost;
    }
    public int SellPrice { get { return blueprint.sellPrice; } } // 두 개의 기법을 다 써봄 연습

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        animator = GetComponent<Animator>();
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        if (target == null)
        {
            if (useLaser && lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                impactEffect.Stop();
            }
            return;
        }

        LookOnTarget();

        if (isMelee)
        {
            if (Vector3.Distance(transform.position, target.position) <= range && fireCountdown <= 0f)
            {
                MeleeAttack();
                fireCountdown = 1f / fireRate;
            }
        }
        else
        {
            if (useLaser)
            {
                Laser();
            }
            else
            {
                if (fireCountdown <= 0f)
                {
                    StartCoroutine(ShootAfterDelay(shootDelay));
                    fireCountdown = 1f / fireRate;
                }
            }
        }

        fireCountdown -= Time.deltaTime;
    }

    private IEnumerator ShootAfterDelay(float delay)
    {
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(delay);
        Shoot();
    }

    private void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target, damage);
        }
    }

    private void Laser()
    {
        if (target == null) return;

        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(slowValue);

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
        impactEffect.transform.position = target.position;
        impactEffect.transform.right = firePoint.position - target.position;
    }

    private void MeleeAttack()
    {
        animator.SetTrigger("MeleeAttack");
        if (targetEnemy != null)
        {
            targetEnemy.TakeDamage(damage);
            // 스턴 공격인 경우에만 스턴 적용
            if (isStunAttack)
            {
                targetEnemy.ApplyStun(stunDuration);
            }
        }
    }

    private void LookOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
}