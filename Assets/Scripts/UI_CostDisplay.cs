
using DG.Tweening;
using UnityEngine;

public class UI_CostDisplay : Singleton<UI_CostDisplay>
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private UI_CostDisplayStat costPrefab;
    private void Start()
    {
        EventSystem<OnMouseOverPlaceableData>.Subscribe(OnMouseOverPlaceableData);
        EventSystem<OnMouseOverLeft>.Subscribe(OnMouseOverLeft);
    }

    private void OnMouseOverLeft(OnMouseOverLeft obj)
    {
        canvasGroup.DOKill();
        canvasGroup.DOFade(0, .4f);
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void OnMouseOverPlaceableData(OnMouseOverPlaceableData obj)
    {
        var cost = PlaceableManager.Instance.UIState == UIState.Placing
            ? obj.Data.CurrentStats.PurchaseCost
            : obj.Data.CurrentStats.UpgradeCost;
        canvasGroup.DOKill();
        foreach (var resCost in cost)
        {
            var prefab = Instantiate(costPrefab, transform);
            prefab.Init(resCost);
        }
        canvasGroup.DOFade(1, .3f).SetEase(Ease.Linear);
        transform.position = obj.Position + Vector3.up * 30;
       
    }
    
    private void OnDestroy()
    {
        EventSystem<OnMouseOverPlaceableData>.Unsubscribe(OnMouseOverPlaceableData);
    }
}
