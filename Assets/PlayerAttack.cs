using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public PlayerInput playerInput;

    [Header("AttackSettings")]
    public GameObject Bullet;
    public bool canFire = false;
    public float cooltime = 0.5f;//발사지연
    public float currenttime;
    public Transform FirePoint; //총알 시작 위치 및 방향
    public float firePower = 50f;
    public int BulletDamage = 5;

    private void OnEnable()
    {
        //Data Reset
        currenttime = cooltime;
    }
    public void OnAttack(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (!canFire)
        {
            Debug.Log($"[PlayerAttack] 발사 지연시간 {currenttime}초");
            return;
        }
        GameObject bullet = ObjectPoolManager.instance.GetPool(Bullet, FirePoint.transform.position, FirePoint.rotation);

        if (bullet.TryGetComponent(out Bullet perfab))
        {
            perfab.GetStats(BulletDamage);
        }
        if (bullet.TryGetComponent(out Rigidbody2D rb))
        {
            rb.AddForce(FirePoint.up * firePower, ForceMode2D.Impulse);
        }

        currenttime = cooltime;
    }

    private void Update()
    {
        if (currenttime <= 0)
        {
            canFire = true;
        }
        else if (currenttime >= 0)
        {
            currenttime -= Time.deltaTime;
        }
    }
}
