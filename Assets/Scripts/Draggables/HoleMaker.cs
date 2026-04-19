using UnityEngine;

public class HoleMaker : Draggable
{
    public Transform sharpPoint;
    public GameObject movingPart;
    public Transform movingPartTransform;
    public SpriteRenderer movingPartSpriteRenderer;

    new void Update()
    {
        base.Update();

        if (G.Mouse.holding == this)
        {
            if (Input.GetMouseButtonDown(1)) // RMB
            {
                MakeHole();
            }
        }
    }

    public void MakeHole()
    {
        foreach(Hole hole in G.Mouse.GetAllHolesOnPosition(sharpPoint.position))
        {
            hole.Punch();
        }
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