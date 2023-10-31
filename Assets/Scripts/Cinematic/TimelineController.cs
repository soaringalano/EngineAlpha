using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    [SerializeField]
    public PlayableDirector director;

    private bool finishedPlaying = false;

    private void Awake()
    {
        //director = GetComponent<PlayableDirector>();
        director.played += DirectorPlayed;
        director.stopped += DirectorStopped;
    }

    private void DirectorStopped(PlayableDirector obj)
    {
        finishedPlaying = false;
    }

    private void DirectorPlayed(PlayableDirector obj)
    {
        finishedPlaying = true;
    }

    public void StartTimeline()
    {
        director.Play();
    }

    public void StopTimeline()
    {
        director.Stop();
    }

    public bool IsPlaying()
    {
        return finishedPlaying;
    }
}