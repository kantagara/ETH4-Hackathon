using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_CostDisplayStat : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;
    

    public void Init(ResourceCost resCost)
    {
        image.sprite = SpritesManager.Instance.GetSprite(resCost.ResourceType);
        text.color = ResourceManager.CanBePurchased(resCost) ? Color.white : Color.red;
        text.text = resCost.Amount.ToString();
    }
}
