using System.Linq;
using UnityEngine;

public class Card : Draggable
{
    public bool[][] values = new bool[][]{};
    public Hole[] holes;

    new void Update()
    {
        base.Update();
        Debug.Log("f " + values[0][0]);
    }

    new void Start()
    {
        base.Start();

        values = new bool[4][];
        for (int q = 0; q < 4; q++)
            values[q] = new bool[6];

        holes = GetComponentsInChildren<Hole>();

        var sorted = holes
            .OrderByDescending(h => h.transform.localPosition.y)
            .ThenBy(h => h.transform.localPosition.x)
            .ToArray();

        holes = sorted;

        var orderedHoles = holes
            .OrderBy(h => h.transform.GetSiblingIndex())
            .ToArray();

        for (int i = 0; i < orderedHoles.Length; i++)
        {
            Debug.Log(i + ";" + orderedHoles[i].gameObject.name);
            Vector2Int quarterPos = new Vector2Int(
                i / 6,
                i % 6
            );

            orderedHoles[i].quarterPos = quarterPos;
        }

        holes = orderedHoles;
    }

    public void SetHolesSortingOrder(int order)
    {
        for(int i = 0; i < holes.Length; i++)
        {
            holes[i].GetComponent<SpriteRenderer>().sortingOrder = order;
        }
    }
}