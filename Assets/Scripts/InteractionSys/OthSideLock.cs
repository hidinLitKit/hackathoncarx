using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OthSideLock : Interactable
{
    public string message;
    public GameObject TrigDial;
    public GameObject othSideLock;
    public override void Interact()
    {
        TrigDial.GetComponent<ItemRequireDialogue>().ShowTips();
        gameObject.SetActive(false);
        othSideLock.SetActive(false);
    }

    public override string GetDescription()
    {
        return message;
    }
}   

