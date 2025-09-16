using UnityEngine;

public class RicochetBullet : BulletBase
{
    private const int MaxBounces = 3;
    private int _bounces;
    private Enemy _lastHit;

    protected override void OnHit(Enemy hitEnemy)
    {
        hitEnemy.TakeDamage();
        _lastHit = hitEnemy;

        if (_bounces >= MaxBounces)
        {
            Destroy(gameObject);
            return;
        }

        var next = EnemyManager.Instance.GetRandomOtherEnemy(_lastHit);
        SetDirection(CalcDirTo(next));
        _bounces++;
    }
}