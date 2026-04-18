using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueContextPrefab;
    public Dictionary<string, string[]> targetLanguage;
    public Dictionary<string, string[]> fallbackLanguage;

    public DialogueContext StartDialogue(string id, Vector3 contextPosition)
    {
        var context = GameObject.Instantiate(
            dialogueContextPrefab,
            contextPosition,
            Quaternion.identity)
        .GetComponent<DialogueContext>();

        string[] dialogue = {};

        if (targetLanguage != null && targetLanguage.TryGetValue(id, out var value))
        {
            dialogue = value;
        }
        else if (fallbackLanguage != null && fallbackLanguage.TryGetValue(id, out value))
        {
            dialogue = value;
        }
        else
        {
            Debug.LogError($"Missing dialogue ID in both target and fallback languages: {id}");
        }

        context.StartCoroutine(context.DialogueCoroutine(dialogue));

        return context;
    }

    public DialogueContext StartDialogueQuickly(string[] dialogue, Vector3 contextPosition, bool controlledByInput = true)
    {
        var context = GameObject.Instantiate(
            dialogueContextPrefab,
            contextPosition,
            Quaternion.identity)
        .GetComponent<DialogueContext>();

        context.controlledByInput = controlledByInput;

        context.StartCoroutine(context.DialogueCoroutine(dialogue));

        return context;
    }

    public IEnumerator LoadTargetLanguage(string languageCode)
    {
        yield return StartCoroutine(LoadLanguage(languageCode, (lang) => targetLanguage = lang));
    }

    public IEnumerator LoadFallbackLanguage(string languageCode)
    {
        yield return StartCoroutine(LoadLanguage(languageCode, (lang) => fallbackLanguage = lang));
    }

    private IEnumerator LoadLanguage(string languageCode, System.Action<Dictionary<string, string[]>> onLoaded)
    {
        // Path must be relative to a Resources folder and without file extension
        string path = Path.Combine("lang", $"lang_{languageCode}");

        TextAsset textAsset = Resources.Load<TextAsset>(path);
        if (textAsset == null)
        {
            Debug.LogError($"Localization file not found at Resources/{path}");
            onLoaded?.Invoke(new());
            yield break;
        }

        string json = textAsset.text;

        Dictionary<string, string[]> result = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(json);

        if (result == null || result.Count == 0)
        {
            Debug.LogWarning($"Localization file is empty or malformed: {languageCode}");
            onLoaded?.Invoke(new());
            yield break;
        }

        onLoaded?.Invoke(result);
    }
}