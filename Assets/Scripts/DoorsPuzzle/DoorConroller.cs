using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DoorConroller : Interactable
{
    public GameObject tpPoint;
    public static GameObject blackscreen;
    private static Image img;
    public static GameObject crosshair;
    public static AudioClip doorClip;
    
    public GameObject Player;
    public GameObject PuzzleCount;
    public enum DoorType{
        doorZero, doorMultTwo, doorPlusOne, doorMinusOne, doorPlusThree
    }
    public DoorType Doors;

    private void Awake()
    {
        PuzzleCount = GameObject.Find("DoorPuzzle");
        blackscreen = GameObject.Find("BlackScreen");
        img = blackscreen.GetComponent<Image>();
        crosshair = GameObject.Find("Crosshair");
        doorClip = GameObject.Find("DoorClip").GetComponent<AudioSource>().clip;
        Player = GameObject.Find("FirstPerson-AIO"); 
    }
    public override void Interact()
    {
        ChooseType(Doors);
        StartCoroutine(tp());
    }

    public void ChooseType(DoorType op)
    {
        switch(op)
        {
            case DoorType.doorZero:
            PuzzleCount.GetComponent<PuzzleDoorInteract>().counter *=0;
            break;

            case DoorType.doorMultTwo:
            PuzzleCount.GetComponent<PuzzleDoorInteract>().counter *=2;
            break;

            case DoorType.doorPlusOne:
            PuzzleCount.GetComponent<PuzzleDoorInteract>().counter +=1;
            break;

            case DoorType.doorMinusOne:
            PuzzleCount.GetComponent<PuzzleDoorInteract>().counter -=1;
            break;

            case DoorType.doorPlusThree:
            PuzzleCount.GetComponent<PuzzleDoorInteract>().counter +=3;
            break;
        }
    }

    public override string GetDescription()
    {
        return "enter";
    }

    IEnumerator tp()
    {
        img.DOFade(1f,1f);
        crosshair.SetActive(false);
        AudioManager.instance.ShortSound(doorClip);
        yield return new WaitForSeconds(1f);
        Debug.Log("tp");
        img.DOFade(0f,1f);
        Player.transform.position = tpPoint.transform.position;
        crosshair.SetActive(true);
        


    }
}
