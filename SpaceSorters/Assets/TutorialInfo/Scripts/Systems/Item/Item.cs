using UnityEngine;

public enum ItemType { Good, Bad }
public enum ItemTier { Tier1, Tier2, Tier3 }

public class Item : MonoBehaviour
{
    [Header("Item Info")]
    public ItemType type; // Good or Bad
    public ItemTier tier; // 1, 2, 3 단계

    [Header("Settings")]
    public int scoreValue = 100; // 점수 (Good일 때)
    public float weight = 0.1f;  // 무게 (이동속도 저하용)
}