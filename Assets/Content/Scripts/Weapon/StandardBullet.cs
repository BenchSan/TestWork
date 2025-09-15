using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBullet : BulletBase
{
    protected override void OnHit(Enemy e)
    {
        e.TakeDamage();
        Destroy(gameObject);
    }
}
