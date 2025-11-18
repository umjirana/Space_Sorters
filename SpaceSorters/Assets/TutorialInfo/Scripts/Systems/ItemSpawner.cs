using UnityEngine;
using System.Collections;
using System.Collections.Generic; // 리스트(List)를 쓰기 위해 필수!

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("생성할 아이템 프리팹 6종을 이곳에 등록하세요")]
    public List<GameObject> spawnList; // 아이템 목록 (리스트)

    [Header("Random Timing Settings")]
    [Tooltip("최소 대기 시간 (초)")]
    public float minDelay = 1.0f;
    [Tooltip("최대 대기 시간 (초)")]
    public float maxDelay = 6.0f; // 6초 안에 나오게 설정

    private void Start()
    {
        // 리스트에 아이템이 하나라도 있어야 실행 (에러 방지)
        if (spawnList.Count > 0)
        {
            StartCoroutine(SpawnRoutine());
        }
        else
        {
            Debug.LogError("스포너에 아이템 프리팹이 등록되지 않았습니다!");
        }
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // 1. 랜덤 아이템 뽑기 (0번부터 리스트 끝까지 중 하나)
            int randomIndex = Random.Range(0, spawnList.Count);
            GameObject randomItem = spawnList[randomIndex];

            // 2. 아이템 생성 (내 위치에서)
            Instantiate(randomItem, transform.position, Quaternion.identity);

            // 3. 랜덤 시간 계산 (최소 ~ 최대 사이)
            float randomWaitTime = Random.Range(minDelay, maxDelay);

            // 4. 기다리기
            yield return new WaitForSeconds(randomWaitTime);
        }
    }
}