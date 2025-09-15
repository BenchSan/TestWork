using UnityEngine;

public class RicochetBullet : BulletBase
{
    [SerializeField] private int maxBounces = 3;
    private int _bounces;
    private Enemy _lastHit;

    protected override void OnHit(Enemy hitEnemy)
    {
        hitEnemy.TakeDamage();
        _lastHit = hitEnemy;

        if (_bounces >= maxBounces)
        {
            Destroy(gameObject);
            return;
        }

        var next = manager.GetRandomOtherEnemy(_lastHit);
        SetDirectionTo(next);
        _bounces++;
    }
}