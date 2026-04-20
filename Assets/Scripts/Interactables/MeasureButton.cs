using UnityEngine;

public class MeasureButton : MouseInteractable
{
    public Color lockedColor;
    public Color pressedColor;
    public Color releasedColor;
    public bool IsLocked {get; private set;}
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Lock(true);
    }

    public void Lock(bool l)
    {
        if (l)
        {
            sr.color = lockedColor;
            IsLocked = true;
        }
        else
        {
            sr.color = releasedColor;
            IsLocked = false;
        }
    }

    public void Press()
    {
        if (IsLocked) return;

        Lock(true);
        Sensor.FindByID("temp").Activate();
        Sensor.FindByID("pressure").Activate();
        // TODO: play sound
    }

    public override void MouseDown()
    {
        Press();
    }
}