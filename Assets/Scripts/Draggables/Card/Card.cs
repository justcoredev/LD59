using System.Linq;
using UnityEngine;

public class Card : Draggable
{
    public bool[][] values = new bool[][]{};
    public Hole[] holes;

    public void SetHolesSortingOrder(int order)
    {
        values = new bool[4][];
        for (int q = 0; q < 4; q++)
            values[q] = new bool[6];

        if (holes == null || holes.Length == 0)
        {
            holes = GetComponentsInChildren<Hole>();

            var sorted = holes
                .OrderByDescending(h => h.transform.localPosition.y)
                .ThenBy(h => h.transform.localPosition.x)
                .ToArray();

            holes = sorted;
            
        }

        for(int i = 0; i < holes.Length; i++)
        {
            holes[i].GetComponent<SpriteRenderer>().sortingOrder = order;

            int row = i / 6;   // 0–3
            int col = i % 6;   // 0–5

            Vector2Int quarterPos = new Vector2Int(
                col % 3, // 0–2
                row % 2  // 0–1
            );

            holes[i].quarterPos = quarterPos;
        }
    }
}