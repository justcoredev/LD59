using DG.Tweening;
using UnityEngine;

public class MonitorMoveButton : MouseInteractable
{
    public bool isMoved;
    public Transform movedPoint;
    Vector2 startMonitorPos;
    Tween tween;

    void Start()
    {
        startMonitorPos = transform.parent.position;
    }

    public override void MouseDown()
    {
        Vector2 dest = default;

        if (isMoved)
        {
            isMoved = false;
            dest = startMonitorPos;
        }
        else
        {
            isMoved = true;
            dest = movedPoint.position;
        }

        tween?.Kill();
        tween = transform.parent.DOMoveX(dest.x, 1.5f * 0.33f).SetEase(Ease.OutQuad);
    }
}