using System.Collections;
using DG.Tweening;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private const float ReloadingCooldown = 1f;
    private const float FlashTime = 0.15f;
    
    [Header("References")]
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Renderer[] _emissionPistolPartsRenderer;

    [Header("Prefabs")]
    [SerializeField] private Bullet _standardBulletPrefab;
    [SerializeField] private Bullet _explosiveBulletPrefab;
    [SerializeField] private Bullet _ricochetBulletPrefab;
    

    [Header("Colors")]
    [ColorUsage(true,true)][SerializeField] private Color _standardColor = Color.cyan;
    [ColorUsage(true,true)][SerializeField] private Color _explosiveColor = new Color(1f, 0.4f, 0f);
    [ColorUsage(true,true)][SerializeField] private Color _ricochetColor = new Color(0.2f, 1f, 0.2f);

    private bool _canShoot = true;
    private Color _currentGunColor;
    private Bullet _selectedBullet;
    
    private void Start()
    {
        SetStandardBullet();
    }

    private void SetBullet(Color gunColor, Bullet selectedBullet)
    {
        _currentGunColor = gunColor;
        _selectedBullet = selectedBullet;
    }

    public void SetStandardBullet() =>  SetBullet(_standardColor, _standardBulletPrefab);
    public void SetExplosiveBullet() =>  SetBullet(_explosiveColor, _explosiveBulletPrefab);
    public void SetRicochetBullet() =>  SetBullet(_ricochetColor, _ricochetBulletPrefab);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _canShoot)
        {
            Shoot();
            StartCoroutine(CooldownRoutine());
        }
    }
    
    private void Shoot()
    {
        Instantiate(_selectedBullet.gameObject, _muzzle.position, Quaternion.identity);
        _canShoot = false;
    }

    private IEnumerator CooldownRoutine()
    {
        ApplyColor(Color.black);
        yield return new WaitForSeconds(ReloadingCooldown);
        ApplyColor(_currentGunColor,FlashTime);
        yield return new WaitForSeconds(FlashTime);
        _canShoot = true;
    }

    private void ApplyColor(Color color, float time = 0f)
    {
        foreach (var r in _emissionPistolPartsRenderer)
            r.material.DOColor(color,EmissionColor, time);
    }
    
    public void SetColorOnButtonClick()
    {
        if(_canShoot)
            ApplyColor(_currentGunColor);
    }
    
}

