using System.Collections.Generic;
using UnityEngine;

public class MouseManager : GlobalService
{
    public MouseInteractable hover;
    public MouseInteractable holding;

    void Update()
    {
        hover = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
        List<MouseInteractable> allHitInteractables = new List<MouseInteractable>();

        // Find top

        int bestOrder = int.MinValue;

        foreach (var hit in hits)
        {
            var interactable = hit.collider.GetComponent<MouseInteractable>();
            allHitInteractables.Add(interactable);

            // Hover top only (requires sr for ordering) 
            var sr = hit.collider.GetComponent<SpriteRenderer>();
            if (sr == null) continue;

            if (sr.sortingOrder > bestOrder)
            {
                bestOrder = sr.sortingOrder;
                hover = interactable;
            }
        }

        foreach(var i in allHitInteractables)
            i.MouseHoverAll();

        if (hover != null)
        {
            // Hover
            hover.MouseHover();

            // Down
            if (Input.GetMouseButtonDown(0))
            {
                hover.MouseDown();
                holding = hover;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            foreach(var i in allHitInteractables)
                i.MouseUpAll();

            if (holding != null)
            {
                holding.MouseUp();
                holding = null;
            }
        }        
    }
}