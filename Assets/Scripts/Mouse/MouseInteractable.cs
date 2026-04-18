using UnityEngine;

public class MouseInteractable : MonoBehaviour
{
    public virtual void MouseHover() {}
    public virtual void MouseDown() {}
    public virtual void MouseUp() {}

    public virtual void MouseHoverAll() {}
    public virtual void MouseUpAll() {}
}