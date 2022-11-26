using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DrawerRotate : Interactable
{
    private AudioSource drawerOpAudio;
    private bool isOpen = false;
    public Vector3 origin;
    public Vector3 movePoint;

    private void Start()
    {
        drawerOpAudio = GameObject.Find("DrawerOpRt").GetComponent<AudioSource>();
        origin = gameObject.transform.localPosition;
        
    }
    public override void Interact()
    {
        if (isOpen==false)
        {
            gameObject.transform.DOLocalRotate( movePoint, 1f);
            isOpen = !isOpen;
            drawerOpAudio.Play();
        }
        else
        {
            gameObject.transform.DOLocalRotate( origin, 1f);
            isOpen = !isOpen;
            drawerOpAudio.Play();
        }
    }

    public override string GetDescription()
    {
        if (isOpen==false) return "open";
        else return "close";
    }
}

