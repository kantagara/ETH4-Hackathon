
using System;
using System.Collections.Generic;
using UnityEngine;

public enum UIState
{
    Placing,
    Upgrading,
}

public class PlaceableManager : Singleton<PlaceableManager>
{
    private PlaceableObject _placeableObject;

    public UIState UIState { get; set; } = UIState.Placing;
    protected override bool ShouldntBeDestroyedOnLoad => false;

    public List<Transform> PlaceablesTransforms { get; } = new();

    private void OnEnable()
    {
        EventSystem<OnPlaceableDataSelected>.Subscribe(OnPlaceableDataSelected);
        EventSystem<OnPlaceablePlaced>.Subscribe(OnPlaceablePlaced);
        EventSystem<OnPlaceableDestroyed>.Subscribe(OnPlaceableDestroyed);
    }

    private void OnPlaceableDestroyed(OnPlaceableDestroyed obj)
    {
        PlaceablesTransforms.Remove(obj.PlaceableObject.transform);
    }

    public void SetUIState(string newState)
    {
        var state = Enum.Parse<UIState>(newState, true);
        if(UIState == state)return;
        EventSystem<OnUIStateChanged>.Invoke(new OnUIStateChanged()
        {
            NewState = state,
            PreviousState = UIState
        });
        UIState = state;
        if (_placeableObject != null) Destroy(_placeableObject.gameObject);
    }

    private void OnPlaceablePlaced(OnPlaceablePlaced obj)
    {
        PlaceablesTransforms.Add(obj.PlaceableObject.transform);
        if (obj.PlaceableObject == _placeableObject)
            _placeableObject = null;
    }

    private void OnDisable()
    {
        EventSystem<OnPlaceableDataSelected>.Unsubscribe(OnPlaceableDataSelected);
        EventSystem<OnPlaceablePlaced>.Unsubscribe(OnPlaceablePlaced);
        EventSystem<OnPlaceableDestroyed>.Unsubscribe(OnPlaceableDestroyed);
    }
    
    private void OnPlaceableDataSelected(OnPlaceableDataSelected obj)
    {
        if (_placeableObject != null) Destroy(_placeableObject.gameObject);

        if (UIState == UIState.Placing)
        {
            _placeableObject = Instantiate(obj.Data.Prefab, transform);
        }
        else if (UIState == UIState.Upgrading)
        {
            if (obj.Data.CurrentLevel < obj.Data.Stats.Length - 1 && ResourceManager.CanBePurchased(obj.Data.CurrentStats.UpgradeCost))
            {
                ResourceManager.Purchase(obj.Data.CurrentStats.UpgradeCost);
                obj.Data.UpgradeLevel();
            }
        }
    }
}
