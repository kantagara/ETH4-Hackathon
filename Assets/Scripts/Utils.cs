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
