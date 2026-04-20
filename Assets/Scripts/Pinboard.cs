using UnityEngine;

public class Pinboard : MonoBehaviour, IOnFirstSceneStartListener
{
    public Transform Bound0;
    public Transform Bound1;
    public Transform PinPoint;
    public Pinable Pinable;

    public void OnFirstSceneStart()
    {
        Bound0 = GameObject.Find("PinboardBound0").transform;
        Bound1 = GameObject.Find("PinboardBound1").transform;
        PinPoint = GameObject.Find("PinboardPinPoint").transform;
    }
}