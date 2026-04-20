using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CardEater : MouseInteractable
{
    public List<string[]> requirements = new();
    public bool AllRequirementsMet => requirements != null ? (requirements.Count == 0) : false;

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
            Destroy(card.gameObject);
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

        foreach (string[] req in requirements)
        {
            Debug.Log("BITS:\n" + string.Join("\n", bits));
            Debug.Log("REQ:\n" + string.Join("\n", req));

            if (Matches(bits, req))
            {
                requirements.Remove(req);
                Memo.Show("code_result", 4.0f, "<color=#00cc00>Correct code.</color>");

                if (requirements.Count == 0)
                {
                    Debug.Log("ALL REQ MET - PASSED");
                    ClearRequirements();
                    FindAnyObjectByType<ButtonSend>().Lock(false);
                }

                return;
            }
        }

        Memo.Show("code_result", 4.0f, "Wrong code. Try again.");
        DOVirtual.DelayedCall(2.0f, () => G.CardGiver.GiveCards(1));
        Debug.Log("CANT FIND THIS IN REQ - UR WRONG");
    }

    bool Matches(string[] bits, string[] req)
    {
        if (bits.Length != req.Length)
            return false;

        var remaining = new List<string>(req);

        foreach (var b in bits)
        {
            int index = remaining.IndexOf(b);
            if (index == -1)
                return false;

            remaining.RemoveAt(index); // consume match
        }

        return true;
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