using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Color _hitColor = Color.red;
    
    private Color _baseColor;
    private Renderer[] _childrenRenderers;
    private Animator _animator;
    private const string AnimatorTrigger = "HitTrigger";
    private const float HitFlashTime = 0.1f;

    private void Awake()
    {
        InitializeParams();
    }
    
    private void InitializeParams()
    {
        _childrenRenderers = GetComponentsInChildren<Renderer>();
        _baseColor = _childrenRenderers[0].material.color;
        _animator = GetComponent<Animator>();
    }

    public void TakeDamage()
    {
        foreach (var rend in _childrenRenderers)
        {
            rend.material.color = _hitColor;
        }
        _animator.SetTrigger(AnimatorTrigger);
        StartCoroutine(ResetColorAfterDelay());
    }

    private IEnumerator ResetColorAfterDelay()
    {
        yield return new WaitForSeconds(HitFlashTime);

        foreach (var rend in _childrenRenderers)
            rend.material.color = _baseColor;
    }


}
