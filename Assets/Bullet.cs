
using UnityEngine;


public class Bullet : MonoBehaviour
{
    private int Damage;
    public float lifeTime = 5f;
    public float time;
    public GameObject OriginBullet;
    private void OnEnable()
    {
        time = lifeTime;
    }
    public void SetOriginPrefab(GameObject prefab)
    {
        OriginBullet = prefab;
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
        ObjectPoolManager.instance.ReturnPool(OriginBullet, gameObject); //자기자신(Clone)을 보내면 실패함.
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
