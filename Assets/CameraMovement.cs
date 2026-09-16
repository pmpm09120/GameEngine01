using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;

    [Header("Camera Setting")]
    public float smoothTime = 0.25f;
    public Vector3 offset;

    [Header("Look-Ahead Setting")]
    public float lookAheadDstX = 2.5f;       // 앞을 미리 보는 거리
    public float lookAheadSmoothTime = 0.4f; // 앞을 볼 때 부드러움 정도

    private Vector3 velocity = Vector3.zero;

    // 룩 에헤드 계산용 변수들
    private float currentLookAheadX;
    private float targetLookAheadX;
    private float lookAheadVelocity;
    private float lastTargetX;

    private void LateUpdate()
    {
        if (target == null) return;

        // 1. 플레이어의 X축 이동 방향 추적
        float moveDeltaX = target.position.x - lastTargetX;
        if (Mathf.Abs(moveDeltaX) > 0.001f)
        {
            targetLookAheadX = Mathf.Sign(moveDeltaX) * lookAheadDstX;
        }
        lastTargetX = target.position.x;

        // 2. 룩 에헤드 값을 부드럽게 전환
        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref lookAheadVelocity, lookAheadSmoothTime);

        // 3. 최종 목표 위치 계산 (기본 Offset + 룩 에헤드 적용)
        Vector3 targetPos = target.position + offset;
        targetPos.x += currentLookAheadX;

        // 4. 카메라 이동
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}