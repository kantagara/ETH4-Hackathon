using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(DamageableHardcodedHealth))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float damageDealing = 20;
    [field:SerializeField] public DamageableBase Damageable { get; private set; }
    [SerializeField] private NavMeshAgent navmeshAgent;

    private DamageableBase _targetDamageable;


    public float velocity;
   

    private void Start()
    {
        time = Time.time;
        Damageable.OnDeath += OnDeath;
        EventSystem<OnPlaceablePlaced>.Subscribe(OnPlaceablePlace);
        SetRandomTarget();
    }

    void SetRandomTarget()
    {
        SetTarget(Random.Range(0, 1f) < 0.5f &&
                  PlaceableManager.Instance.PlaceablesTransforms.Count > 0
            ? PlaceableManager.Instance.PlaceablesTransforms[
                Random.Range(0, PlaceableManager.Instance.PlaceablesTransforms.Count)]
            : GameObject.FindWithTag("Castle").transform
        );
    }

    private float time;

    private Coroutine _coroutine;
    private void Update()
    {
        velocity = navmeshAgent.velocity.sqrMagnitude;
        //Add small delay before the check
        if (velocity < 0.6f && _coroutine == null 
            && Time.time - time > 3f)
        {
            _coroutine = StartCoroutine(Attack());
        }
        
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _targetDamageable.TakeDamage(damageDealing);
            yield return new WaitForSeconds(Random.Range(1, 2f));
        }
    }

    private void OnDestroy()
    {
        Damageable.OnDeath -= OnDeath;
        EventSystem<OnPlaceablePlaced>.Unsubscribe(OnPlaceablePlace);
    }

    private void OnPlaceablePlace(OnPlaceablePlaced obj)
    {
        if (_targetDamageable == null)
        {
            SetTarget(obj.PlaceableObject.transform);
            return;
        }
        
        if (Vector3.Distance(_targetDamageable.transform.position, transform.position) <
            Vector3.Distance(obj.PlaceableObject.transform.position, transform.position))
            return;
        if (Random.Range(0, 1f) < 0.5f) return;
        
        SetTarget(obj.PlaceableObject.transform);
    }

    private void SetTarget(Transform dataTransform)
    {
        if (_targetDamageable != null) _targetDamageable.OnDeath -= TargetDamageableOnDeath;
        _targetDamageable = dataTransform.GetComponent<DamageableBase>();
        _targetDamageable.OnDeath += TargetDamageableOnDeath;
        navmeshAgent.SetDestination(_targetDamageable.Collider.GetRandomPointInBounds());
        if(_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private void TargetDamageableOnDeath()
    {
        _targetDamageable.OnDeath -= TargetDamageableOnDeath;
        SetRandomTarget();
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }
}