using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public enum BulletType { Standard, Explosive, Ricochet }

    [Header("Refs")]
    [SerializeField] private Transform _muzzle;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Renderer[] _emissionPistolPartsRenderer;

    [Header("Prefabs")]
    [SerializeField] private GameObject _standardBulletPrefab;
    [SerializeField] private GameObject _explosiveBulletPrefab;
    [SerializeField] private GameObject _ricochetBulletPrefab;
    

    [Header("Colors")]
    [ColorUsage(true,true)][SerializeField] private Color _standardColor = Color.cyan;
    [ColorUsage(true,true)][SerializeField] private Color _explosiveColor = new Color(1f, 0.4f, 0f);
    [ColorUsage(true,true)][SerializeField] private Color _ricochetColor = new Color(0.2f, 1f, 0.2f);
    [ColorUsage(true,true)][SerializeField] private Color _readyFlashColor = Color.white;

    private bool _canShoot = true;
    private BulletType _selectedBulletType = BulletType.Standard;
    private Color _currentGunColor;
    private Coroutine _flashRoutine;

    private const string EmissionColor = "_EmissionColor";
    private const float Cooldown = 1f;
    private const float FlashTime = 0.15f;

    void Start()
    {
        SetStandard();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _canShoot)
        {
            Shoot();
            StartCoroutine(CooldownRoutine());
        }
    }
    
    public void SetStandard()  { _selectedBulletType = BulletType.Standard;  SetGunColor(_standardColor); }
    public void SetExplosive() { _selectedBulletType = BulletType.Explosive; SetGunColor(_explosiveColor); }
    public void SetRicochet()  { _selectedBulletType = BulletType.Ricochet;  SetGunColor(_ricochetColor); }

    private void SetGunColor(Color c)
    {
        _currentGunColor = c;
        ApplyEmission(c);
    }
    
    private void Shoot()
    {
        var target = _enemyManager.GetFrontEnemy();

        var prefab = _selectedBulletType switch
        {
            BulletType.Standard  => _standardBulletPrefab,
            BulletType.Explosive => _explosiveBulletPrefab,
            BulletType.Ricochet  => _ricochetBulletPrefab,
            _ => _standardBulletPrefab
        };

        var go = Instantiate(prefab, _muzzle.position, Quaternion.identity);
        go.GetComponent<BulletBase>().Init(target, _enemyManager);

        _canShoot = false;
    }

    private IEnumerator CooldownRoutine()
    {
        SetBlack();
        yield return new WaitForSeconds(Cooldown);
        _canShoot = true;
        yield return LerpEmission(Color.black, _currentGunColor, FlashTime);
    }
    
    private void ApplyEmission(Color c)
    {
        if (!_canShoot)
        {
            return;
        }
        foreach (var r in _emissionPistolPartsRenderer)
        {
            r.material.SetColor(EmissionColor, c);
        }
    }

    private void SetBlack()
    {
        foreach (var r in _emissionPistolPartsRenderer)
        {
            r.material.SetColor(EmissionColor,Color.black);
        }
    }

    private IEnumerator LerpEmission(Color from, Color to, float time)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float k = t / time;
            ApplyEmission(Color.Lerp(from, to, k));
            yield return null;
        }
        ApplyEmission(to);
    }
}

