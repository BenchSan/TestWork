using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector3 CenterPoint => _centerPoint.position;
    [SerializeField] private Color _hitColor = Color.red;
    [SerializeField] private Transform _centerPoint;
    
    private static readonly int HitTrigger = Animator.StringToHash("HitTrigger");
    private readonly Color _baseColor = Color.white;
    private Renderer[] _childrenRenderers;
    private Animator _animator;
    private const float HitFlashTime = 0.1f;

    private void Start()
    {
        InitializeParams();
    }
    
    private void InitializeParams()
    {
        _childrenRenderers = GetComponentsInChildren<Renderer>();
        _animator = GetComponent<Animator>();
    }

    public void TakeDamage()
    {
        _animator.SetTrigger(HitTrigger);
        StartCoroutine(ResetColorAfterDelay());
    }

    private IEnumerator ResetColorAfterDelay()
    {
        foreach (var rend in _childrenRenderers)
            rend.material.color = _hitColor;
        yield return new WaitForSeconds(HitFlashTime);
        foreach (var rend in _childrenRenderers)
            rend.material.color = _baseColor;
    }
}
