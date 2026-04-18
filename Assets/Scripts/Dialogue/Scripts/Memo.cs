using UnityEngine;

public class Memo : MonoBehaviour
{
    private void Awake()
    {
        var canvas = GetComponentInChildren<Canvas>();
        canvas.sortingLayerName = "UI";
        canvas.worldCamera = Camera.main;
    }

    public void StartFadingAway()
    {

    }
}