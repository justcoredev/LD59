using UnityEngine;

public class G
{
    public static GameObject GameStateObject;
    public static bool IsInitialized => GameStateObject != null;

    // System
    public static AudioManager Audio;
    public static PauseManager Pause;
    public static MouseManager Mouse;

    public static void Initialize(GameObject gameStateObject)
    {
        GameStateObject = gameStateObject;
        Audio = GameStateObject.GetComponent<AudioManager>();
        Pause = GameStateObject.GetComponent<PauseManager>();
        Mouse = GameStateObject.GetComponent<MouseManager>();
    }

    public static void OnFirstSceneAwake()
    {
        var mbs = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var mb in mbs)
            if (mb is IFirstSceneAwakeListener listener)
                listener.OnFirstSceneAwake();
    }

    public static void OnFirstSceneStart()
    {
        var mbs = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var mb in mbs)
            if (mb is IOnFirstSceneStartListener listener)
                listener.OnFirstSceneStart();
    }
}