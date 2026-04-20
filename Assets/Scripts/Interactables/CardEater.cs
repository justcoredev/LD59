using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardEater : MouseInteractable
{
    public List<string[]> requirements = new();

    public override void MouseHoverAll()
    {
        if (G.Mouse.holding != null)
        {
            // some hover effect like shake
        }
    }

    public override void MouseUpAll()
    {
        if (G.Mouse.holding != null && G.Mouse.holding is Card card)
        {
            CheckRequirements(card);
            // EAT THE CARD
            //Destroy(card.gameObject);
        }
    }

    public void AddCardRequirement(string q1, string q2, string q3, string q4) // format: q = "000111" 6 digits (one quarter)
    {
        string[] req = new string[4];
        req[0] = q1;
        req[1] = q2;
        req[2] = q3;
        req[3] = q4;
        requirements.Add(req);
    }

    void ClearRequirements()
    {
        requirements = new();
    }

    void CheckRequirements(Card card)
    {
        string[] bits = CardToBits(card);

        foreach(string[] req in requirements)
        {
            Debug.Log("BITS:\n" + string.Join("\n", bits));
            Debug.Log("REQ:\n" + string.Join("\n", req));

            if (bits.SequenceEqual(req))
            {
                requirements.Remove(req);

                if (requirements.Count == 0)
                {
                    Debug.Log("ALL REQ MET - PASSED");
                    ClearRequirements();
                }

                return;
            }
        }

        Debug.Log("CANT FIND THIS IN REQ - UR WRONG");
    }

    public string[] CardToBits(Card card)
    {
        string[] bits = new string[4]
        {
            "",
            "",
            "",
            ""
        };

        foreach(Hole hole in card.holes)
        {
            bits[hole.quarterPos.x] += hole.value ? '1' : '0';
        }

        return bits;
    }
}