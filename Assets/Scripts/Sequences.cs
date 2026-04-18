using DG.Tweening;
using UnityEngine;

public class Sequences : MonoBehaviour, IOnFirstSceneStartListener
{
    public Transform dialogueContextPoint;

    public void OnFirstSceneStart()
    {
        dialogueContextPoint = GameObject.Find("DialogueContextPoint").transform;

        G.Dialogue.StartDialogueQuickly(new[]{"Hello! This is a test dialogue!"}, dialogueContextPoint.position);
        DOVirtual.DelayedCall(2.0f, () => G.CardGiver.GiveCards(3));
    }
}