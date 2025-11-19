using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("벨트 이동 속도 (m/s)")]
    [SerializeField] private float _speed = 1.5f;

    [Tooltip("벨트가 움직이는 방향 (기본: 앞쪽)")]
    [SerializeField] private Vector3 _direction = Vector3.back;
    // 유니티 좌표계상 보통 뒤(Back)쪽이 플레이어 쪽으로 오는 방향입니다.

    private Rigidbody _rb;
    private Renderer _renderer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        // 1. 물리 이동 (위에 있는 물체 옮기기)
        // 리지드바디의 위치를 강제로 이동시켜서, 위에 있는 물체도 같이 끌려가게 만듭니다.
        Vector3 pos = _rb.position;
        _rb.position += _direction * _speed * Time.fixedDeltaTime;
        _rb.MovePosition(pos);
    }

    private void Update()
    {
        // 2. 시각적 이동 (텍스처 스크롤)
        // 눈에 보이는 무늬만 계속 흘러가게 해서 움직이는 것처럼 보이게 합니다.
        float textureOffset = Time.time * _speed * 0.1f; // 0.1은 스크롤 속도 보정값
        _renderer.material.mainTextureOffset = new Vector2(0, textureOffset);
    }
}