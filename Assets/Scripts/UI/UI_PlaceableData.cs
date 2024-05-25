using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    public void Init(PlaceableData placeableData)
    {
        _placeableData = placeableData;
        image.sprite = _placeableData.Icon;
    }
}
