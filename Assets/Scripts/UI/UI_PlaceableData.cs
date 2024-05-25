using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlaceableData : MonoBehaviour
{
    [SerializeField] private Image image;

    private PlaceableData _placeableData;

    public void Init(PlaceableData placeableData)
    {
        _placeableData = placeableData;
        image.sprite = _placeableData.Icon;
    }
}
