using UnityEngine;

public class Pinable : Draggable
{
    public static Transform PinboardBound0;
    public static Transform PinboardBound1;

    new void Start()
    {
        base.Start();

        PinboardBound0 = GameObject.Find("PinboardBound0").transform;
        PinboardBound1 = GameObject.Find("PinboardBound1").transform;
    }

    new void Update()
    {
        base.Update();

        if (InPinboardBounds())
        {
            if (G.Mouse.holding == this)
            {
                
            }
        }
    }

    public bool InPinboardBounds()
    {
        return transform.position.x > PinboardBound0.position.x && transform.position.x < PinboardBound1.position.x &&
            transform.position.y > PinboardBound0.position.y && transform.position.y < PinboardBound1.position.y;
    }
}