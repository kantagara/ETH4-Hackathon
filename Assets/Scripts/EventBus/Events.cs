using System;
using UnityEngine;

public class OnPlaceableDataSelected : EventArgs
{
    public PlaceableData Data { get; set; }
}

public class OnPlaceablePlaced : EventArgs
{
    public PlaceableObject PlaceableObject { get; set; }
}

public class OnResourceAmountChanged : EventArgs
{
    public Resource Data { get; set; }
}

public class OnPlaceableLeveledUp : EventArgs
{
    public PlaceableData Data { get; set; }
}

public class OnUIStateChanged : EventArgs
{
    public UIState NewState { get; set; }
    public UIState PreviousState { get; set; }
}

public class OnPlaceableDestroyed : EventArgs
{
    public PlaceableObject PlaceableObject { get; set; }

}

public class OnMouseOverPlaceableData : EventArgs
{
    public Vector3 Position { get; set; }
    public PlaceableData Data { get; set; }
}

public class OnMouseOverLeft : EventArgs
{
    private PlaceableData PlaceableData { get; set; }
}