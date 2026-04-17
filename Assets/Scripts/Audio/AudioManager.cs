using DG.Tweening;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AudioManager : GlobalService
{
    public Dictionary<EventReference, EventInstance> loopedSounds = new();
    public FMODEvents Events { get; private set; }

    private Bus master;
    private float masterVolume;

    public override void OnCleanup()
    {
        StopAllEvents();
    }

    private void Awake()
    {
        if (AssertPathField())
            return;

        Events = GetComponent<FMODEvents>();

        master = FMODUnity.RuntimeManager.GetBus("bus:/");

        masterVolume = 1;
        master.setVolume(1);
    }

    private bool AssertPathField()
    {
        FieldInfo pathField = typeof(EventReference).GetField(
            "Path",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
        );

        if (pathField == null)
        {
            UnityEngine.Debug.LogError("FMOD EventReference.Path field not found.");
            return true;
        }

        if (!pathField.IsPublic)
        {
            UnityEngine.Debug.LogError(
                "FMOD EventReference.Path is not public! " +
                "Dear Justcore, your AudioManager relies on Setting it from code, modify the FMOD source."
            );
            return true;
        }

        return false;
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPosition)
    {
        if (!RuntimeManager.HaveAllBanksLoaded) return;

        RuntimeManager.PlayOneShot(sound, worldPosition);
    }

    public void PlayOneShot(EventReference sound)
    {
        if (!RuntimeManager.HaveAllBanksLoaded) return;

        PlayOneShot(sound, Vector3.zero);
    }

    public void PlayLooped(EventReference sound)
    {
        float delay = 0;
        if (!RuntimeManager.HaveAllBanksLoaded)
            delay = 2f; // If the music is not loaded yet, then wait 2 seconds before starting it

        StartCoroutine(DelayedRoutine(() =>
        {
            loopedSounds[sound] = CreateInstance(sound); // Using ToString instead of Path
            loopedSounds[sound].start();
        }, delay));
    }

    public void StopLooped(EventReference sound)
    {
        if (!RuntimeManager.HaveAllBanksLoaded) return;
        if (loopedSounds.ContainsKey(sound)) loopedSounds[sound].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public EventInstance CreateInstance(EventReference eventRef)
    {
        var instance = RuntimeManager.CreateInstance(eventRef);
        return instance;
    }

    public EventReference GetReference(string path)
    {
        // DOESNT WORK I GUESS
        EventReference r = new();
        r.Path = path;
        return r;
    }

    public void StopAllEvents()
    {
        // Stops all events on Master bus
        master.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public Tween DOMasterVolume(float to, float duration)
    {
        return DOTween.To(
            () => masterVolume, 
            (x) =>
            {
                masterVolume = x;
                master.setVolume(x);
            },
            to,
            duration
        );
    }

    public Tween DOBusVolume(string busPath, float to, float duration)
    {
        var bus = FMODUnity.RuntimeManager.GetBus(busPath);
        bus.getVolume(out var v);

        return DOTween.To(
            () => v, 
            (x) =>
            {
                v = x;
                bus.setVolume(x);
            },
            to,
            duration
        );
    }

    IEnumerator DelayedRoutine(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}