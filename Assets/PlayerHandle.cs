using UnityEngine;

public class PlayerHandle : MonoBehaviour
{
    public GameObject handel;
    ///<summary>
    ///Mouse 벡터 좌표
    ///MousePos - Target.position
    /// dy,dx는 마우스와 객체간의 방향 벡터의 수직과 수평값 
    /// float z = Mathf.Atan2(dy,dx).Rad2f;
    ///handel의 euler z축 회전값을 Quaternion.Euler()를 통해 변형
}
