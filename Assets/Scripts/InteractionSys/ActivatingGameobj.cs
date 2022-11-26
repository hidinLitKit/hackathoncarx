using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatingGameobj : Interactable
{
    public GameObject gameobj;
    bool isEventHappened = false;
    public override void Interact() {
        if (isEventHappened==false)
        {
            gameobj.SetActive(true);
            isEventHappened  = true;
        }
        
    }



    public override string GetDescription() {
        return null;      
    }
    
}
