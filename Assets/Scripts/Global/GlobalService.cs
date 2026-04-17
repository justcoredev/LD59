using UnityEngine;

public class GlobalService : MonoBehaviour, IOnCleanupListener, IOnFirstSceneStartListener
{
    public virtual void OnCleanup() {}
    public virtual void OnFirstSceneStart() {}
}