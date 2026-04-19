using UnityEngine;

public class Hole : MonoBehaviour
{
    public Card parentCard;
    public Vector2Int quarterPos;

    public void Punch()
    {
        parentCard.values[quarterPos.x][quarterPos.y] = true;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}