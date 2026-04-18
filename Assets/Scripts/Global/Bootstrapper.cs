using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    public const string bootstrapperSceneName = "Bootstrapper";
    public const int bootstrapperSceneIndex = 0;

    public const string splashScreenSceneName = "SplashScreen";

    public static int queuedSceneIndex;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void BeforeFirstSceneLoad()
    {
        ChoosePrimaryMonitor();

        var activeScene = SceneManager.GetActiveScene();

        // By default we will go to the scene that is right after the bootstrapper
        queuedSceneIndex = activeScene.buildIndex + 1;

        // Else, if the first scene we loaded is not the bootstrapper, redirect
        if (activeScene.name != bootstrapperSceneName)
        {
            queuedSceneIndex = activeScene.buildIndex;
            SceneManager.LoadScene(bootstrapperSceneName);
        }
    }

    public static void RestartGame(bool finishedGame = false)
    {
        if (G.Pause.isPaused)
            G.Pause.Unpause();

        // Go to the bootstrapper right away
        queuedSceneIndex = bootstrapperSceneIndex + 1;
        SceneManager.LoadScene(bootstrapperSceneName);
    }

    private IEnumerator Start()
    {
        Debug.Log("<color=green>Entry point hit!</color>");

        // Cleanup
        Cleanup();

        // Create the Game State, Initialize G
        GameObject gameStatePrefab = Resources.Load<GameObject>("GameState");
        if (gameStatePrefab == null)
        {
            Debug.LogError("Couldn't find the GameState prefab in \"Resources/\"");
            yield break;
        }
            
        GameObject gameStateObject = Instantiate(gameStatePrefab);
        DontDestroyOnLoad(gameStateObject);
        G.Initialize(gameStateObject);

        // Load all shit that takes time to load
        yield return StartCoroutine(LoadAudio());

        // Proceed to the queued scene
        SceneManager.LoadScene(queuedSceneIndex, LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == bootstrapperSceneName || scene.name == splashScreenSceneName)
            return;

        // - Game specific inits
        //FindAnyObjectByType<PlayerBase>().Init();
        // --

        G.OnFirstSceneAwake();
        G.OnFirstSceneStart();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Cleanup()
    {
        foreach(AudioManager oldGameState in FindObjectsByType<AudioManager>(FindObjectsSortMode.None))
        {
            foreach(var cleanupListener in oldGameState.gameObject.GetComponents<IOnCleanupListener>())
                cleanupListener.OnCleanup();
            
            Destroy(oldGameState.gameObject);
        }
    }

    private IEnumerator LoadAudio()
    {
        while (!FMODUnity.RuntimeManager.HaveAllBanksLoaded)
            yield return null;
    }

    private static void ChoosePrimaryMonitor()
    {
#if UNITY_STANDALONE_WIN
        List<DisplayInfo> displayInfos = new();
        Screen.GetDisplayLayout(displayInfos);
        // 0 is always primary according to this post:
        // https://discussions.unity.com/t/how-to-get-game-build-to-run-on-main-display/879642/27
        var displayInfo = displayInfos[0];
        Screen.MoveMainWindowTo(displayInfo, Vector2Int.zero);
#endif
    }
}