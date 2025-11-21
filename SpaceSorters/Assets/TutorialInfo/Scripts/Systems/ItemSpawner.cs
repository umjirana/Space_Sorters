using UnityEngine;
using System.Collections;
using System.Collections.Generic; // 리스트 사용을 위해 추가

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<GameObject> spawnList; // 생성할 아이템 목록 (6종 넣을 곳)

    [Header("Timing (GDD: 1.2s ~ 1.8s)")]
    public float minDelay = 1.2f;
    public float maxDelay = 1.8f;

    private void Start()
    {
        if (spawnList.Count > 0)
            StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // 1. 랜덤 아이템 선택
            int randomIndex = Random.Range(0, spawnList.Count);
            GameObject selectedItem = spawnList[randomIndex];

            // 2. 생성
            Instantiate(selectedItem, transform.position, Quaternion.identity);

            // 3. 랜덤 시간 대기
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);
        }
    }
}