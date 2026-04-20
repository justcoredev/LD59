using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Sequences : MonoBehaviour, IOnFirstSceneStartListener
{
    public Coroutine currentSequence;
    public bool completed;
    public bool skip;

    public GameDialogue gameDialogue;
    public MeasureButton measureButton;
    public ButtonSend buttonSend;

    public void OnFirstSceneStart()
    {
        gameDialogue = FindAnyObjectByType<GameDialogue>();
        measureButton = FindAnyObjectByType<MeasureButton>();
        buttonSend = FindAnyObjectByType<ButtonSend>();

        StartCoroutine(FirstSceneRoutine());
    }

    IEnumerator FirstSceneRoutine()
    {
        currentSequence = StartCoroutine(Level1());
        yield return new Justcore.WaitUntilAny(() => completed, () => skip);
        //ApplySequencePoint(SequencePoint.Intro);

        currentSequence = StartCoroutine(Level2());
        yield return new Justcore.WaitUntilAny(() => completed, () => skip);
        //ApplySequencePoint(SequencePoint.Intro);

        currentSequence = StartCoroutine(Level3());
        yield return new Justcore.WaitUntilAny(() => completed, () => skip);
        //ApplySequencePoint(SequencePoint.Intro);

        currentSequence = StartCoroutine(Level4());
        yield return new Justcore.WaitUntilAny(() => completed, () => skip);
        //ApplySequencePoint(SequencePoint.Intro);

        currentSequence = StartCoroutine(Level5());
        yield return new Justcore.WaitUntilAny(() => completed, () => skip);
        //ApplySequencePoint(SequencePoint.Intro);
    }

    IEnumerator Level1()
    {
        completed = false;

        // Pinable manualList = G.ItemsGiver.ManualList.GetComponent<Pinable>();
        // manualList.Pin(); // Pin the manual

        Pinable codeList1 = G.ItemsGiver.CodeList1.GetComponent<Pinable>();

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

        G.ItemsGiver.Give(G.ItemsGiver.CodeList1);
        //yield return new WaitUntil(() => G.Mouse.holding is Pinable);

        yield return new WaitForSeconds(1.0f);
        context = gameDialogue.StartDialogue("pin_paper");
        yield return new WaitUntil(() => context.IsCompleted);
        yield return new WaitForSeconds(1.0f);

        yield return new WaitUntil(() => G.Pinboard.Pinable == codeList1);

        yield return new WaitForSeconds(1.0f);
        context = gameDialogue.StartDialogue("take_puncher");
        yield return new WaitUntil(() => context.IsCompleted);
        yield return new WaitForSeconds(1.0f);

        G.ItemsGiver.Give(G.ItemsGiver.HoleMaker);
        //yield return new WaitUntil(() => G.Mouse.holding is HoleMaker);

        yield return new WaitForSeconds(2.0f);
        context = gameDialogue.StartDialogue("task_1");
        yield return new WaitUntil(() => context.IsCompleted);
        yield return new WaitForSeconds(1.0f);

        // Character hides
        gameDialogue.HideCharacter();

        yield return new WaitForSeconds(1.0f);

        G.CardGiver.GiveCards(1);
        measureButton.Lock(false);

        // "Press the measure button" memo
        Tween tween = DOVirtual.DelayedCall(5.0f, () => Memo.Show("measure_press", -1));
        yield return new WaitUntil(() => measureButton.wasPressedOnce);
        Memo.Hide("measure_press");
        tween?.Kill();

        // "Encode temperature" memo
        DOVirtual.DelayedCall(2.0f, () => Memo.Show("encode_value", -1));
        yield return new WaitUntil(() => FindAnyObjectByType<Card>() != null && FindAnyObjectByType<Card>().punchedCount > 7);
        Memo.Hide("encode_value");

        // "Put to the machine" memo
        Memo.Show("eat_card", -1);
        yield return new WaitUntil(() => G.CardEater.AllRequirementsMet);
        Memo.Hide("eat_card");

        // "Open monitor" memo
        Memo.Show("monitor", -1);
        yield return new WaitUntil(() => FindAnyObjectByType<MonitorMoveButton>().isMoved);
        Memo.Hide("monitor");

        yield return new WaitUntil(() => buttonSend.Sent);

        Debug.Log("You sent the code!!! yay!");

        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(G.FullscreenOverlay.Fade(true, 2.0f));

        completed = true;
    }

    IEnumerator Level2()
    {
        completed = false;

        Sensor.FindByID("pressure").SetDesiredValue(10.2f);
        Sensor.FindByID("temp").SetDesiredValue(32.4f);

        // Pressure
        G.CardEater.AddCardRequirement("000010", "000000", "000001", "100010");

        // Temperature x2
        G.CardEater.AddCardRequirement("101010", "100010", "000001", "110101");
        G.CardEater.AddCardRequirement("101010", "100010", "000001", "110101");

        // Fade
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(G.FullscreenOverlay.Fade(false, 2.0f));
        
        // Character appears
        yield return new WaitForSeconds(1.0f);
        gameDialogue.AppearCharacter(GameDialogue.Character.Worker);

        yield return new WaitForSeconds(1.0f);
        var context = gameDialogue.StartDialogue("hi_now_pressure_too");
        yield return new WaitUntil(() => context.IsCompleted);
        yield return new WaitForSeconds(1.0f);

        G.ItemsGiver.Give(G.ItemsGiver.ListLevel2);

        yield return new WaitForSeconds(1.0f);
        context = gameDialogue.StartDialogue("see_you_later");
        yield return new WaitUntil(() => context.IsCompleted);
        yield return new WaitForSeconds(1.0f);

        // Character hides
        gameDialogue.HideCharacter();

        yield return new WaitForSeconds(1.0f);

        G.CardGiver.GiveCards(3);
        measureButton.Lock(false);

        yield return new WaitUntil(() => buttonSend.Sent);

        Debug.Log("You sent the code!!! yay!");

        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(G.FullscreenOverlay.Fade(true, 2.0f));

        completed = true;
    }

    IEnumerator Level3()
    {
        completed = false;

        Sensor.FindByID("pressure").SetDesiredValue(0.0f);
        Sensor.FindByID("temp").SetDesiredValue(0.0f);

        // Cultist's card
        G.CardEater.AddCardRequirement("101010", "101010", "010101", "010101");

        // Fade
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(G.FullscreenOverlay.Fade(false, 2.0f));
        
        // Character appears
        yield return new WaitForSeconds(1.0f);
        gameDialogue.AppearCharacter(GameDialogue.Character.Cultist);

        yield return new WaitForSeconds(1.0f);
        var context = gameDialogue.StartDialogue("cultist_intro");
        yield return new WaitUntil(() => context.IsCompleted);
        yield return new WaitForSeconds(1.0f);

        G.ItemsGiver.Give(G.ItemsGiver.ListLevel3);

        yield return new WaitForSeconds(1.0f);
        context = gameDialogue.StartDialogue("cultist_stop_them");
        yield return new WaitUntil(() => context.IsCompleted);
        yield return new WaitForSeconds(1.0f);

        // Character hides
        gameDialogue.HideCharacter();

        yield return new WaitForSeconds(1.0f);

        G.CardGiver.GiveCards(1);
        //measureButton.Lock(false);

        yield return new WaitUntil(() => buttonSend.Sent);

        Debug.Log("You sent the code!!! yay!");

        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(G.FullscreenOverlay.Fade(true, 2.0f));

        completed = true;
    }

    IEnumerator Level4()
    {
        completed = false;

        Sensor.FindByID("pressure").SetDesiredValue(0.0f);
        Sensor.FindByID("temp").SetDesiredValue(43.9f);

        // Temperature x4
        G.CardEater.AddCardRequirement("110101", "101010", "000001", "111111");
        G.CardEater.AddCardRequirement("110101", "101010", "000001", "111111");
        G.CardEater.AddCardRequirement("110101", "101010", "000001", "111111");
        G.CardEater.AddCardRequirement("110101", "101010", "000001", "111111");

        // Fade
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(G.FullscreenOverlay.Fade(false, 2.0f));
        
        // Character appears
        yield return new WaitForSeconds(1.0f);
        gameDialogue.AppearCharacter(GameDialogue.Character.Worker);

        yield return new WaitForSeconds(1.0f);
        var context = gameDialogue.StartDialogue("house");
        yield return new WaitUntil(() => context.IsCompleted);
        yield return new WaitForSeconds(1.0f);

        G.ItemsGiver.Give(G.ItemsGiver.ListLevel4);

        yield return new WaitForSeconds(1.0f);
        context = gameDialogue.StartDialogue("see_you_later");
        yield return new WaitUntil(() => context.IsCompleted);
        yield return new WaitForSeconds(1.0f);

        // Character hides
        gameDialogue.HideCharacter();

        yield return new WaitForSeconds(1.0f);

        G.CardGiver.GiveCards(4);
        measureButton.Lock(false);

        yield return new WaitUntil(() => buttonSend.Sent);

        Debug.Log("You sent the code!!! yay!");

        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(G.FullscreenOverlay.Fade(true, 2.0f));

        completed = true;
    }

    IEnumerator Level5()
    {
        completed = false;

        Sensor.FindByID("pressure").SetDesiredValue(0.0f);
        Sensor.FindByID("temp").SetDesiredValue(79.9f);

        // Temperature
        G.CardEater.AddCardRequirement("010101", "000001", "111111", "111111");

        // Fade
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(G.FullscreenOverlay.Fade(false, 2.0f));

        yield return new WaitForSeconds(1.0f);

        G.CardGiver.GiveCards(1);
        measureButton.Lock(false);

        yield return new WaitUntil(() => FindAnyObjectByType<Card>() != null && FindAnyObjectByType<Card>().punchedCount >= 9);

        Debug.Log("ENDING STARTS");

        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(G.FullscreenOverlay.Fade(true, 2.0f));

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