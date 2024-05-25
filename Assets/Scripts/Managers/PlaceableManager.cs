
using System.Collections.Generic;
using UnityEngine;

public class PlaceableManager : Singleton<PlaceableManager>
{
    private PlaceableObject _placeableObject;

    protected override bool ShouldntBeDestroyedOnLoad => false;

    public List<Transform> PlaceablesTransforms { get; } = new();

    private void OnEnable()
    {
        EventSystem<OnPlaceableDataSelected>.Subscribe(OnPlaceableDataSelected);
        EventSystem<OnPlaceablePlaced>.Subscribe(OnPlaceablePlaced);
    }

    private void OnPlaceablePlaced(OnPlaceablePlaced obj)
    {
        PlaceablesTransforms.Add(obj.Data.transform);
        if (obj.Data == _placeableObject)
            _placeableObject = null;
    }

    private void OnDisable()
    {
        EventSystem<OnPlaceableDataSelected>.Unsubscribe(OnPlaceableDataSelected);
        EventSystem<OnPlaceablePlaced>.Unsubscribe(OnPlaceablePlaced);
    }
    
    private void OnPlaceableDataSelected(OnPlaceableDataSelected obj)
    {
        if (_placeableObject != null) Destroy(_placeableObject.gameObject);
        _placeableObject = Instantiate(obj.Data.Prefab, transform);
        _placeableObject.PlaceableData = obj.Data;
    }
}
