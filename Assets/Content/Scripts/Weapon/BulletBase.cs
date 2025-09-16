using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] protected float speed = 18f;

    private readonly Vector3 _aimYOffset = Vector3.up;
    private Vector3 _dir;

    public virtual void Init(Enemy target)
    {
        SetDirection(CalcDirTo(target));
    }

    protected Vector3 CalcDirTo(Enemy target)
    {
        var targetPos = target.transform.position + _aimYOffset;
        return (targetPos - transform.position).normalized;
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

    protected void SetDirection(Vector3 newDir)
    {
        _dir = newDir.normalized;
    }

    protected abstract void OnHit(Enemy hitEnemy);
}