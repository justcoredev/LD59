using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameDialogue : MonoBehaviour
{
    public SpriteRenderer charRenderer;

    public Sprite charCultistSprite;
    public Sprite charLabSprite;

    public enum Character { Cultist, Worker }
    public Character currentChar;

    Vector3 charShowPos;
    Vector3 charHidePos;
    Sequence seq;

    public Dictionary<string, string[]> dialogues = new()
    {
        // NOTE: We must keep it short and meaningful like in 
        // https://dmullinsgames.itch.io/sacrifices-must-be-made
        {"take_paper_1", new[]
        {
            "Good morning.",
            "Please, take your daily instructions.",
            "We changed them for safety.",
        }},
        {"pin_paper", new[]
        {
            "Pin it to your board.",
        }},
        {"take_puncher", new[]
        {
            "And of course the hole-puncher.",
        }},
        {"task_1", new[]
        {
            "Punch the <color=red>temperature</color> data into a card and send it.",
            "Good luck. [pause]See you later."
        }},
        {"hi_now_pressure_too", new[]
        { // TODO polish: "i got you a present (pot with a sprout)" or smth wholesome like that.
            "Good morning again.",
            "Today they want you to send a card with <color=#0000ee>pressure</color> along with two cards of <color=red>temperature</color>.",

        }},
        {"see_you_later", new[]
        {
            "See you later.",
        }},
        {"cultist_intro", new[]
        {
            "Greetings to you.",
            "Something terrible is in making.",
            "You shouldn't provide them information.",
            "A wax candle melted behind you. Doesn't look like it was ever lit."
        }},
        {"cultist_stop_them", new[]
        {
            "They must be stopped."
        }},
        {"house", new[]
        {
            "My house burned down.",
            "But it's fine."
        }},
    };

    public DialogueContext StartDialogue(string id)
    {
        return G.Dialogue.StartDialogueQuickly(dialogues[id], transform.position);
    }

    void Start()
    {
        charShowPos = charRenderer.transform.position;
        charHidePos = charShowPos + Vector3.down * 1.0f;

        HideCharacter(0.0f);
    }

    void Update()
    {
        charRenderer.transform.localScale = new Vector3(
            charRenderer.transform.localScale.x,
            1.0f + Mathf.Sin(Time.time * 0.5f) * 0.005f,
            charRenderer.transform.localScale.z
        );
    }

    public void AppearCharacter(Character character, float duration = 2f)
    {
        SetCharacter(character);

        charRenderer.transform.position = charHidePos;
        charRenderer.color = new Color(1, 1, 1, 0);

        seq?.Kill();
        seq = DOTween.Sequence();
        seq.Join(charRenderer.transform.DOMoveY(charShowPos.y, duration).SetEase(Ease.OutQuart));
        seq.Join(charRenderer.DOColor(Color.white, duration).SetEase(Ease.OutQuart));
    }

    public void HideCharacter(float duration = 2f)
    {
        charRenderer.transform.position = charShowPos;
        charRenderer.color = Color.white;

        seq?.Kill();
        seq = DOTween.Sequence();
        seq.Join(charRenderer.transform.DOMoveY(charHidePos.y, duration).SetEase(Ease.OutQuart));
        seq.Join(charRenderer.DOColor(new Color(1, 1, 1, 0), duration).SetEase(Ease.OutQuart));
    }

    public void SetCharacter(Character character)
    {
        switch (character)
        {
            case Character.Cultist: charRenderer.sprite = charCultistSprite; break;
            case Character.Worker: charRenderer.sprite = charLabSprite; break;
        }

        currentChar = character;
    }
}