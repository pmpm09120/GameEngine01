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

    //time
    public float cooldown = 2f;
    public float currentcooldown = 0;

    private void LateUpdate()
    {
        if (target == null) return;

        // 1. 플레이어의 X축 이동 방향 추적
        float moveDeltaX = target.position.x - lastTargetX;
        if (Mathf.Abs(moveDeltaX) > 0.001f) //움직임 검사 Abs => 절댓값
        {
            targetLookAheadX = Mathf.Sign(moveDeltaX) * lookAheadDstX; //Mathf.Sign() => 부호에 따라 1 or -1 반환  / 이동방향 * 카메라 x축 이동 오프셋 => 카메라 도착 위치
            currentcooldown = cooldown;
        }
        else
        {
            if (currentcooldown > 0)
            {
                currentcooldown -= Time.deltaTime;
            }
            else if (currentcooldown <= 0)
            {
                targetLookAheadX = 0f;
            }
        }
        lastTargetX = target.position.x; //지난 위치 업데이트

        // 2. 룩 에헤드 값을 부드럽게 전환
        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref lookAheadVelocity, lookAheadSmoothTime);
        //현재 위치, 목표 위치, 속도 현재값, 도착하는데 걸리는 시간 => 보정값, 이동할 x위치를 부드럽게함

        // 3. 최종 목표 위치 계산 (기본 Offset + 룩 에헤드 적용)
        Vector3 targetPos = target.position + offset; //카메라 위치
        targetPos.x += currentLookAheadX; //카메라 위치 x에 보정값 추가

        // 4. 카메라 이동
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime); //카메라의 이동을 부드럽게 함


    }
}