using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class ItemEAT : Interactable
{
    public GameObject DisableCol;
    public GameObject item;
    public  AudioSource EatAud;
    private GameObject blackscreen;
    public GameObject trig;
    private Image img;
    
    
    void Awake()
    {
        blackscreen = GameObject.Find("BlackScreen");
        img = blackscreen.GetComponent<Image>();
        item = gameObject;
        EatAud  = gameObject.GetComponent<AudioSource>();
    }
    public override string GetDescription() {
        return "eat";
    }

    public override void Interact() {
        StartCoroutine(Fading());


    }

    IEnumerator Fading()
    {
        img.DOFade(1f,1f);
        yield return new WaitForSeconds(1f);
        EatAud.Play();
        item.SetActive(false);
        img.DOFade(0f,1f);
        trig.SetActive(true);
        DisableCol.SetActive(false);
    }
}
