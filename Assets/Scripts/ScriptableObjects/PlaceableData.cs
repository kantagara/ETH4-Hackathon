using System;
using DefaultNamespace;
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
    
    [field: NonSerialized] private int CurrentLevel {  get; set; } = 0;
    
    public void UpgradeLevel()
    {
        if (CurrentLevel < Stats.Length - 1)
        {
            CurrentLevel++;
        }
    }
}

[Serializable]
public class PlaceableDataStats
{
    public float FireRate { get; set; }
    public float Damage { get; set; }
}