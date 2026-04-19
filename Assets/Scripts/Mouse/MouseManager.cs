using System.Collections.Generic;
using UnityEngine;

public class MouseManager : GlobalService
{
    public MouseInteractable hover;
    public MouseInteractable holding;

    public Vector3 worldMousePosition;

    void Update()
    {
        worldMousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        hover = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
        List<MouseInteractable> allHitInteractables = new List<MouseInteractable>();

        // Find top

        int bestOrder = int.MinValue;

        foreach (var hit in hits)
        {
            var interactable = hit.collider.GetComponent<MouseInteractable>();
            if (interactable == null) continue;

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

    public MouseInteractable GetTopOnPosition(Vector3 position)
    {
        Ray ray = new Ray(new Vector3(position.x, position.y, -10), Vector3.forward);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
        List<MouseInteractable> allHitInteractables = new List<MouseInteractable>();

        MouseInteractable top = null;

        // Find top

        int bestOrder = int.MinValue;

        foreach (var hit in hits)
        {
            var interactable = hit.collider.GetComponent<MouseInteractable>();
            if (interactable == null) continue;

            allHitInteractables.Add(interactable);

            // Hover top only (requires sr for ordering) 
            var sr = hit.collider.GetComponent<SpriteRenderer>();
            if (sr == null) continue;

            if (sr.sortingOrder > bestOrder)
            {
                bestOrder = sr.sortingOrder;
                top = interactable;
            }
        }

        return top;
    }

    public List<Hole> GetAllHolesOnPosition(Vector3 position)
    {
        Ray ray = new Ray(new Vector3(position.x, position.y, -10), Vector3.forward);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
        List<Hole> allHitHoles = new List<Hole>();

        foreach (var hit in hits)
        {
            var hole = hit.collider.GetComponent<Hole>();
            if (hole == null) continue;

            allHitHoles.Add(hole);
        }

        return allHitHoles;
    }
}