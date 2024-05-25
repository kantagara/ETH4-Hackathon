using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask LayerMask;
    public float launchAngle = 45.0f;
    public Rigidbody projectileRigidbody;
    public Collider targetCollider;
    public PlaceableDataStats stats;

    protected readonly Collider[] _colliders = new Collider[2];

    private Vector3? _previousPosition;
    public Transform LaunchPoint { get; set; }

    private void Start()
    {
        Invoke(nameof(DestroySelf), 3);
    }

    private void Update()
    {
        // Rotate the projectile to face along its velocity vector
        if (_previousPosition != null && projectileRigidbody.velocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(projectileRigidbody.velocity);
    }


    private void LateUpdate()
    {
        if (Physics.OverlapSphereNonAlloc(transform.position, 1, _colliders, LayerMask) > 0)
        {
            if (_colliders[0].transform.gameObject.TryGetComponent<Damageable>(out var damageable))
                damageable.TakeDamage(stats.Damage);
            //Play Particles
            Destroy(gameObject);
        }


        _previousPosition = transform.position;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    public virtual void FireProjectile()
    {
        var toTarget = targetCollider.transform.position - transform.position;
        var distance = toTarget.magnitude;
        var angle = launchAngle * Mathf.Deg2Rad;

        var g = Physics.gravity.magnitude; // Get the gravity value
        var initialVelocity = Mathf.Sqrt(g * distance * distance /
                                         (distance * Mathf.Sin(2 * angle) -
                                          2 * toTarget.y * Mathf.Cos(angle) * Mathf.Cos(angle)));

        // Decompose the initial velocity into vertical and horizontal components
        var velocity = new Vector3(initialVelocity * Mathf.Cos(angle), initialVelocity * Mathf.Sin(angle), 0);

        // Rotate the velocity to point at the target
        velocity = Quaternion.LookRotation(toTarget) * velocity;

        projectileRigidbody.velocity = BallisticVelocity(targetCollider.transform, LaunchPoint);
    }


    public Vector3 BallisticVelocity(Transform target, Transform launchPoint, float minDistance = 1.0f)
    {
        // Get the direction to the target
        // Get the direction to the target
        var direction = target.position - launchPoint.position;

        // Check if the target is too close
        if (direction.magnitude <= minDistance)
            // If too close, shoot straight down
            return -Vector3.up * Mathf.Sqrt(minDistance * Physics.gravity.magnitude);

        // Extract the height difference and retain only the horizontal direction
        var heightDifference = direction.y;
        direction.y = 0; // Nullify the vertical component to calculate horizontal distance

        // Calculate the horizontal distance
        var horizontalDistance = direction.magnitude;

        // Convert launch angle from degrees to radians
        var angleRadians = launchAngle * Mathf.Deg2Rad;

        // Calculate the required vertical component for the given launch angle
        direction.y = horizontalDistance * Mathf.Tan(angleRadians);

        // Adjust the horizontal distance to account for the height difference
        horizontalDistance = horizontalDistance / Mathf.Cos(angleRadians);

        // Calculate the velocity magnitude needed to reach the target
        // Formula: v^2 = (g * d^2) / (d * sin(2 * angle) - 2 * h * cos^2(angle))
        var gravity = Physics.gravity.magnitude; // Use the magnitude of gravity (should be positive)
        var initialVelocity = Mathf.Sqrt(gravity * horizontalDistance * horizontalDistance /
                                         (horizontalDistance * Mathf.Sin(2 * angleRadians) -
                                          2 * heightDifference * Mathf.Cos(angleRadians) * Mathf.Cos(angleRadians)));

        // Normalize the direction vector and scale it by the calculated velocity magnitude
        return initialVelocity * direction.normalized;
    }
}