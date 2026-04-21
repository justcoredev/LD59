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
        MoveOut(!isMoved);
        G.Audio.PlayOneShot(G.Audio.Events.BigButton);
    }

    public void MoveOut(bool b)
    {
        isMoved = b;
        Vector2 dest = b ? movedPoint.position : startMonitorPos;
        tween?.Kill();
        tween = transform.parent.DOMoveX(dest.x, 1.5f * 0.33f).SetEase(Ease.OutQuad);
    }
}