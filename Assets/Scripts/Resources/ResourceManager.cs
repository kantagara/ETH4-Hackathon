using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ResourceType
{
    Wood,
    Stone,
    Diamond
}

public class ResourceManager : Singleton<ResourceManager>
{
    public Resource[] Resources;

    public Transform ResourceUIParent;
    public UI_Resource UIResource;

    private Dictionary<ResourceType, Resource> _resourcesDict;
    
    public static bool CanBePurchased(params ResourceCost[] resourceCosts)
     => resourceCosts.All(x => Instance._resourcesDict[x.ResourceType].Amount >= x.Amount);

    public static void Purchase(params ResourceCost[] resourceCosts)
    {
        foreach (var resourceCost in resourceCosts)
        {
            Instance._resourcesDict[resourceCost.ResourceType].Amount -= resourceCost.Amount;
        }
    }

    private void Start()
    {
        _resourcesDict = Resources.ToDictionary(x => x.ResourceType, x => x);
        
        foreach (var resource in Resources)
        {
            resource.Amount = resource.InitialAmount;
            var uiResource = Instantiate(UIResource, ResourceUIParent);
            uiResource.SetResource(resource.ResourceType, resource.sprite, resource.InitialAmount);
        }
        
        EventSystem<OnPlaceablePlaced>.Subscribe(PlaceablePlaced);
    }

    private void OnDestroy()
    {
        EventSystem<OnPlaceablePlaced>.Unsubscribe(PlaceablePlaced);
    }

    private void PlaceablePlaced(OnPlaceablePlaced obj)
    {
        Purchase(obj.PlaceableObject.PlaceableData.CurrentStats.PurchaseCost);
    }
}

[Serializable]
public class Resource
{
    public int InitialAmount = 100;
    public ResourceType ResourceType;
    public string TokenId;
    public Sprite sprite;
    private int _amount;

    public int Amount
    {
        get => _amount;
        set
        {
            _amount = value;
            EventSystem<OnResourceAmountChanged>.Invoke(new OnResourceAmountChanged()
            {
                Data = this
            });
        }
        
    }
}
