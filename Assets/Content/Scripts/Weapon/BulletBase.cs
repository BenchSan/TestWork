using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] protected float speed = 18f;

    protected EnemyManager manager;
    private Vector3 _dir;
    
    public virtual void Init(Enemy target, EnemyManager mgr)
    {
        manager = mgr;
        _dir = CalcDirTo(target);
    }

    private Vector3 CalcDirTo(Enemy target)
    {
        var targetPos = target.transform.position + Vector3.up;
        return (targetPos - transform.position).normalized;
    }
    
    protected void SetDirectionTo(Enemy newTarget)
    {
        _dir = CalcDirTo(newTarget);
    }

    protected virtual void Update()
    {
        transform.position += _dir * speed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy e))
            OnHit(e);
    }

    protected abstract void OnHit(Enemy hitEnemy);
}