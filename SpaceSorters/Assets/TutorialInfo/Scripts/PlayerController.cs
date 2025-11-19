using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerController : MonoBehaviour
{
    [Header("1. 카메라 설정")]
    public GameObject mainCamera;      // 1인칭 카메라 (Main Camera)
    public GameObject topDownCamera;   // 3인칭 카메라 (TopDownCamera)
    public InputActionProperty switchViewAction; // 시점 전환 버튼 (B키)

    [Header("2. 손 설정 (기능)")]
    public XRDirectInteractor leftHandInteractor;  // 왼쪽 손 잡기 기능
    public XRDirectInteractor rightHandInteractor; // 오른쪽 손 잡기 기능

    [Header("3. 손 설정 (모델)")]
    public GameObject rightHandObject; // 오른손 전체 (시점 전환 시 꺼버릴 대상)

    [Header("4. 설정값")]
    public float switchCooldown = 3.0f; // 쿨타임 3초

    private bool isTopDown = false;     // 현재 시점 상태
    private float lastSwitchTime = -99f; // 마지막으로 버튼 누른 시간

    private void OnEnable()
    {
        // 버튼 입력 활성화 및 연결
        switchViewAction.action.Enable();
        switchViewAction.action.performed += OnSwitchView;

        // 잡기 시도할 때마다 검사하는 기능 연결
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

    // ■ 시점 전환 로직 (버튼 눌렀을 때)
    private void OnSwitchView(InputAction.CallbackContext context)
    {
        // 쿨타임 체크
        if (Time.time - lastSwitchTime < switchCooldown)
        {
            Debug.Log($"쿨타임! {switchCooldown - (Time.time - lastSwitchTime):F1}초 남음");
            return;
        }

        isTopDown = !isTopDown; // 상태 뒤집기 (1인칭 <-> 3인칭)

        if (isTopDown)
        {
            // 3인칭 모드: 탑뷰 켜기, 1인칭 끄기, **오른손 끄기**
            mainCamera.SetActive(false);
            topDownCamera.SetActive(true);
            if (rightHandObject != null) rightHandObject.SetActive(false);
            Debug.Log(">> 3인칭 전환 (오른손 봉인)");
        }
        else
        {
            // 1인칭 모드: 원래대로 복구
            mainCamera.SetActive(true);
            topDownCamera.SetActive(false);
            if (rightHandObject != null) rightHandObject.SetActive(true);
            Debug.Log(">> 1인칭 복귀");
        }

        lastSwitchTime = Time.time; // 시간 기록
    }

    // ■ 잡기 제한 로직 (왼손은 1단계만 잡게 하기)
    private void OnGrabAttempt(SelectEnterEventArgs args)
    {
        // 잡으려는 물건 가져오기
        var grabInteractable = args.interactableObject as XRGrabInteractable;
        if (grabInteractable == null) return;

        // 물건에 붙은 'Item' 스크립트 정보 가져오기
        Item itemData = grabInteractable.GetComponent<Item>();

        // 아이템 정보가 있고, 왼손으로 잡았는데, 1단계가 아니라면?
        if (itemData != null && args.interactorObject == leftHandInteractor && itemData.tier != ItemTier.Tier1)
        {
            // 강제로 놓게 만듦 (Interaction Manager에게 "취소해!"라고 명령)
            args.manager.SelectExit(args.interactorObject, grabInteractable);
            Debug.Log("🚫 왼손은 1단계 아이템만 잡을 수 있습니다!");
        }
    }
}