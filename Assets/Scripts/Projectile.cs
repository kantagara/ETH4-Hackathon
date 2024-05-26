using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask LayerMask;
    public PlaceableDataStats stats;
    public float speed = 5;

    public float duration;

    protected readonly Collider[] _colliders = new Collider[2];
    private float currentTime;

    private Vector3 _previousPosition;


    private void Start()
    {
        currentTime = 0;
    }

    protected virtual void Update()
    {
        if (currentTime <= duration)
            MoveToTarget();
        else transform.Translate(transform.forward * (speed * Time.deltaTime));
    }

    private void MoveToTarget()
    {
        var position = transform.position;
        _previousPosition = position;
        position = Vector3.Lerp(position, _targetPosition, (currentTime)/duration);
        transform.position = position;
        currentTime += Time.deltaTime * speed;
        transform.rotation = Quaternion.LookRotation(position - _previousPosition);
    }

    private Vector3 _targetPosition;

    public virtual void SetTarget(Transform target)
    {
        _targetPosition = target.GetComponent<Collider>().bounds.center;
    }

    private void LateUpdate()
    {
        CheckForCollision();
    }

    private void CheckForCollision()
    {
        if (Physics.OverlapSphereNonAlloc(transform.position, 1, _colliders, LayerMask) <= 0) return;
        if (_colliders[0].transform.gameObject.TryGetComponent<DamageableHardcodedHealth>(out var damageable))
            damageable.TakeDamage(stats.Damage);
        //Play Particles
        Destroy(gameObject);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}