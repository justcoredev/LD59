using TMPro;
using UnityEngine;

public class ButtonSend : MouseInteractable
{
    public TMP_Text tmp;
    public SpriteRenderer litSr;
    public bool Sent;
    public bool Locked;

    void Start()
    {
        Lock(true);
    }

    public override void MouseDown()
    {
        if (!Locked)
        {
            Sent = true;
            Lock(true);
        }
    }

    public void Lock(bool l)
    {
        if (l)
        {
            Locked = true;
            tmp.color = Color.red;
            litSr.color = Color.red;
        }
        else
        {
            Sent = false;
            Locked = false;
            tmp.color = Color.green;
            litSr.color = Color.green;
        }
    }
}