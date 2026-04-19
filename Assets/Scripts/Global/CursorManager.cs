using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorDot;

    public enum PlayerCursor
    {
        Dot
    }

    void Awake()
    {
        SetCursor(PlayerCursor.Dot);
    }

    public void SetCursor(PlayerCursor cursor)
    {
        switch (cursor)
        {
            case PlayerCursor.Dot: Cursor.SetCursor(cursorDot, new Vector2(16, 16), CursorMode.Auto); break;
        }
    }
}