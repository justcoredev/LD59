using UnityEngine;

public class MouseManager : GlobalService
{
    public MouseInteractable top;
    public MouseInteractable holding;

    void Update()
    {
        top = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.Log(ray.origin + " " + ray.direction);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);

        // Find top

        int bestOrder = int.MinValue;

        foreach (var hit in hits)
        {
            var sr = hit.collider.GetComponent<SpriteRenderer>();
            if (sr == null) continue;

            if (sr.sortingOrder > bestOrder)
            {
                bestOrder = sr.sortingOrder;
                top = hit.collider.GetComponent<MouseInteractable>();
            }
        }

        if (top != null)
        {
            // Hover

            // Down
            if (Input.GetMouseButtonDown(0))
            {
                top.MouseDown();
                holding = top;
            }
        }

        if (holding != null)
        {
            // Up
            if (Input.GetMouseButtonUp(0))
            {
                holding.MouseUp();
            }
        }
    }
}