using UnityEngine;

public class RicochetBullet : Bullet
{
    private const int MaxBounces = 3;
    private const float SearchingEnemiesRadius = 10f;
    private int _bounces;
    private Enemy _lastHit;

    protected override void OnEnemyHit(Enemy lastHitEnemy)
    {
        lastHitEnemy.TakeDamage();

        if (_bounces >= MaxBounces)
        {
            Destroy(gameObject);
            return;
        }
        Collider[] hits = new Collider[3];
        if (Physics.OverlapSphereNonAlloc(transform.position, SearchingEnemiesRadius, hits,enemyMask) != 0)
        {
            Enemy nextEnemy = null;
            while (nextEnemy == null || nextEnemy == lastHitEnemy)
            {
                nextEnemy = hits[Random.Range(0, hits.Length)].gameObject.GetComponent<Enemy>();
            }
            SetDirection(CalculateDirectionToEnemy(nextEnemy));
            _bounces++;
        }

    }
    private Vector3 CalculateDirectionToEnemy(Enemy target)
    {
        return (target.CenterPoint - transform.position).normalized;
    }
}