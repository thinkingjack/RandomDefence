using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public Transform[] enemyPrefabs; // 각 라운드별로 스폰할 적 프리팹들을 설정할 배열
    public Transform spawnPoint;  // 적이 스폰될 위치
    private float countdown = 1f;  // 다음 웨이브까지 남은 시간

    public Text waveCountdownText; // 남은 시간을 표시할 텍스트 UI 요소
    public Text waveCountText; // 현재 라운드를 표시할 텍스트 UI 요소

    private int waveIndex = 0; // 현재 웨이브의 인덱스

    // 각 웨이브별 적의 수를 저장하는 배열 (예: 1라운드 10마리, 2라운드 20마리 등)
    public int[] enemyCountPerWave;
    public float timeBetweenRounds; // 라운드 사이의 대기 시간

    void Update()
    {
        if (countdown <= 0f)
        {
            if (waveIndex < enemyCountPerWave.Length)
            {
                StartCoroutine(SpawnWave()); // 웨이브 시작
                countdown = timeBetweenRounds; // 라운드 사이의 대기 시간으로 초기화
            }
            else
            {
                // 모든 웨이브가 완료된 경우의 처리 (예: 게임 승리)
                Debug.Log("All waves completed!");
            }
        }
        countdown -= Time.deltaTime; // 카운트다운 감소

        waveCountdownText.text = string.Format("{0:00.00}", countdown); // 남은 시간 표시
        waveCountText.text = waveIndex.ToString(); // 현재 라운드 표시
    }

    IEnumerator SpawnWave()
    {
        int enemyCount = enemyCountPerWave[waveIndex]; // 현재 웨이브의 적 수
        waveIndex++; // 웨이브 인덱스 증가

        Transform enemyPrefabToSpawn = enemyPrefabs[(waveIndex - 1) % enemyPrefabs.Length]; // 현재 웨이브에 스폰할 적 프리팹 선택

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy(enemyPrefabToSpawn); // 적 스폰
            yield return new WaitForSeconds(0.5f); // 0.5초 대기
        }
    }

    void SpawnEnemy(Transform enemyPrefab)
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation); // 선택된 적 인스턴스화
    }
}