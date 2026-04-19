using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Sequences : MonoBehaviour, IOnFirstSceneStartListener
{
    public Coroutine currentSequence;
    public bool completed;
    public bool skip;

    public GameDialogue gameDialogue;

    public void OnFirstSceneStart()
    {
        gameDialogue = FindAnyObjectByType<GameDialogue>();

        StartCoroutine(FirstSceneRoutine());
    }

    IEnumerator FirstSceneRoutine()
    {
        // Menu
        currentSequence = StartCoroutine(Level1());
        yield return new Justcore.WaitUntilAny(() => completed, () => skip);
        //ApplySequencePoint(SequencePoint.Intro);
    }

    IEnumerator Level1()
    {
        completed = false;

        G.FullscreenOverlay.Fade(true, 0);
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(G.FullscreenOverlay.Fade(false, 2.0f));

        gameDialogue.StartDialogue("test");
        yield return new WaitForSeconds(2.0f);
        G.CardGiver.GiveCards(3);

        completed = true;
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            skip = true;
        }
    }
#endif
}