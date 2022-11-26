using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class ScreamerConroller : MonoBehaviour
{
    public GameObject scr;
    string GameOver = "GameOver";
    public void ScrEvent()
    {
        scr.SetActive(true);
        scr.GetComponent<VideoPlayer>().Play();
        StartCoroutine(Wait());
        
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(GameOver);
    }
}
