using System.Collections;
using UnityEngine;

public class Sequences : MonoBehaviour, IOnFirstSceneStartListener
{
    public Coroutine currentSequence;
    public bool completed;
    public bool skip;

    public GameDialogue gameDialogue;
    public MeasureButton measureButton;

    public void OnFirstSceneStart()
    {
        gameDialogue = FindAnyObjectByType<GameDialogue>();
        measureButton = FindAnyObjectByType<MeasureButton>();

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

        Sensor.FindByID("temp").SetDesiredValue(28.5f);
        G.CardEater.AddCardRequirement("100010", "011101", "000001", "010000");

        // Fade
        G.FullscreenOverlay.Fade(true, 0);
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(G.FullscreenOverlay.Fade(false, 2.0f));
        
        // Character appears
        yield return new WaitForSeconds(1.0f);
        gameDialogue.AppearCharacter(GameDialogue.Character.Worker);

        yield return new WaitForSeconds(1.0f);
        var context = gameDialogue.StartDialogue("take_paper_1");
        yield return new WaitUntil(() => context.IsCompleted);

        yield return new WaitForSeconds(1.0f);
        G.ItemsGiver.Give(G.ItemsGiver.ManualList);
        yield return new WaitForSeconds(0.5f);
        G.ItemsGiver.Give(G.ItemsGiver.CodeList1);

        yield return new WaitUntil(() => G.Pinboard.Pinable != null);

        yield return new WaitForSeconds(1.0f);
        context = gameDialogue.StartDialogue("take_puncher");
        yield return new WaitUntil(() => context.IsCompleted);
        yield return new WaitForSeconds(1.0f);

        G.ItemsGiver.Give(G.ItemsGiver.HoleMaker);
        yield return new WaitUntil(() => G.Mouse.holding is HoleMaker);

        yield return new WaitForSeconds(2.0f);
        context = gameDialogue.StartDialogue("task_1");
        yield return new WaitUntil(() => context.IsCompleted);
        yield return new WaitForSeconds(1.0f);

        // Character hides
        gameDialogue.HideCharacter();

        yield return new WaitForSeconds(2.0f);
        G.CardGiver.GiveCards(1);
        measureButton.Lock(false);

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