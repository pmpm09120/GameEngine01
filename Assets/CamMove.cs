using UnityEngine;

public class CamMove : MonoBehaviour
{
    [Header("Cam")]
    public Transform Target;
    public float smooth = 0.5f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    /// <summary>
    /// target.position.x 움직임
    /// 카메라의 이동 x 방향 + or -
    /// 이동 방향 * xOffset
    /// 카메라 이동
    /// 
    /// x움직임이 없을시 카운트다운 시작
    /// 만약 카운트가 0이라면
    /// =>카메라 x위치 0
    /// </summary>
    /// 

    public float Xoffset;
    private float targetXPos;
    private float lastTargetX;

    private float currenttargetXPos;
    private float targetPosPos;
    public float smooth2;
    [SerializeField] private float timeScale = 2f;
    [SerializeField] private float currenttime;

    private void LateUpdate()
    {
        if (Target == null) return;
        float xMove = Target.position.x - lastTargetX;
        if (Mathf.Abs(xMove) > 0.001f)
        {
            targetXPos = Mathf.Sign(xMove) * Xoffset;
            currenttime = timeScale;
        }
        else if(xMove <= 0)
        {
            if (currenttime >= 0)
            {
                currenttime -= Time.deltaTime;
            }
            else
            {
                targetXPos = 0;
            }
        }
        lastTargetX = Target.position.x;
        currenttargetXPos = Mathf.SmoothDamp(currenttargetXPos, targetXPos, ref targetPosPos, smooth2);
        Vector3 targetpos = Target.position + offset;
        targetpos.x += currenttargetXPos; 
        transform.position = Vector3.SmoothDamp(transform.position, targetpos, ref velocity, smooth);
    }
}
