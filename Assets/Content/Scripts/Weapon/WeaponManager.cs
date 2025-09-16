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
    [SerializeField] private Color _standardColor = Color.cyan;
    [SerializeField] private Color _explosiveColor = new Color(1f, 0.4f, 0f);
    [SerializeField] private Color _ricochetColor = new Color(0.2f, 1f, 0.2f);
    [SerializeField] private Color _readyFlashColor = Color.white;

    private bool _canShoot = true;
    private BulletType _selectedBulletType = BulletType.Standard;
    private Color _currentGunColor;
    private Coroutine _flashRoutine;

    private const string EmissionColor = "_EmissionColor";
    private const float Cooldown = 1f;
    private const float FlashTime = 0.15f;

    
    public void SetStandard()  { _selectedBulletType = BulletType.Standard;  _currentGunColor = _standardColor;  if (_canShoot) StartCoroutine(ChangeEmissionRoutine(_currentGunColor, _currentGunColor, 0f)); }
    public void SetExplosive() { _selectedBulletType = BulletType.Explosive; _currentGunColor = _explosiveColor; if (_canShoot) StartCoroutine(ChangeEmissionRoutine(_currentGunColor, _currentGunColor, 0f)); }
    public void SetRicochet()  { _selectedBulletType = BulletType.Ricochet;  _currentGunColor = _ricochetColor;  if (_canShoot) StartCoroutine(ChangeEmissionRoutine(_currentGunColor, _currentGunColor, 0f)); }
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
    
    private void Shoot()
    {
        Enemy target = _enemyManager.GetFrontEnemy();

        GameObject prefab = _selectedBulletType switch
        {
            BulletType.Standard  => _standardBulletPrefab,
            BulletType.Explosive => _explosiveBulletPrefab,
            BulletType.Ricochet  => _ricochetBulletPrefab,
        };

        GameObject go = Instantiate(prefab, _muzzle.position, Quaternion.identity);
        go.GetComponent<BulletBase>().Init(target);
        _canShoot = false;
        StartCoroutine(ChangeEmissionRoutine(_currentGunColor, Color.black, 0f));
    }

    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(Cooldown);
        yield return ChangeEmissionRoutine(Color.black, _currentGunColor, FlashTime);
        _canShoot = true;
    }

    private void ApplyColor(Color c)
    {
        foreach (var r in _emissionPistolPartsRenderer)
            r.material.SetColor(EmissionColor, c);
    }

    private IEnumerator ChangeEmissionRoutine(Color from, Color to, float time)
    {
        if (time <= 0f)
        {
            ApplyColor(to);
            yield break;
        }

        float t = 0f;
        ApplyColor(from);
        while (t < time)
        {
            t += Time.deltaTime;
            float k = t / time;
            ApplyColor(Color.Lerp(from, to, k));
            yield return null;
        }
        ApplyColor(to);
    }
}

