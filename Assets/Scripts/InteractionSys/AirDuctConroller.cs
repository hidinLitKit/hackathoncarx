using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDuctConroller : Interactable
{
    private Animator anim;
    public AudioSource ventSound;
    
    private void Awake()
    {
        ventSound = gameObject.GetComponent<AudioSource>();
        anim = gameObject.GetComponent<Animator>();
    }
    public override void Interact()
    {
       anim.SetBool("Open",true);
       Invoke("SetBoolBack",2.4f);
       ventSound.Play();
    }

    private void SetBoolBack()
    {
        anim.SetBool("Open",false);
    }

    public override string GetDescription()
    {
        return "airduct";
    }
}
