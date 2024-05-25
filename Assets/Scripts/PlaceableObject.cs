using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public enum PlaceableState
    {
        Placing,
        Building,
        Finished
    }
    
    public class PlaceableObject : MonoBehaviour
    {
        public Transform InstantiationPoint;
        public GameObject ObjectToShoot;
        public LayerMask LayerMask;
        public PlaceableState State { get; private set; } = PlaceableState.Placing;
        
        private bool IsBeingMoved => _placeableData == null;
        private PlaceableData _placeableData;
        private static Camera _camera;

        public bool CanBePlaced { get; private set; } = true;
        public PlaceableData PlaceableData { get; set; }

        private Renderer[] _renderers;
        private Material[] _sharedMaterial;

        private BoxCollider _box;

        private List<RaycastResult> _results = new();


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
            foreach (var renderer1 in _renderers)
            {
                renderer1.gameObject.SetActive(!isPointerOverUi);
            }
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
            
        }


        private void Place()
        {
            _box.enabled = true;
            State = PlaceableState.Building;
            EventSystem<OnPlaceablePlaced>.Invoke(new OnPlaceablePlaced()
            {
                Data = this
            });
        }

        private void FollowMouse()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 1000 ,LayerMask))
            {
                transform.position = hit.point;
                CanBePlaced = CheckPlacement();
                for (int i = 0; i < _sharedMaterial.Length; i++)
                    _renderers[i].material.color = CanBePlaced ? Color.white : Color.red;
            }
        }

        private Collider[] _colliders = new Collider[1];

        private bool CheckPlacement()
        {
            float radius = 5f;
            float distance = 10f;
    
            int layerMask = 1 << LayerMask.NameToLayer("Towers");

            return Physics.OverlapSphereNonAlloc(transform.position, radius, _colliders, layerMask) == 0;
        }

    }
}