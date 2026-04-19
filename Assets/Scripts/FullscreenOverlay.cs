using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FullscreenOverlay : MonoBehaviour, IOnCleanupListener
{
    public GameObject FullscreenOverlayPrefab;
    GameObject FullscreenOverlayObject;

    public Image image;
    public Coroutine fadeRoutine;

    public void OnCleanup()
    {
        Destroy(FullscreenOverlayObject);
    }

    void Awake()
    {
        FullscreenOverlayObject = Instantiate(FullscreenOverlayPrefab);
        image = FullscreenOverlayObject.GetComponentInChildren<Image>();
        DontDestroyOnLoad(FullscreenOverlayObject);
    }

    public void StartFade(bool fadeIn, float duration = 2.0f)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(Fade(fadeIn, duration));
    }

    public IEnumerator Fade(bool fadeIn, float duration = 2.0f)
    {
        image.DOKill();

        float targetAlpha = fadeIn ? 1f : 0f;

        Tween tween = image
            .DOFade(targetAlpha, duration)
            .SetEase(Ease.OutQuad);

        yield return tween.WaitForCompletion();

        fadeRoutine = null;
    }
}