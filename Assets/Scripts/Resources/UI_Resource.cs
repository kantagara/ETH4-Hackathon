using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Resource : MonoBehaviour
{
    public Image image;
    public TMP_Text text;
    
    public void SetResource(ResourceType resourceType, int amount)
    {
        image.sprite = Resources.Load<Sprite>($"Sprites/{resourceType}");
        text.text = amount.ToString();
    }
}
