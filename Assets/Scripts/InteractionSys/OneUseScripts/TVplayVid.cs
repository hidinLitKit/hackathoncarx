using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class TVplayVid : MonoBehaviour
{
    public GameObject TVplayer;
    public float VideoTime;
    public bool isExtraAction;
    
    public void PlayVid()
    {
        StartCoroutine(WatchingVid());
        if (isExtraAction)
        {
            gameObject.GetComponent<TvScareEvent>().xStartEvent();
        }
    }
    
    IEnumerator WatchingVid()
    {
        TVplayer.SetActive(true);
        TVplayer.GetComponent<VideoPlayer>().Play();
        yield return new WaitForSeconds(VideoTime);
        TVplayer.SetActive(false);


    }

    
}
