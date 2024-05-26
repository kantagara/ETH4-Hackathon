using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public enum PlaceableState
{
    Placing,
    Building,
    Finished
}

public class PlaceableObject : MonoBehaviour
{
    private static Camera _camera;
    private static readonly int Progress = Shader.PropertyToID("_Progress");

    [FormerlySerializedAs("LayerMask")] [Header("Placing")]
    public LayerMask BuildingLayerMask;

    [Header("Shooting")] public Transform InstantiationPoint;

    public GameObject UI;
    
    public Projectile ObjectToShoot;

    private readonly Collider[] _colliders = new Collider[1];

    private readonly List<RaycastResult> _results = new();

    private BoxCollider _box;

    private Coroutine _coroutine;

    private float _currentBuildTime;
    private double _nextFire;


    private Renderer[] _renderers;
    private Transform _targetToShoot;
    [field:SerializeField]public PlaceableState State { get; private set; } = PlaceableState.Placing;

    public bool CanBePlaced { get; private set; } = true;
    [field:SerializeField]public PlaceableData PlaceableData { get; set; }

    public Transform OverrideTarget;

    private DamageableBase _damageableBase;

    private void Awake()
    {
        _damageableBase = GetComponent<DamageableBase>();
        _damageableBase.OnDeath += OnDeath;
        _box = GetComponent<BoxCollider>();
        _box.enabled = false;
        _camera = Camera.main;
        _renderers = GetComponentsInChildren<Renderer>(true);
        EnableOrDisableRenderers();
        EventSystem<OnPlaceableLeveledUp>.Subscribe(OnPlaceableLeveledUp);
    }

    private void OnDestroy()
    {
        EventSystem<OnPlaceableLeveledUp>.Unsubscribe(OnPlaceableLeveledUp);
    }

    private void OnPlaceableLeveledUp(OnPlaceableLeveledUp obj)
    {
        if (obj.Data != PlaceableData) return;
        transform.GetChild(PlaceableData.CurrentLevel).gameObject.SetActive(true);
        transform.GetChild(PlaceableData.CurrentLevel - 1).gameObject.SetActive(false);
    }

    private void OnDeath()
    {
        _damageableBase.OnDeath -= OnDeath;
        EventSystem<OnPlaceableDestroyed>.Invoke(new OnPlaceableDestroyed()
        {
            PlaceableObject = this
        });

        Destroy(gameObject);
    }


    private void Update()
    {
        switch (State)
        {
            case PlaceableState.Placing:
                FollowMouse();
                CheckMouseClick();
                EnableOrDisableRenderers();
                break;
            case PlaceableState.Building:
                Building();
                break;
            case PlaceableState.Finished:
                Shoot();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void EnableOrDisableRenderers()
    {
        var isPointerOverUi = Utils.IsPointerOverUIObject(_results);

        foreach (var renderer1 in _renderers)
        {
            renderer1.gameObject.SetActive(false);
        }
        
        UI.SetActive(!isPointerOverUi);
        transform.GetChild(PlaceableData.CurrentLevel).gameObject.SetActive(!isPointerOverUi);
    
    }

    private void CheckMouseClick()
    {
        if (!Utils.IsPointerOverUIObject() && Input.GetMouseButtonDown(0) && CanBePlaced)
            Place();
    }

    private void Shoot()
    {
        RotateTowardsClosestTarget();
        if (Time.time > _nextFire)
        {
            _targetToShoot = FindClosestTarget();
            if (_targetToShoot == null)
            {
                return;
            }

            _nextFire = Time.time + PlaceableData.CurrentStats.FireRate;
            Projectile projectile = Instantiate(ObjectToShoot, InstantiationPoint.position, InstantiationPoint.rotation);
            projectile.SetTarget(_targetToShoot);
            projectile.stats = PlaceableData.CurrentStats;
        }
        
    }

    private void RotateTowardsClosestTarget()
    {
        if (_targetToShoot == null) return;
        var direction = _targetToShoot.position - transform.position;
        var rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 5);
    }

    private Transform FindClosestTarget()
    {
        var enemies = SpawnManager.Instance.SpawnedEnemies;
        if (enemies.Count == 0) return null;
        EnemyController closest = null;
        
        foreach (var enemy in enemies)
        {
            var distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
            if (distanceToEnemy > PlaceableData.CurrentStats.Range)
            {
                continue;
            }
            
            if (closest == null)
            {
                closest = enemy;
                continue;
            }
            
            if (distanceToEnemy < Vector3.Distance(closest.transform.position, transform.position))
                closest = enemy;
        }
        
        return closest?.transform;
    }

    private void Building()
    {
        if (_currentBuildTime > PlaceableData.CurrentStats.BuildTime)
        {
            State = PlaceableState.Finished;
            _currentBuildTime = 0;
        }
        else
        {
            _currentBuildTime += Time.deltaTime;
            foreach (var renderer in _renderers)
            {
                renderer.material.GetFloat(Progress);
                renderer.material.SetFloat(Progress, _currentBuildTime / PlaceableData.CurrentStats.BuildTime);
            }
        }
    }


    private void Place()
    {
        _box.enabled = true;
        State = PlaceableState.Building;
        EventSystem<OnPlaceablePlaced>.Invoke(new OnPlaceablePlaced
        {
            PlaceableObject = this
        });
    }

    private void FollowMouse()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, 1000, BuildingLayerMask)) return;
        
        transform.position = hit.point;
        CanBePlaced = CheckPlacement();
        foreach (var renderer in _renderers)
            renderer.material.color = CanBePlaced ? Color.white : Color.red;
    }

    private bool CheckPlacement()
    {
        return Physics.OverlapSphereNonAlloc(transform.position, 5, _colliders, 1 << LayerMask.NameToLayer("Towers")) ==
               0;
    }
}