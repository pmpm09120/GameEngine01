using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHealth = 30;
    [SerializeField] private int health;

    private void Start()
    {
        health = MaxHealth;
    }
    public int _health
    {
        get { return health; }
        set
        {
            health = Mathf.Clamp(value, 0, MaxHealth);
        }
    }

    public void TakeDamage(int dam)
    {
        _health -= dam;
        if (health <= 0)
        {
            Debug.Log($"[TK]{gameObject.name} Died");
            ObjectPoolManager.instance.ReturnPool(gameObject,gameObject); //인수로 들어가는 객체는 클론=> 코드 실패
        }
    }
}
