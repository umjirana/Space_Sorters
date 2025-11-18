using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject itemPrefab; // 생성할 아이템 프리팹 (테스트용 큐브)
    public float spawnInterval = 2.0f; // 생성 간격 (초)

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // 1. 아이템 생성 (내 위치에서)
            Instantiate(itemPrefab, transform.position, Quaternion.identity);

            // 2. 기다리기
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}