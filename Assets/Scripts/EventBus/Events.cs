using System;
using DefaultNamespace;

public class OnPlaceableDataSelected : EventArgs
{
    public PlaceableData Data { get; set; }
}

public class OnPlaceablePlaced : EventArgs
{
    public PlaceableObject Data { get; set; }
}