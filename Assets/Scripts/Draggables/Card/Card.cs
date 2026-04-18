using UnityEngine;

public class Card : Draggable
{
    public Hole[] holes;

    public void Start()
    {
        base.Start();
    }

    public void SetHolesSortingOrder(int order)
    {
        if (holes == null || holes.Length == 0) holes = GetComponentsInChildren<Hole>();
        foreach(var h in holes)
            h.GetComponent<SpriteRenderer>().sortingOrder = order;
    }
}