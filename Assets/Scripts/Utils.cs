using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public static class Utils
{
    public static bool IsPointerOverUIObject(List<RaycastResult> raycastResults = null)
    {
        raycastResults?.Clear();
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };
        List<RaycastResult> results = raycastResults ?? new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}

public static class Extensions
{
    public static Vector3 GetRandomPointInBounds(this Collider collider)
    {
        var bounds = collider.bounds;
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
