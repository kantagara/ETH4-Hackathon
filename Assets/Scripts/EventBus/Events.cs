using System;

public class OnPlaceableDataSelected : EventArgs
{
    public PlaceableData Data { get; set; }
}

public class OnPlaceablePlaced : EventArgs
{
    public PlaceableObject Data { get; set; }
}

public class OnResourceAmountChanged : EventArgs
{
    public Resource Data { get; set; }
}