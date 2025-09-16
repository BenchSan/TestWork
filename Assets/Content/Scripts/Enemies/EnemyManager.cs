using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    [SerializeField] private Enemy _frontEnemy;
    [SerializeField] private Enemy[] _enemies;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public Enemy GetFrontEnemy() => _frontEnemy;

    public Enemy GetRandomOtherEnemy(Enemy exclude)
    {
        Enemy[] array = _enemies.Where(e => e != exclude).ToArray();
        return array[Random.Range(0, array.Length)];
    }
}
