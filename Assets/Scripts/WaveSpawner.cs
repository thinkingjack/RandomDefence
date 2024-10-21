using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public Transform[] enemyPrefabs; // �� ���庰�� ������ �� �����յ��� ������ �迭
    public Transform spawnPoint;  // ���� ������ ��ġ
    private float countdown = 1f;  // ���� ���̺���� ���� �ð�

    public Text waveCountdownText; // ���� �ð��� ǥ���� �ؽ�Ʈ UI ���
    public Text waveCountText; // ���� ���带 ǥ���� �ؽ�Ʈ UI ���

    private int waveIndex = 0; // ���� ���̺��� �ε���

    // �� ���̺꺰 ���� ���� �����ϴ� �迭 (��: 1���� 10����, 2���� 20���� ��)
    public int[] enemyCountPerWave;
    public float timeBetweenRounds; // ���� ������ ��� �ð�

    void Update()
    {
        if (countdown <= 0f)
        {
            if (waveIndex < enemyCountPerWave.Length)
            {
                StartCoroutine(SpawnWave()); // ���̺� ����
                countdown = timeBetweenRounds; // ���� ������ ��� �ð����� �ʱ�ȭ
            }
            else
            {
                // ��� ���̺갡 �Ϸ�� ����� ó�� (��: ���� �¸�)
                Debug.Log("All waves completed!");
            }
        }
        countdown -= Time.deltaTime; // ī��Ʈ�ٿ� ����

        waveCountdownText.text = string.Format("{0:00.00}", countdown); // ���� �ð� ǥ��
        waveCountText.text = waveIndex.ToString(); // ���� ���� ǥ��
    }

    IEnumerator SpawnWave()
    {
        int enemyCount = enemyCountPerWave[waveIndex]; // ���� ���̺��� �� ��
        waveIndex++; // ���̺� �ε��� ����

        Transform enemyPrefabToSpawn = enemyPrefabs[(waveIndex - 1) % enemyPrefabs.Length]; // ���� ���̺꿡 ������ �� ������ ����

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy(enemyPrefabToSpawn); // �� ����
            yield return new WaitForSeconds(0.5f); // 0.5�� ���
        }
    }

    void SpawnEnemy(Transform enemyPrefab)
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation); // ���õ� �� �ν��Ͻ�ȭ
    }
}