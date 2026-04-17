using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PauseManager : GlobalService
{
    public bool isPaused {get; private set;}

    public List<ParticleSystem> particlesPaused;

    public void Pause()
    {
        isPaused = true;

        Time.timeScale = 0;
        PauseParticles();
        //FMODUnity.RuntimeManager.GetBus("bus:/SFX").setPaused(true);
        //FMODUnity.RuntimeManager.GetBus("bus:/Music").setPaused(true);
        DOTween.PauseAll();
    }

    public void Unpause()
    {
        isPaused = false;

        Time.timeScale = 1;
        UnpauseParticles();
        //FMODUnity.RuntimeManager.GetBus("bus:/SFX").setPaused(false);
        //FMODUnity.RuntimeManager.GetBus("bus:/Music").setPaused(false);
        DOTween.PlayAll();
    }

    public void PauseParticles()
    {
        particlesPaused = new();

        foreach (var p in FindObjectsByType<ParticleSystem>(FindObjectsSortMode.None))
        {
            if (p.isPlaying)
            {
                particlesPaused.Add(p);
                p.Pause();
            }
        }
    }

    public void UnpauseParticles()
    {
        foreach (var p in particlesPaused)
            p.Play();
        
        particlesPaused.Clear();
    }
}