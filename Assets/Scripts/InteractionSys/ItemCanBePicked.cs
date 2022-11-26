using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//maybe enable to drop item
public class ItemCanBePicked : Interactable
{
    Transform slot;
    public GameObject item;
    public  AudioSource PickAudio;
    public string descr;
    
    
    void Awake()
    {
        item = gameObject;
        PickAudio  = GameObject.Find("PickItem").GetComponent<AudioSource>();
        slot = GameObject.Find("Hand").GetComponent<Transform>();
    }
    public override string GetDescription() {
        return descr;
    }

    public override void Interact() {
        PickItem();
        PickAudio.Play();
    
    }
    private void PickItem(){
        item.transform.SetParent(slot);
        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = Vector3.zero;
        item.GetComponent<Collider>().enabled = false;
        item.gameObject.SetActive(false);
        item.gameObject.layer = 7;
    }
}
