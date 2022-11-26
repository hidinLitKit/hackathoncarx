using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemRequireDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI charName;
    public string[] lines;
    public string[] lines2;
    public float textSpeed = 0.1f;
    public GameObject DialogueUI;
    public GameObject NPC;
    private GameObject Player;
    private int index;
    private int indexForname;
    private bool IsDialogueAct = false;
    void Awake()
    {
        Player = GameObject.Find("FirstPerson-AIO");

    }
    public void ShowTips()
    {  // Starting dialogue - disabling player movement 
        DialogueUI.SetActive(true);
        IsDialogueAct = true;
        textComponent.text = string.Empty;
        StartDialogue(); 
        Player.GetComponent<FirstPersonAIO>().playerCanMove = false;
        //NPC.GetComponent<Collider>().enabled = !NPC.GetComponent<Collider>().enabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDialogueAct)
        {
            if(Input.GetMouseButtonDown(0)){
            if (textComponent.text == lines[index]){
                NextLine();
                NextName();
            }
            else{
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
        }
        
    }

    void StartDialogue(){
        index =0;
        indexForname = 0;
        StartCoroutine(TypeLine());
        charName.text = lines2[indexForname];
    }

    IEnumerator TypeLine(){
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text +=c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    void NextLine(){
        if (index < lines.Length-1){
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        //End of dialogue - enabling everything
        else{
            IsDialogueAct = false;
            DialogueUI.SetActive(false);
            Player.GetComponent<FirstPersonAIO>().playerCanMove = true;
        }
    }
    void NextName(){
        if (indexForname < lines2.Length-1){
            indexForname++;
            charName.text = lines2[indexForname];
        }
        else
        {
            charName.text = string.Empty;
        }
    }
}
