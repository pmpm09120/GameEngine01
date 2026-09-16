using System;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;

    [Header("Camera Setting")]
    public float smoothTime = 0.25f; //값이 작을 수록 속도가 빠르고, 클 수록 느림
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position + offset;
        //SmoothDamp() / 목표값 까지 부드럽게 이동시키는 메서드
        Vector3 smoothDamp = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        transform.position = smoothDamp;
    }
}
