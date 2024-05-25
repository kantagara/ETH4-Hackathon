using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public Transform InstantiationPoint;
    public GameObject ObjectToShoot;
    public LayerMask LayerMask;

    private readonly Collider[] _colliders = new Collider[1];

    private readonly List<RaycastResult> _results = new();

    private BoxCollider _box;

    private float _currentBuildTime;

    private PlaceableData _placeableData;

    private Renderer[] _renderers;
    private Material[] _sharedMaterial;
    public PlaceableState State { get; private set; } = PlaceableState.Placing;

    public bool CanBePlaced { get; private set; } = true;
    public PlaceableData PlaceableData { get; set; }


    private void Awake()
    {
        _box = GetComponent<BoxCollider>();
        _box.enabled = false;
        _camera = Camera.main;
        _renderers = GetComponentsInChildren<Renderer>();
        _sharedMaterial = _renderers.Select(x => x.sharedMaterial).ToArray();
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
        foreach (var renderer1 in _renderers) renderer1.gameObject.SetActive(!isPointerOverUi);
    }

    private void CheckMouseClick()
    {
        if (!Utils.IsPointerOverUIObject() && Input.GetMouseButtonDown(0) && CanBePlaced)
            Place();
    }

    private void Shoot()
    {
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
                renderer.material.SetFloat(Progress, _currentBuildTime / PlaceableData.CurrentStats.BuildTime);
        }
    }


    private void Place()
    {
        _box.enabled = true;
        State = PlaceableState.Building;
        EventSystem<OnPlaceablePlaced>.Invoke(new OnPlaceablePlaced
        {
            Data = this
        });
    }

    private void FollowMouse()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 1000, LayerMask))
        {
            transform.position = hit.point;
            CanBePlaced = CheckPlacement();
            for (var i = 0; i < _sharedMaterial.Length; i++)
                _renderers[i].material.color = CanBePlaced ? Color.white : Color.red;
        }
    }

    private bool CheckPlacement()
    {
        return Physics.OverlapSphereNonAlloc(transform.position, 5, _colliders, 1 << LayerMask.NameToLayer("Towers")) ==
               0;
    }
}