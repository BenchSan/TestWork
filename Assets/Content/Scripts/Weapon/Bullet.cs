using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed = 18f;
    [SerializeField] protected LayerMask enemyMask;
    
    private Vector3 _direction;
    private void Start()
    {
        SetDirection(transform.forward);
    }

    protected void SetDirection(Vector3 newDir)
    {
        _direction = newDir.normalized;
    }

    protected void FixedUpdate()
    {
        Vector3 moveBullet = _direction * speed * Time.fixedDeltaTime;
        transform.Translate(moveBullet);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == enemyMask.value)
        {
            if (other.TryGetComponent(out Enemy e))
                OnEnemyHit(e);
        }
    }

    protected virtual void OnEnemyHit(Enemy enemy)
    {
        enemy.TakeDamage();
        Destroy(gameObject);
    }
}
