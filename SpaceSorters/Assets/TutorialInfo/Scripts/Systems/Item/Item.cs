using UnityEngine;

// 아이템의 종류와 등급을 정의하는 목록표
public enum ItemType { Good, Bad }
public enum ItemTier { Tier1, Tier2, Tier3 }
public enum BadEffect { None, InstantFine, BeltOverload, VisionBlur } // 나중에 쓸 디버프 종류

public class Item : MonoBehaviour
{
    [Header("1. 기본 정보")]
    public ItemType type;         // Good(정상) 또는 Bad(불량)
    public ItemTier tier;         // 1, 2, 3 단계 (크기/무게)

    [Header("2. Good 아이템 설정")]
    public int price = 100;       // 획득 시 점수

    [Header("3. Bad 아이템 설정")]
    public BadEffect badEffect = BadEffect.None; // 불량품 패널티 종류

    [Header("4. 공통 물리 설정")]
    [Range(0.1f, 0.4f)]
    public float weightPenalty = 0.1f; // 잡았을 때 이동속도 감소율 (나중에 구현)

    // 에디터에서 값을 바꾸면 오브젝트 이름도 보기 좋게 자동 변경
    private void OnValidate()
    {
        gameObject.name = $"Item_{type}_{tier}";
    }
}
