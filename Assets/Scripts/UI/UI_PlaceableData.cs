using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PlaceableData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    [SerializeField] private GameObject upgradeState;
    [SerializeField] private GameObject cantAfford;

    private Button _button;

    private PlaceableData _placeableData;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() =>
            EventSystem<OnPlaceableDataSelected>.Invoke(new OnPlaceableDataSelected { Data = _placeableData }));
        EventSystem<OnResourceAmountChanged>.Subscribe(ResourceAmountChanged);
        EventSystem<OnUIStateChanged>.Subscribe(OnUIStateChanged);
        EventSystem<OnPlaceableLeveledUp>.Subscribe(OnPlaceableLeveledUp);
    }

    private void OnDestroy()
    {
        EventSystem<OnResourceAmountChanged>.Unsubscribe(ResourceAmountChanged);
        EventSystem<OnPlaceableLeveledUp>.Unsubscribe(OnPlaceableLeveledUp);
        EventSystem<OnUIStateChanged>.Unsubscribe(OnUIStateChanged);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem<OnMouseOverPlaceableData>.Invoke(new OnMouseOverPlaceableData
            { Data = _placeableData, Position = eventData.position });
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventSystem<OnMouseOverLeft>.Invoke(new OnMouseOverLeft());
    }


    private void OnPlaceableLeveledUp(OnPlaceableLeveledUp obj)
    {
        if (obj.Data != _placeableData) return;
        cantAfford.SetActive(!ResourceManager.CanBePurchased(_placeableData.CurrentStats.PurchaseCost));
    }

    private void OnUIStateChanged(OnUIStateChanged obj)
    {
        cantAfford.SetActive(!ResourceManager.CanBePurchased(_placeableData.CurrentStats.PurchaseCost));
        upgradeState.SetActive(obj.NewState == UIState.Upgrading && !cantAfford.activeSelf);
    }

    private void ResourceAmountChanged(OnResourceAmountChanged obj)
    {
        cantAfford.SetActive(!ResourceManager.CanBePurchased(_placeableData.CurrentStats.PurchaseCost));
    }

    public void Init(PlaceableData placeableData)
    {
        _placeableData = placeableData;
        image.sprite = _placeableData.Icon;
        cantAfford.SetActive(!ResourceManager.CanBePurchased(placeableData.CurrentStats.PurchaseCost));
    }

}