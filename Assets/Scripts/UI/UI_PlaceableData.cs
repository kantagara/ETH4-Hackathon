using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlaceableData : MonoBehaviour
{
    [SerializeField] private Image image;

    private Button _button;
    private PlaceableData _placeableData;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => EventSystem<OnPlaceableDataSelected>.Invoke(new OnPlaceableDataSelected(){ Data = _placeableData }));
        EventSystem<OnResourceAmountChanged>.Subscribe(ResourceAmountChanged);
    }

    private void Start()
    {
        _button.enabled = ResourceManager.CanBePurchased(_placeableData.CurrentStats.PurchaseCost);
    }

    private void OnDestroy()
    {
        EventSystem<OnResourceAmountChanged>.Unsubscribe(ResourceAmountChanged);
    }

    private void ResourceAmountChanged(OnResourceAmountChanged obj)
    {
        _button.enabled = ResourceManager.CanBePurchased(_placeableData.CurrentStats.PurchaseCost);
    }

    public void Init(PlaceableData placeableData)
    {
        _placeableData = placeableData;
        image.sprite = _placeableData.Icon;
    }
}
