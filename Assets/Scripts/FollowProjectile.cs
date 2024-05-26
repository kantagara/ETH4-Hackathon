using UnityEngine;

public class FollowProjectile : Projectile
{
    public Rigidbody projectileRigidbody;
    public float Speed;

    private Collider _targetCollider;

    public override void SetTarget(Transform target)
    {
        _targetCollider = target.GetComponent<Collider>();
    }

    private void Start()
    {
        Invoke(nameof(DestroySelf), 20);
    }

    protected override void Update()
    {
        if (_targetCollider == null)
        {
            Destroy(gameObject);
            return;
        }
        projectileRigidbody.velocity = (_targetCollider.bounds.center - transform.position).normalized * Speed;
        if (Physics.OverlapSphereNonAlloc(transform.position, 1, _colliders, LayerMask) <= 0) return;
        if (_colliders[0].transform.gameObject.TryGetComponent<DamageableHardcodedHealth>(out var damageable))
        {
            damageable.TakeDamage(stats.Damage);
        }else Debug.LogError(_colliders[0].transform.gameObject.name);
        //Play Particles
        Destroy(gameObject);
    }

  

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}