using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// fix error
public class AttachingItem : Interactable
{
    Transform slot;
    Transform AttachSlot;
    public GameObject item;
    private string nameItem;
    private bool isAttached;
    void Awake()
    {
        isAttached = false;
        nameItem = item.name;
        slot = GameObject.Find("Hand").GetComponent<Transform>();
        AttachSlot = this.transform.GetChild(0).GetComponent<Transform>();
    }
    public override string GetDescription() {
        if (isAttached == false) return "Press to attach";
        return null;
    }

    public override void Interact() {
        if (item.transform.parent & item.transform.parent.name == "Hand")
        {
            if(slot.transform.Find(nameItem).gameObject.activeSelf)
            {
                AttachItem();
            }  
        }
 
    }
    private void AttachItem(){
        item.transform.SetParent(AttachSlot);
        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = Vector3.zero;
        item.GetComponent<Collider>().enabled = false;
        item.gameObject.layer = 0;
        isAttached = !isAttached;
    }
}
