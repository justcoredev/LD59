using System.Collections.Generic;
using UnityEngine;

public class GameDialogue : MonoBehaviour
{
    public Dictionary<string, string[]> dialogues = new()
    {
        {"test", new[]{"This is a test lmao."}}
    };

    public DialogueContext StartDialogue(string id)
    {
        return G.Dialogue.StartDialogueQuickly(dialogues[id], transform.position);
    }
}