using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 부딪힌 게 아이템인지 확인
        Item item = other.GetComponent<Item>();

        if (item != null)
        {
            // 게임 매니저에게 보고
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnItemMissed(item);
            }

            // 아이템 삭제 (메모리 정리)
            Destroy(other.gameObject);
        }
    }
}