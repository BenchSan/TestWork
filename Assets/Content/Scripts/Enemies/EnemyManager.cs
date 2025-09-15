using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy _frontEnemy;
    [SerializeField] private Enemy[] _enemies;

    public Enemy GetFrontEnemy() => _frontEnemy;

    public Enemy GetRandomOtherEnemy(Enemy exclude)
    {
        var list = _enemies.Where(e => e != exclude).ToList();
        return list[Random.Range(0, list.Count)];
    }
}
