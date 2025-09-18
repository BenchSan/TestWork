using UnityEngine;

public class ExplosiveBullet : Bullet
{
    private const float ExplosionRadius = 10f;
    private const int EnemiesLimit = 3;
    [SerializeField] private GameObject _explosionPrefab;

    protected override void OnEnemyHit(Enemy hitEnemy)
    {
        Collider[] hits = new Collider[EnemiesLimit];
        if (Physics.OverlapSphereNonAlloc(transform.position, ExplosionRadius, hits, enemyMask) != 0)
        {
            foreach (var hitColliders in hits)
                if (hitColliders.TryGetComponent(out Enemy enemy))
                {
                    enemy.TakeDamage();
                }
        }
        Destroy(Instantiate(_explosionPrefab, transform.position, Quaternion.identity),3f);
        Destroy(gameObject);
    }
}
