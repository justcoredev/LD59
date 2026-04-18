using UnityEngine;

public class CardEater : MouseInteractable
{
    public override void MouseHoverAll()
    {
        Debug.Log("GMOUSE " + G.Mouse);
        Debug.Log("HOLDING " + G.Mouse.holding);
        if (G.Mouse.holding != null)
        {
            // some hover effect like shake
        }
    }

    public override void MouseUpAll()
    {
        if (G.Mouse.holding != null)
        {
            // EAT THE CARD
            Destroy(G.Mouse.holding.gameObject);
        }
    }
}