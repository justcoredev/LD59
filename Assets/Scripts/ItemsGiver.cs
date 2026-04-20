using DG.Tweening;
using UnityEngine;

public class ItemsGiver : MonoBehaviour
{
    public Draggable CodeList1;
    public Draggable ManualList;
    public Draggable HoleMaker;

    public void Give(Draggable draggable)
    {
        var sr = draggable.GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 0);
        draggable.gameObject.SetActive(true);
        sr.DOColor(Color.white, 0.5f);

        draggable.ApplyForceImpulse(
            Vector2.down + 
            new Vector2(
                Random.Range(0.0f, 0.0f), 
                Random.Range(0, -5.0f)), 
            10.0f
        );
    }
}