using UnityEngine;

public class VRBodyAnimator : MonoBehaviour
{
    [Header("연결할 대상")]
    public Animator animator;
    public Transform playerHead;   // 메인 카메라

    [Header("설정")]
    public float moveThreshold = 0.1f;

    private Vector3 lastPosition;

    void Start()
    {
        if (playerHead != null) lastPosition = playerHead.position;
    }

    void Update()
    {
        if (animator == null || playerHead == null) return;

        // [★ 핵심] 몸통 위치를 강제로 카메라(눈) 위치로 이동 (높이는 유지)
        transform.position = new Vector3(playerHead.position.x, transform.position.y, playerHead.position.z);

        // [★ 핵심] 몸통 회전도 카메라가 보는 방향(Y축)으로 돌림
        Vector3 lookDir = playerHead.forward;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        // 걷기 애니메이션 계산
        Vector3 currentPos = playerHead.position;
        float speed = Vector3.Distance(new Vector3(currentPos.x, 0, currentPos.z),
                                       new Vector3(lastPosition.x, 0, lastPosition.z)) / Time.deltaTime;

        if (speed > moveThreshold) animator.SetInteger("AnimationPar", 1);
        else animator.SetInteger("AnimationPar", 0);

        lastPosition = currentPos;
    }
}