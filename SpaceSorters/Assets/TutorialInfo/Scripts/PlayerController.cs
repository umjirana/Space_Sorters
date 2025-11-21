using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerController : MonoBehaviour
{
    [Header("1. 카메라 설정")]
    public Camera mainCamComponent;    // Main Camera의 Camera 컴포넌트
    public Camera topDownCamComponent; // TopDownCamera의 Camera 컴포넌트
    public InputActionProperty switchViewAction; // 시점 전환 버튼 (B키)

    [Header("2. 손 설정")]
    public XRDirectInteractor leftHandInteractor;
    public XRDirectInteractor rightHandInteractor;
    public GameObject rightHandObject; // 오른손 전체 (끄기용)

    [Header("3. 설정값")]
    public float switchCooldown = 3.0f;
    private float lastSwitchTime = -99f;
    private bool isTopDown = false; // 현재 시점 상태 (false=1인칭, true=3인칭)

    private void Start()
    {
        // 카메라 초기화: 메인 카메라는 항상 켜두되, 우선순위(depth)로 조절
        if (mainCamComponent != null) mainCamComponent.depth = 0;
        if (topDownCamComponent != null)
        {
            topDownCamComponent.depth = 10; // 켜지면 무조건 위에 덮어씌움
            topDownCamComponent.gameObject.SetActive(false); // 일단 꺼둠
        }
    }

    private void OnEnable()
    {
        switchViewAction.action.Enable();
        switchViewAction.action.performed += OnSwitchView;

        // 잡기 시도할 때마다 검사 (이벤트 연결)
        leftHandInteractor.selectEntered.AddListener(OnGrabAttempt);
        rightHandInteractor.selectEntered.AddListener(OnGrabAttempt);
    }

    private void OnDisable()
    {
        switchViewAction.action.Disable();
        switchViewAction.action.performed -= OnSwitchView;
        leftHandInteractor.selectEntered.RemoveListener(OnGrabAttempt);
        rightHandInteractor.selectEntered.RemoveListener(OnGrabAttempt);
    }

    // ■ 시점 전환 로직
    private void OnSwitchView(InputAction.CallbackContext context)
    {
        if (Time.time - lastSwitchTime < switchCooldown) return;

        isTopDown = !isTopDown;
        lastSwitchTime = Time.time;

        if (isTopDown) // 3인칭 전환
        {
            // 메인 카메라는 끄지 않음 (그래야 이동 키가 먹힘)
            topDownCamComponent.gameObject.SetActive(true); // 탑뷰 켜서 덮기
            if (rightHandObject != null) rightHandObject.SetActive(false); // 오른손 봉인
            Debug.Log(">> 3인칭 전환 (왼손 1티어만 가능)");
        }
        else // 1인칭 복귀
        {
            topDownCamComponent.gameObject.SetActive(false); // 탑뷰 끄기
            if (rightHandObject != null) rightHandObject.SetActive(true); // 오른손 복구
            Debug.Log(">> 1인칭 복귀 (자유)");
        }
    }

    // ■ 잡기 제한 로직 (수정됨!)
    private void OnGrabAttempt(SelectEnterEventArgs args)
    {
        // 1인칭일 때는 제한 없음! (바로 리턴)
        if (!isTopDown) return;

        var grabInteractable = args.interactableObject as XRGrabInteractable;
        if (grabInteractable == null) return;

        Item itemData = grabInteractable.GetComponent<Item>();

        // [조건] 3인칭이고 + 아이템 데이터가 있고 + 1단계가 아니라면? -> 뱉어냄
        if (itemData != null && itemData.tier != ItemTier.Tier1)
        {
            // 강제로 놓기
            args.manager.SelectExit(args.interactorObject, grabInteractable);
            Debug.Log("🚫 3인칭에서는 1단계 아이템만 잡을 수 있습니다!");
        }
    }
}