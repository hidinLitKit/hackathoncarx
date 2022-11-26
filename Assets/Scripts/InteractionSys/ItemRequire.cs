using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRequire : Interactable
{
    #region Set items
    private GameObject hand;
    public GameObject KeyItem;
    public GameObject ActionIt;
    
    #endregion

    #region Set Descr
    //public string Tip;
    public string ObjName;
    #endregion

    public enum ItemActionType{
        Tv,
        HideItem,
        Microwave
    }
    public ItemActionType ChooseType;
    //privates
    private GameObject Trash;
    private Transform garbPos;
    private string KeyN; //name of item to attach to hand
    
    private bool DidPlUseItem = false; // did player use item?
    private bool showTip = false; //need to show tip?
    
    private bool complete = false; //completed action?

    
    void Awake()
    {
        Trash = GameObject.Find("Garbage");
        garbPos = Trash.transform;
        KeyN = KeyItem.name;
        hand = GameObject.Find("Hand");
    }

    public override string GetDescription() {
        //if (showTip) return null;
        //else return ObjName;
        return ObjName;

    }

    public override void Interact() {
        if (KeyItem.transform.parent & KeyItem.transform.parent.name == "Hand"){
            if (hand.transform.Find(KeyN).gameObject.activeSelf){
                DisposeOfItem();
                complete = true;
                Debug.Log("Success");
                ItemAction(ChooseType);



                
            }
        }
        //else StartCoroutine(showTipFunc());
        else if (complete==false) gameObject.GetComponent<ItemRequireDialogue>().ShowTips();
    }



    IEnumerator showTipFunc()
    {
        if (DidPlUseItem==false & complete==false)
        {
            showTip = true;
            yield return new WaitForSeconds(3f);
            showTip =false;   
        }
    }




    public void DisposeOfItem()
    {
        KeyItem.transform.SetParent(garbPos);
        KeyItem.transform.localPosition = Vector3.zero;
        KeyItem.transform.localEulerAngles = Vector3.zero;
        KeyItem.gameObject.SetActive(false);
        KeyItem.gameObject.layer = 0; // future* set layer to garbage so no camera can't see it
    }


    public void ItemAction(ItemActionType op)
    {
        switch (op)
        {
            case ItemActionType.Tv:
            ActionIt.GetComponent<TVplayVid>().PlayVid();
            break;


            case ItemActionType.HideItem:
            ActionIt.SetActive(false);
            break;

            case ItemActionType.Microwave:
            ActionIt.GetComponent<Microwave>().StartHeating();
            break;


        }
    }


}
