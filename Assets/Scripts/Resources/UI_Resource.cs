using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Resource : MonoBehaviour
{
    public Image image;
    public TMP_Text text;

    private ResourceType _resourceType;
    private void Awake()
    {
        EventSystem<OnResourceAmountChanged>.Subscribe(OnResourceAmountChanged);
    }

    private void OnDestroy()
    {
        EventSystem<OnResourceAmountChanged>.Unsubscribe(OnResourceAmountChanged);
    }

    private void OnResourceAmountChanged(OnResourceAmountChanged obj)
    {
        if(obj.Data.ResourceType != _resourceType) return;
        text.text = obj.Data.Amount.ToString();
    }

    public void SetResource(ResourceType resourceType, Sprite resourceSprite, int initialAmount)
    {
        _resourceType = resourceType;
        image.sprite = resourceSprite;
        text.text = initialAmount.ToString();
    }
}
