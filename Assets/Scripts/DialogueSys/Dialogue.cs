using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// ИСПРАВИТЬ БАГ КОГДА МОЖНО БЕСКОНЕЧНО НАЖИМАТЬ НА ОБЪЕКТ И КОРОЧЕ ДИАЛОГ ЛОМАЕТСЯ
public class Dialogue : Interactable
{

    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI charName;
    public string[] lines;
    public string[] lines2;
    public float textSpeed;
    public GameObject DialogueUI;
    public string returntex;
    private int index;
    private int indexForname;
    private bool IsDialogueAct = false;
    void Awake()
    {

    }
    public override void Interact()
    {  // Starting dialogue - disabling player movement 
        StopAllCoroutines();
        DialogueUI.SetActive(true);
        textComponent.text = string.Empty;
        StartDialogue(); 
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().playerCanMove = false;
        IsDialogueAct = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDialogueAct){
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
            DialogueUI.SetActive(false);
            GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().playerCanMove = true;
            IsDialogueAct = false;
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
    public override string GetDescription()
    {
       return returntex;
    }
}
