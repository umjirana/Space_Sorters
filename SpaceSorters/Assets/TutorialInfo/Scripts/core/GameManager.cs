using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 어디서든 부를 수 있게 (싱글톤)

    [Header("Game Status")]
    public int badItemsMissed = 0;
    public int maxBadItemsAllowed = 5;
    public bool isGameOver = false;

    private void Awake()
    {
        Instance = this;
    }

    // 아이템이 끝 기계로 넘어갔을 때 호출
    public void OnItemMissed(Item item)
    {
        if (isGameOver) return;

        if (item.type == ItemType.Bad)
        {
            badItemsMissed++;
            Debug.Log($"경고! Bad 아이템 놓침! ({badItemsMissed}/{maxBadItemsAllowed})");

            if (badItemsMissed >= maxBadItemsAllowed)
            {
                GameOver();
            }
        }
        else
        {
            Debug.Log("Good 아이템이 그냥 지나갔습니다. (점수 획득 실패)");
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.LogError("!!! GAME OVER !!! Bad 아이템 5개 누적!");
        // 나중에 여기에 게임 오버 UI 띄우는 코드 추가
    }
}