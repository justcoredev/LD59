using System.Collections.Generic;
using UnityEngine;

public class Draggable : MouseInteractable, IFirstSceneAwakeListener, IOnFirstSceneStartListener
{
    public static List<Draggable> draggables;

    public float shadowOpacity = 0.5f;
    public float shadowOffset = 0.1f;
    public int topSortingOrder = 100;
    public int afterTopSortingOrder = 90;

    public bool isDragging;

    SpriteRenderer spriteRenderer;
    SpriteRenderer shadowRenderer;

    public Vector3 worldMousePosition;

    Vector3 localClickPoint;

    public void OnFirstSceneAwake()
    {
        Debug.Log("fsa");
        draggables = new List<Draggable>();
    }

    public void OnFirstSceneStart()
    {
        Debug.Log("fss");
        draggables.Add(this);

        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject shadowObj = new GameObject("Shadow");

        shadowObj.transform.localScale = transform.localScale;
        shadowObj.transform.localRotation = transform.localRotation;
        shadowObj.transform.position = transform.position + new Vector3(1, -1) * shadowOffset;
        shadowObj.transform.SetParent(transform);

        shadowRenderer = shadowObj.AddComponent<SpriteRenderer>();
        shadowRenderer.sprite = spriteRenderer.sprite;
        shadowRenderer.color = new Color(0, 0, 0, shadowOpacity);
        shadowRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
        shadowRenderer.enabled = false;
    }

    void Update()
    {
        Debug.Log(draggables.Count);

        worldMousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (isDragging)
        {
            Vector3 worldOffset = transform.TransformVector(localClickPoint);
            transform.position = worldMousePosition - worldOffset;
        }
    }

    public override void MouseDown()
    {
        isDragging = true;
        shadowRenderer.enabled = true;

        // Make this draggable appear on top every other draggable

        int maxOrder = 0;

        foreach (var draggable in draggables)
        {
            var sr = draggable.spriteRenderer;

            if (sr.sortingOrder > maxOrder)
            {
                maxOrder = sr.sortingOrder;
            }
        }

        spriteRenderer.sortingOrder = maxOrder + 2;
        shadowRenderer.sortingOrder = maxOrder + 1;

        // Local point

        localClickPoint = transform.InverseTransformPoint(worldMousePosition);
    }

    public override void MouseUp()
    {
        // Shift all draggables closer to zero to avoid sortingOrders getting too high

        int minOrder = int.MaxValue;

        foreach (var draggable in draggables)
        {
            var sr = draggable.spriteRenderer;

            if (sr.sortingOrder < minOrder)
            {
                minOrder = sr.sortingOrder;
            }
        }

        foreach(var draggable in draggables)
        {
            draggable.spriteRenderer.sortingOrder -= minOrder;
            draggable.shadowRenderer.sortingOrder = draggable.spriteRenderer.sortingOrder - 1;
        }

        shadowRenderer.enabled = false;
        isDragging = false;
    }

    void OnDestroy()
    {
        if (draggables != null)
            draggables.Remove(this);
    }
}