using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPEffects.CharacterData;
using TMPEffects.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueContext : MonoBehaviour
{
    public event System.Action onComplete;
    public bool IsCompleted { get; private set; }
    public int CurrentCharacterIndex => _writer.CurrentIndex;
    public int CurrentDialogueLine { get; private set; }
    public bool controlledByInput;
    public bool showMemo;
    public AudioClip voiceClip;
    public TMP_Text memoText;
    public Image memoBack;
    public Image emotionImage;

    private TMP_Text _text;
    private TMPWriter _writer;
    private TMPAnimator _animator;
    private Canvas _canvas;

    private bool continueKeyDown;
    private bool writerFinish;

    private Dictionary<Vector2Int, (string, string)> parsedCustomTags = new();

    private float memoBackStartAlpha;

    private void Awake()
    {
        _writer = GetComponentInChildren<TMPWriter>();
        _animator = _writer.GetComponent<TMPAnimator>();
        _text = _writer.GetComponent<TMP_Text>();

        _writer.OnFinishWriter.AddListener((w) => writerFinish = true);
        _writer.OnCharacterShown.AddListener((writer, charData) => OnCharacterShownDefault(writer, charData));

        _canvas = GetComponentInChildren<Canvas>();
        _canvas.sortingLayerName = "UI";

        memoBackStartAlpha = 0.8156863f;
    }

    private Tween _memoTween;

    private void Update()
    {
        //continueKeyDown = Input.GetKeyDown(KeyCode.E);
        continueKeyDown = Input.GetMouseButtonDown(0);

        // Memo text visibility

        if (!controlledByInput || !showMemo)
        {
            memoText.color = new Color(memoText.color.r, memoText.color.g, memoText.color.b, 0);
            memoBack.color = new Color(memoBack.color.r, memoBack.color.g, memoBack.color.b, 0);
        }

        if(memoText != null)
        {
            if (_writer.IsWriting)
            {
                memoText.color = new Color(memoText.color.r, memoText.color.g, memoText.color.b, 0);
                memoBack.color = new Color(memoBack.color.r, memoBack.color.g, memoBack.color.b, 0);

                if (_memoTween != null)
                {
                    _memoTween.Kill();
                    _memoTween = null;
                }
            }
            else
            {
                if (_memoTween == null)
                {
                    _memoTween = DOVirtual.DelayedCall(5f, () =>
                    {
                        memoText.color = new Color(memoText.color.r, memoText.color.g, memoText.color.b, 1);
                        memoBack.color = new Color(memoBack.color.r, memoBack.color.g, memoBack.color.b, memoBackStartAlpha);
                    });
                }
            }
        }
    }

    string[] processedDialogue;

    public IEnumerator DialogueCoroutine(string[] dialogue)
    {
        processedDialogue = ProcessRawDialogueText(dialogue);
        CurrentDialogueLine = 0;

        OnStartDialogue();

        // For each text block in the dialogue
        foreach (string text in processedDialogue)
        {
            OnStartBlock();

            writerFinish = false; // Reset the flag of writer finishing writing the current text block. The flag becomes true when _writer.OnFinishWriter is invoked
            _text.text = text; // Update the UI text 
            _writer.StartWriter(); // Start showing letters of the UI text one by one (typewriter fashion)

            // If the writer didn't finish writing the current text block

            while (!writerFinish)
            {
                // If the writer is not writing
                if (!_writer.IsWriting)
                {
                    // Together two previous conditions form a case where the writer didn't hit the end
                    // But it stopped writing. Therefore, just paused.
                    yield return new WaitUntil(() => continueKeyDown); // Wait for user continuing dialogue
                    _writer.StartWriter(); // Resume writing
                }

                yield return null;
            }

            // Else, if the writer finished writing the current text block

            OnCompleteBlock();

            if(controlledByInput)
                yield return new WaitUntil(() => continueKeyDown); // Wait for user continuing dialogue

            CurrentDialogueLine++;
        }

        // No more text blocks in the current dialogue. Invoke the compliting action

        onComplete?.Invoke();
        IsCompleted = true;

        OnCompleteDialogue();

        Destroy(gameObject);

        yield return null;
    }

    public string[] ProcessRawDialogueText(string[] rawDialogue)
    {
        // TODO: move processing to lang loading

        // Replace [wait=1.0] -> <!wait=1.0>
        for (int i = 0; i < rawDialogue.Length; i++)
        {
            rawDialogue[i] = Regex.Replace(rawDialogue[i], @"\[wait\s*=\s*([0-9]*\.?[0-9]+)\]", "<!wait=$1>");
        }

        // Replace [delay=1.0] -> <!delay=1.0>
        for (int i = 0; i < rawDialogue.Length; i++)
        {
            rawDialogue[i] = Regex.Replace(rawDialogue[i], @"\[delay\s*=\s*([0-9]*\.?[0-9]+)\]", "<!delay=$1>");
        }

        // Replace [pause] -> <!pause>
        for (int i = 0; i < rawDialogue.Length; i++)
        {
            rawDialogue[i] = Regex.Replace(rawDialogue[i], @"\[pause\]", "<!pause>");
        }

        // Process custom tags (those that are not implemented via Luca3317/TMPEffects)

        parsedCustomTags.Clear();

        string[] rawDialogueNoTMPETags = (string[])rawDialogue.Clone();

        for (int i = 0; i < rawDialogueNoTMPETags.Length; i++)
            rawDialogueNoTMPETags[i] = Regex.Replace(rawDialogueNoTMPETags[i], @"<\!.*?>", "");

        for (int i = 0; i < rawDialogueNoTMPETags.Length; i++)
        {
            var pattern = new Regex(@"\[([^=\]\s]+)\s*=\s*([^\]]+)\]");

            while (true)
            {
                var match = pattern.Match(rawDialogueNoTMPETags[i]);
                if (!match.Success) break;

                string key = match.Groups[1].Value;
                string value = match.Groups[2].Value;

                parsedCustomTags.Add(new Vector2Int(i, match.Index), (key, value));

                rawDialogueNoTMPETags[i] = rawDialogueNoTMPETags[i].Remove(match.Index, match.Length);
            }

            rawDialogue[i] = Regex.Replace(rawDialogue[i], @"\[(.*?)\]", "");
        }

        return rawDialogue;
    }

    public virtual void OnCharacterShownDefault(TMPWriter writer, CharData charData)
    {
        // -- Implement custom tags here -- //

        if(parsedCustomTags.TryGetValue(new Vector2Int(CurrentDialogueLine, CurrentCharacterIndex), out (string tag, string value) pair))
        {
            //switch (pair.tag)
            //{
            //    case "emo":
            //        emotionImage.sprite = G.Dialogue.dialogueAssetsDatabase.emotions[pair.value];
            //        break;
            //}
        }

        if(CurrentCharacterIndex % 4 == 0)
        {
            if (processedDialogue[CurrentDialogueLine][0] == '<') // Starts with <color=red> -> deep voice
            {
                //G.Audio.PlayOneShot(G.Audio.Events.Talk);
            }
            else // Usual voice
            {
                //G.Audio.PlayOneShot(G.Audio.Events.Talk2);
            }
        }

        OnCharacterShown(writer, charData);
    }

    // -- Virtuals -- //

    // You can create a derived class from DialogueContext and override these methods to get some custom
    // behaviour like animations and other cool actions!

    public virtual void OnStartDialogue()
    {

    }

    public virtual void OnCompleteDialogue()
    {
        
    }

    public virtual void OnStartBlock()
    {
        _writer.HideAll();
    }

    public virtual void OnCompleteBlock()
    {
        
    }

    public virtual void OnCharacterShown(TMPWriter writer, CharData charData)
    {
        
    }
}