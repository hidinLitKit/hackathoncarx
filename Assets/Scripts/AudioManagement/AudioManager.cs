using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource track01,track02,track03;
    private bool isPlayingTrack01;
    
    public static AudioManager instance;

    private void Awake()
    {
        if (instance==null)
        instance = this;
    }

    private void Start()
    {
        //track01 = gameObject.AddComponent<AudioSource>();
        //track02 = gameObject.AddComponent<AudioSource>();
        isPlayingTrack01 = true;
    }
    public void SwapTracks(AudioClip newClip)
    {
        StopAllCoroutines();
        StartCoroutine(FadeTrack(newClip));
        isPlayingTrack01 = !isPlayingTrack01;

    }


    private IEnumerator FadeTrack(AudioClip newClip)
    {
        float timeToFade = 0.5f;
        float timeElapsed = 0;
        if (isPlayingTrack01)
        {
            track02.clip = newClip;
            track02.Play();
            while (timeElapsed <timeToFade)
            {
                track02.volume = Mathf.Lerp(0,0.5f,timeElapsed/timeToFade);
                track01.volume = Mathf.Lerp(0,0.5f,timeElapsed/timeToFade);
                timeElapsed +=Time.deltaTime;
                yield return null;
            }
            track01.Stop();
        }
        else
        {
            track01.clip = newClip;
            track01.Play();
            while (timeElapsed <timeToFade)
            {
                track01.volume = Mathf.Lerp(0,0.5f,timeElapsed/timeToFade);
                track02.volume = Mathf.Lerp(0.5f,0,timeElapsed/timeToFade);
                timeElapsed +=Time.deltaTime;
                yield return null;
            }
            track02.Stop();
        }

    }
    public void ShortSound(AudioClip newClip)
    {
        track03.clip = newClip;
        track03.Play();
    }
}
