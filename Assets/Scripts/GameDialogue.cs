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
            "Please, take your todays instructions.",
        }},
        {"take_puncher", new[]
        {
            "And of course the hole puncher.",
        }},
        {"task_1", new[]
        {
            "Todays task is to punch the <color=red>pressure</color> data into a punch card and send it.",
            "See you later."
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