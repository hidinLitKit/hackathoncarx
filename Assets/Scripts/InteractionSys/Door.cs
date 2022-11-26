using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Door : Interactable
{
    public GameObject tpPoint;
    public static GameObject blackscreen;
    private static Image img;
    public static GameObject crosshair;
    public static AudioClip doorClip;
    private void Awake()
    {
        blackscreen = GameObject.Find("BlackScreen");
        img = blackscreen.GetComponent<Image>();
        crosshair = GameObject.Find("Crosshair");
        doorClip = GameObject.Find("DoorClip").GetComponent<AudioSource>().clip;
    }
    public override void Interact() {
        StartCoroutine(tp());
    }



    public override string GetDescription() {
        return "door";      
    }

    IEnumerator tp()
    {
        img.DOFade(1f,1f);
        crosshair.SetActive(false);
        AudioManager.instance.ShortSound(doorClip);
        yield return new WaitForSeconds(1f);
        Debug.Log("tp");
        img.DOFade(0f,1f);
        GameObject.Find("FirstPerson-AIO").transform.position = tpPoint.transform.position;
        crosshair.SetActive(true);
        


    }


}
