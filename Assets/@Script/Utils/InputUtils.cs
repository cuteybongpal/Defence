using UnityEngine;

public static class InputUtils
{
    public static Vector2 GetMousePos()
    { 
        return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public static Transform GetHoveringObject(string layerName)
    {
        Collider2D collider = Physics2D.OverlapPoint(GetMousePos(), LayerMask.GetMask(layerName));
        if (collider == null)
            return null;
        return collider.transform;
    }
}
