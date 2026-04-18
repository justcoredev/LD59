using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(TargetJoint2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Draggable : MouseInteractable, IOnCleanupListener
{
    public static List<Draggable> draggables;

    public float shadowOpacity = 0.7f;
    public float shadowOffset = 0.12f;
    public bool isDragging;
    [HideInInspector] public Vector3 worldMousePosition;

    Vector3 localClickPoint;

    SpriteRenderer spriteRenderer;
    SpriteRenderer shadowRenderer;

    Rigidbody2D rb;
    TargetJoint2D joint;
    
    private void Awake()
    {
        if (draggables == null)
            draggables = new List<Draggable>();

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
        shadowRenderer.sortingOrder = spriteRenderer.sortingOrder - 2;
        shadowRenderer.enabled = false;

        InitPhysics();
    }

    public void Start()
    {
        AppearOnTop();
    }

    void Reset() => InitPhysics();

    void InitPhysics()
    {
        joint = GetComponent<TargetJoint2D>();
        joint.enabled = false;
        joint.frequency = 10f;

        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = 26.0f;
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    public void OnCleanup()
    {
        draggables = new List<Draggable>();
    }

    void Update()
    {
        worldMousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (isDragging)
        {
            joint.target = worldMousePosition;
        }
    }

    public override void MouseDown()
    {
        isDragging = true;
        shadowRenderer.enabled = true;
        joint.enabled = true;

        // Make this draggable appear on top every other draggable

        AppearOnTop();

        // Local point

        localClickPoint = transform.InverseTransformPoint(worldMousePosition);
        joint.anchor = localClickPoint;
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
            if (draggable is Card card) card.SetHolesSortingOrder(draggable.spriteRenderer.sortingOrder - 1);
            draggable.shadowRenderer.sortingOrder = draggable.spriteRenderer.sortingOrder - 2;
        }

        shadowRenderer.enabled = false;
        joint.enabled = false;
        isDragging = false;
    }

    void AppearOnTop()
    {
        int maxOrder = 0;

        foreach (var draggable in draggables)
        {
            var sr = draggable.spriteRenderer;

            if (sr.sortingOrder > maxOrder)
            {
                maxOrder = sr.sortingOrder;
            }
        }

        spriteRenderer.sortingOrder = maxOrder + 3;
        if (TryGetComponent<Card>(out var card)) card.SetHolesSortingOrder(maxOrder + 2);
        shadowRenderer.sortingOrder = maxOrder + 1;
    }

    void OnDestroy()
    {
        if (draggables != null)
            draggables.Remove(this);
    }

    public void ApplyForceImpulse(Vector2 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}