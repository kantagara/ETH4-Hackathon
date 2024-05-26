using UnityEngine;

[RequireComponent(typeof(PlaceableObject))]
public class DamageablePlaceableObject : DamageableBase
{
    [SerializeField] private PlaceableObject placeableObject;
    public override float MaxHealth => placeableObject.PlaceableData.CurrentStats.MaxHealth;

    private void Start()
    {
        EventSystem<OnPlaceableLeveledUp>.Subscribe(PlaceableLeveledUp);
    }

    private void OnDestroy()
    {
        EventSystem<OnPlaceableLeveledUp>.Unsubscribe(PlaceableLeveledUp);
    }

    private void PlaceableLeveledUp(OnPlaceableLeveledUp obj)
    {
        if (obj.Data != placeableObject.PlaceableData) return;
        
        //Increasing maximum health to accommodate the new level
        var previousRatio = currentHealth / obj.Data.Stats[obj.Data.CurrentLevel - 1].MaxHealth;
        currentHealth = MaxHealth * previousRatio;
    }
}
