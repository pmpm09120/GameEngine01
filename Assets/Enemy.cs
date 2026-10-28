using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.AI.Navigation.LowLevel;

public enum Pattern { Forward, Circle, Left, Right, Diagonal, Stop }
public enum State { Spawn, Move, Attack, MovingAttack, Stop }
public class Enemy : MonoBehaviour
{
    /*
     * wave system
timelap system

waveData => 1 wave sapwnDatas, WaveTime
spawnData => spawnPosRot, amount, Perfab, delay, spawnStartTime, spawnPattern
spawn system => waveData ,Queue->spawnDatas, spawn 


enemy<
movePatterns
Circle, Forward, Stop, / or \, left or right


     */

    /*
     * 1942---
     * 1
     * Spawn System -> 타임라인 및 웨이브 기반 생성
     * 생성 데이터 구조] 예) 등장 시간, 적ID, 생성 위치(x,y), 시작 방향---
     * 스테이지 진행표(웨이브 시나리오) 데이터 => 생성 시점, 생성 위치, 생성 객체, 지정 경로
     * 
     * 2
     * 1) 움직임 패턴 시스템
     * 적의 움직임의 다양한 패턴 -> 직선, 곡선, Z, 편대 등 수학적 함수와 상태 기계로 구현
     * 
     * 2) 웨이포인트 시스템
     * 적에게 이동 경로점 리스트 부여
     * 웨이포인트 위치 데이터(경로 패턴 데이터) 및 경로 ID 리스트
     * 구조: PathData(PathID, PathParent, WayPoints_Transform_List) > Path(Empty GameObject) > WayPoints // 1 PathData = 1 Path Pattern
     *WaveData > PathData List
     *
     * 3
     * 상태 머신
     * 적의 행동 예) 등장, 지정 위치로 이동, 제자리 정지 후 사격, 화면 밖으로 퇴장
     * 
     * 4
     * 편대 밒 리더 팔로워 시스템
     * 리더 적 한 마리만 복잡한 궤적을 따라 움직이고, 팔로워 적들은 리더의 위치를 기준으로 오프셋 거리만큼 떨어진 채 따라오게 함
     */
    public int MaxHealth = 30;
    [SerializeField] private int health;
    public PerfabID_Data enemyData;

    [Header("Movements")]
    public Rigidbody2D rb;
    //public Pattern pattern;
    public float movespeed = 5;

    public int pathID;
    public List<Transform> waypoints = new List<Transform>();
    public int currentIndex = 0;

    [Header("Enemy")]
    public State statePattern;
    public SpriteRenderer sprite;


    private void Start()
    {
        health = MaxHealth;

        //MovePattern(pattern, 3f);
    }

    public int _health
    {
        get { return health; }
        set
        {
            health = Mathf.Clamp(value, 0, MaxHealth);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MapCollison"))
        {
            Die();
        }
    }
    public virtual void TakeDamage(int dam)
    {
        _health -= dam;
        StartCoroutine(HitColor());
        if (health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Debug.Log($"[TK]{gameObject.name} Died");
        ObjectPoolManager.instance.ReturnPool(enemyData.id, gameObject);
    }

    IEnumerator HitColor()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sprite.color = Color.white;
    }

    bool isMove = false;
    public void StatePattern()
    {
        switch (statePattern)
        {
            case State.Move:
                isMove = true;
                break;
        }
    }
    private void Move()
    {
        if (waypoints == null || waypoints.Count == 0) return;
        if (currentIndex > waypoints.Count) return;

        Vector2 target = waypoints[currentIndex].transform.position;
        Vector2 dir = target - (Vector2)rb.position;
        float distance = dir.magnitude;

        if (distance <= 0.01f)
        {
            currentIndex++;
            if (currentIndex >= waypoints.Count)
            {
                isMove = false;
                currentIndex = 0;
                Debug.Log($"[Enemy] name: {gameObject.name} / Log: Waypoint Progress Ended");
            }
        }
        else
        {
            rb.MovePosition(rb.position + dir.normalized * movespeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (isMove)
        {
            Move();
        }
    }

    /*
    public void MovePattern(Pattern pattern, float moveTime = 0)
    {
        switch (pattern)
        {
            case Pattern.Forward:
                MoveForward();
                break;
            case Pattern.Stop:
                Stop(moveTime);
                break;
            default:
                Debug.LogWarning("[Enemy] MovePattern Error");
                break;
        }
    }

    private void MoveForward()
    {
        StartCoroutine(MoveForwardCor());
    }

    private void Stop(float movetime)
    {
        StartCoroutine(StopCor(movetime));
    }

    IEnumerator StopCor(float duration)
    {
        float currentTime = 0f;
        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            rb.MovePosition(rb.position + Vector2.down * movespeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator MoveForwardCor()
    {
        rb.MovePosition(rb.position + Vector2.down * movespeed * Time.deltaTime);
        yield return null;
    }
    */
}
