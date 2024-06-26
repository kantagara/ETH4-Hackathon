using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Placeable Data", fileName = "New Placeable Data")]
public class PlaceableData : ScriptableObject
{
    [field:SerializeField] public string Name { get; private set; }
    [field:SerializeField] public string Description { get; private set; }
    [field:SerializeField] public PlaceableObject Prefab { get; private set; }
    [field:SerializeField] public Sprite Icon { get; private set; }
    
    [field:SerializeField] public PlaceableDataStats[] Stats { get; private set; }
    
    public PlaceableDataStats CurrentStats => Stats[CurrentLevel];
    
    [field: NonSerialized] public int CurrentLevel {  get; private set; } = 0;
    
    public void UpgradeLevel()
    {
        if (CurrentLevel < Stats.Length - 1)
        {
            CurrentLevel++;
            EventSystem<OnPlaceableLeveledUp>.Invoke(new OnPlaceableLeveledUp()
            {
                Data = this
            });
        }
    }
}

[Serializable]
public class PlaceableDataStats
{
    [field: SerializeField] public ResourceCost[] PurchaseCost { get; private set; }
    [field:SerializeField] public float BuildTime { get; private set; }
    [field:SerializeField] public float FireRate { get; private set; }
    [field:SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float Range { get; private set; } = 20;
    
    [field: SerializeField] public ResourceCost[] UpgradeCost { get; private set; }
    [field: SerializeField] public float MaxHealth { get; set; }
}


[Serializable]
public class ResourceCost
{
    [field: SerializeField] public ResourceType ResourceType { get; private set; }
    [field: SerializeField] public int Amount { get; private set; }
}


