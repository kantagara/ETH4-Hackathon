using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [field:SerializeField] public Damageable Damageable { get; private set; }
    [SerializeField] private NavMeshAgent navmeshAgent;

    private Transform _target;

    private void Start()
    {
        Damageable.OnDeath += OnDeath;
        EventSystem<OnPlaceablePlaced>.Subscribe(OnPlaceablePlace);
        SetTarget(Random.Range(0, 1f) < 0.5f &&
                  PlaceableManager.Instance.PlaceablesTransforms.Count > 0
            ? PlaceableManager.Instance.PlaceablesTransforms[
                Random.Range(0, PlaceableManager.Instance.PlaceablesTransforms.Count)]
            : GameObject.FindWithTag("Castle").transform
        );
    }

    private void OnDestroy()
    {
        Damageable.OnDeath -= OnDeath;
        EventSystem<OnPlaceablePlaced>.Unsubscribe(OnPlaceablePlace);
    }

    private void OnPlaceablePlace(OnPlaceablePlaced obj)
    {
        if (Vector3.Distance(_target.position, transform.position) <
            Vector3.Distance(obj.Data.transform.position, transform.position))
            return;
        if (Random.Range(0, 1f) < 0.5f) return;
        SetTarget(obj.Data.transform);
    }

    private void SetTarget(Transform dataTransform)
    {
        _target = dataTransform;
        navmeshAgent.SetDestination(_target.GetComponent<Collider>().GetRandomPointInBounds());
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }
}