using UnityEngine;

public class FollowProjectile : Projectile
{
    public float Speed;
    
    private void Start()
    {
        Invoke(nameof(DestroySelf), 20);
    }

    private void Update()
    {
        if (targetCollider == null)
        {
            Destroy(gameObject);
            return;
        }
        projectileRigidbody.velocity = (targetCollider.bounds.center - transform.position).normalized * Speed;
        if (Physics.OverlapSphereNonAlloc(transform.position, 1, _colliders, LayerMask) <= 0) return;
        if (_colliders[0].transform.gameObject.TryGetComponent<Damageable>(out var damageable))
        {
            damageable.TakeDamage(stats.Damage);
        }else Debug.LogError(_colliders[0].transform.gameObject.name);
        //Play Particles
        Destroy(gameObject);
    }

    public override void FireProjectile()
    {
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}