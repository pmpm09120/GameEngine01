using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum Pattern { Forward, Circle, Left, Right, Diagonal, Stop }
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
    public int MaxHealth = 30;
    [SerializeField] private int health;
    public PerfabID_Data enemyData;

    [Header("Movements")]
    public Rigidbody2D rb;
    public Pattern pattern;
    public float movespeed = 5;


    private void Start()
    {
        health = MaxHealth;

        MovePattern(pattern, 3f);
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
}
