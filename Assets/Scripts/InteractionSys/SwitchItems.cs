using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SwitchItems : MonoBehaviour
{
    public TextMeshProUGUI ItemDescr;
    public GameObject Inventory;
    public Image image;
    public TextMeshProUGUI Name;
    public int selectedItem;
    // Start is called before the first frame update
    void Start()
    {
        SelectItem();   
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedItem = selectedItem;
        if (Input.GetAxis("Mouse ScrollWheel")>0f)
        {
            if (selectedItem >= transform.childCount-1)
            {
                selectedItem = 0;
            }
            else{
                 selectedItem++; 
            }
     
        }
        if (Input.GetAxis("Mouse ScrollWheel")<0f)
        {
            if (selectedItem <= 0)
            {
                selectedItem = transform.childCount-1;
            }
            else{
                 selectedItem--; 
            }
     
        }
        if (previousSelectedItem!=selectedItem){
            SelectItem();

        }
    }
    void SelectItem(){
        int i = 0;
        foreach(Transform item in transform){
            if (i == selectedItem){
                StopAllCoroutines();
                item.gameObject.SetActive(true);
                Inventory.SetActive(true);
                Name.text = item.gameObject.GetComponent<PickedItemProps>().itemname;
                image.sprite = item.gameObject.GetComponent<PickedItemProps>().img;
                ItemDescr.text = item.gameObject.GetComponent<PickedItemProps>().description;
                Inventory.GetComponent<CanvasGroup>().DOFade(1f,0f );
                //Inventory.GetComponent<CanvasGroup>().alpha = 1;
                StartCoroutine(FadeInventory());
                
            }
            else {
                item.gameObject.SetActive(false);
                //Inventory.SetActive(false);
            }
            i++;

        }
    }

    IEnumerator FadeInventory(){
        yield return new WaitForSeconds(3f);
        Inventory.GetComponent<CanvasGroup>().DOFade(0f,1f );
        yield return new WaitForSeconds(1f);
        Inventory.SetActive(false);
    }
}
