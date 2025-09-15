using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : BulletBase
{
    [SerializeField] private float radius = 3f;
    [SerializeField] private LayerMask enemyMask = ~0;
    [SerializeField] private GameObject _explosionPrefab;

    protected override void OnHit(Enemy hitEnemy)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyMask, QueryTriggerInteraction.Collide);
        foreach (var h in hits)
            if (h.TryGetComponent(out Enemy e))
            {
                e.TakeDamage();
            }
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
