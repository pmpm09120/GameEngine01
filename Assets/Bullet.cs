
using UnityEngine;


public class Bullet : MonoBehaviour
{
    private int Damage;
    public float lifeTime = 5f;
    public float time;
    public PerfabID_Data bulletData;
    private void OnEnable()
    {
        time = lifeTime;
    }
    public void GetStats(int dam)
    {
        Damage = dam;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            return;
        }
        if (collision.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(Damage);
        }
        ReturnBullet();
    }

    private void ReturnBullet()
    {
        ObjectPoolManager.instance.ReturnPool(bulletData.id, gameObject); 
    }

    private void Update()
    {
        if (time >= 0)
        {
            time -= Time.deltaTime;
        }
        else if (time <= 0)
        {
            ReturnBullet();
        }
    }
}
