using System;
using UnityEngine;

public class UI_PlaceableDataManger : Singleton<UI_PlaceableDataManger>
{
    [SerializeField] private PlaceableData[] placeableData;
    [SerializeField] private UI_PlaceableData uiPrefab;

    private void Start()
    {
        foreach (var data in placeableData)
        {
            var ui = Instantiate(uiPrefab,transform);
            ui.Init(data);
        }
    }
}
