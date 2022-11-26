using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TvScareEvent : MonoBehaviour
{
    public GameObject videoScreen;
    public GameObject DialogueTrigger;
    
    public void xStartEvent()
    {
        StartCoroutine(eventGoOn());
    }
    
    IEnumerator eventGoOn()
    {
        //RenderSettings.fogDensity = 0.15f;
        RenderSettings.fogDensity = 0.3f;
        yield return new WaitForSeconds(3f);
        videoScreen.SetActive(true);
        videoScreen.GetComponent<VideoPlayer>().Play();
        videoScreen.SetActive(false);


        DialogueTrigger.SetActive(true);


    }
}
