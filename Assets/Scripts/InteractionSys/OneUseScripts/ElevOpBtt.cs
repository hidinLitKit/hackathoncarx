using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevOpBtt : Interactable
{
    private Collider col;
    public GameObject btt;
    public Animator anim;
    public bool isDoorClosed;
    void Awake()
    {
        col = btt.GetComponent<Collider>();
    }
    public override void Interact()
    {
        if (isDoorClosed)
        {
            anim.Play("opendoor");
            StartCoroutine(waiting());
            isDoorClosed=!isDoorClosed;
        }
        else
        {
            anim.Play("closedoor");
            StartCoroutine(waiting());
            isDoorClosed=!isDoorClosed;
        }
    }
    IEnumerator waiting()
    {
        col.enabled = false;
        yield return new WaitForSeconds(3f);
        col.enabled = true;

    }

    public override string GetDescription()
    {
        if (isDoorClosed) return "Press to open the Elevator";
        else return "Press to close the Elevator";
    }
}
