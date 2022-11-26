using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MetalDrawerOp : Interactable
{
    private AudioSource metalOp;
    private bool isOpen = false;
    public Vector3 origin;
    public Vector3 movePoint;
    public float time;

    private void Start()
    {
        metalOp = gameObject.GetComponent<AudioSource>();
        origin = gameObject.transform.localPosition;
        
    }
    public override void Interact()
    {
        if (isOpen==false)
        {
            gameObject.transform.DOLocalMove( movePoint, time, false);
            isOpen = !isOpen;
            metalOp.Play();
        }
        else
        {
            gameObject.transform.DOLocalMove( origin, time, false);
            isOpen = !isOpen;
            metalOp.Play();
        }
    }

    public override string GetDescription()
    {
        if (isOpen==false) return "open";
        else return "close";
    }
}
