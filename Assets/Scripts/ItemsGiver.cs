using DG.Tweening;
using UnityEngine;

public class ItemsGiver : MonoBehaviour
{
    public Draggable CodeList1;
    public Draggable HoleMaker;

    public void Give(Draggable draggable)
    {
        var sr = draggable.GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 0);
        draggable.gameObject.SetActive(true);
        sr.DOColor(Color.white, 0.5f);
        draggable.ApplyForceImpulse(Vector2.down, 5.0f);
    }
}