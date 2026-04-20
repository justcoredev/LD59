using DG.Tweening;
using UnityEngine;

public class HoleMaker : Draggable
{
    public float punchCooldown = 0.33f;
    public Transform sharpPoint;
    public GameObject movingPart;
    public Transform movingPartTransform;
    public SpriteRenderer movingPartSpriteRenderer;

    Tween tween;
    bool canPunch = true;

    public override void MouseUp()
    {
        base.MouseUp();

        if (canPunch)
        {
            MakeHole();
        }
    }

    public void MakeHole()
    {
        foreach(Hole hole in G.Mouse.GetAllHolesOnPosition(sharpPoint.position))
        {
            hole.Punch();
        }

        tween?.Kill();
        movingPartTransform.localPosition = new Vector3(0, 0.103f, 0);
        tween = movingPartTransform.DOLocalMoveY(0.64f, punchCooldown).SetEase(Ease.OutQuad);
        DOVirtual.DelayedCall(punchCooldown * 0.9f, () => canPunch = true);
        canPunch = false;
    }

    void OnDrawGizmos()
    {
        if (sharpPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(sharpPoint.position, 0.2f);
        }
    }
}