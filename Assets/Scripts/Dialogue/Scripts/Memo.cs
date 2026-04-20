using DG.Tweening;
using TMPro;
using UnityEngine;

public class Memo : MonoBehaviour
{
    public TMP_Text tmp;
    public string id;
    Tween tween;

    private void Awake()
    {
        var canvas = GetComponentInChildren<Canvas>();
        canvas.sortingLayerName = "UI";
        canvas.worldCamera = Camera.main;
    }

    public static Memo FindByID(string id)
    {
        Memo[] ms = FindObjectsByType<Memo>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var m in ms)
            if (m.id == id)
                return m;
        return null;
    }

    public static void Show(string id, float duration = -1, string text = "")
    {
        Memo m = FindByID(id);
        if (text != "") m.tmp.text = text;
        m.gameObject.SetActive(true);
        if (duration == -1) return;
        m.tween?.Kill();
        m.tween = DOVirtual.DelayedCall(duration, () => m.gameObject.SetActive(false));
    }

    public static void Hide(string id)
    {
        Memo m = FindByID(id);
        m.gameObject.SetActive(false);
    }
}