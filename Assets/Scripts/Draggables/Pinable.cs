using UnityEngine;

public class Pinable : Draggable
{
    public Sprite bigSprite;
    public Sprite smallSprite;

    SpriteRenderer sr;

    new void Start()
    {
        base.Start();
        sr = GetComponent<SpriteRenderer>();
    }

    new void Update()
    {
        base.Update();

        if (InPinboardBounds())
        {
            if (G.Mouse.holding == this)
            {
                sr.sprite = smallSprite;
                if (G.Pinboard.Pinable == this)
                {
                    G.Pinboard.Pinable = null;
                    transform.rotation = Quaternion.identity;
                }
            }
            else if (G.Pinboard.Pinable == null)
            {
                Pin();
            }
        }
        else
        {
            sr.sprite = smallSprite;
        }
    }

    public void Pin()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = bigSprite;
        transform.position = G.Pinboard.PinPoint.position;
        transform.rotation = G.Pinboard.PinPoint.rotation;
        G.Pinboard.Pinable = this;
        var rb = G.Pinboard.Pinable.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    public bool InPinboardBounds()
    {
        return transform.position.x > G.Pinboard.Bound0.position.x && transform.position.x < G.Pinboard.Bound1.position.x &&
            transform.position.y > G.Pinboard.Bound0.position.y && transform.position.y < G.Pinboard.Bound1.position.y;
    }
}