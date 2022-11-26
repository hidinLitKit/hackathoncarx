using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockManager : Interactable
{
    public bool isLocked;
    private GameObject hand;
    public GameObject Door;
    private string KeyN;
    public GameObject Key;
    void Awake()
    {
        KeyN = Key.name;
        hand = GameObject.Find("Hand");
    }
    private void Start()
    {
        UpdateDoor();
    }
    void UpdateDoor()
    {
      Door.SetActive(isLocked);
    }
    public override string GetDescription() {
        if (isLocked) return "You need to find Key";
        return null;
    }

    public override void Interact() {
        if (Key.transform.parent & Key.transform.parent.name == "Hand"){
            if (hand.transform.Find(KeyN).gameObject.activeSelf){
                isLocked = !isLocked;
                UpdateDoor();
            }
        }
}
}