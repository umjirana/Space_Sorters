using UnityEngine;

public enum ItemType { Good, Bad }
public enum ItemTier { Tier1, Tier2, Tier3 }
public enum BadEffect { None, InstantFine, BeltOverload, VisionBlur } // 디버프 종류

public class Item : MonoBehaviour
{
    [Header("1. 기본 정보")]
    public ItemType type;         // Good / Bad
    public ItemTier tier;         // 크기 및 무게 등급

    [Header("2. Good 아이템 설정")]
    public int price = 100;       // 획득 점수 ($100, $300, $700)

    [Header("3. Bad 아이템 설정")]
    public BadEffect badEffect = BadEffect.None; // 패널티 종류

    [Header("4. 공통 물리 설정")]
    [Range(0.1f, 0.4f)]
    public float weightPenalty = 0.1f; // 이동속도 감소율 (10%, 25%, 40%)

    // 나중에 모델링이 바뀌어도 찾기 쉽게 이름 자동 변경
    private void OnValidate()
    {
        gameObject.name = $"Item_{type}_{tier}";
    }
}