using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NoteCollector : Interactable
{
    public GameObject player;
    public GameObject Garbage;
    public GameObject objective;
    public TextMeshProUGUI notes;
    public  AudioSource PickAudio;
    public GameObject monster;

    void Awake()
    {
        PickAudio  = GameObject.Find("PickItem").GetComponent<AudioSource>();
        Garbage = GameObject.Find("Garbage");
    }
    public override string GetDescription() {
        return "take";
    }

    public override void Interact() {
        player.GetComponent<PlayerNotes>().count+=1;
        PickAudio.Play();
        transform.localPosition = Garbage.transform.position;
        notes.text = "find seven notes\n"+"("+player.GetComponent<PlayerNotes>().count+"/7)";
        if (player.GetComponent<PlayerNotes>().count == 3)
        {
            monster.SetActive(true);
        }
        if (player.GetComponent<PlayerNotes>().count == 7)
        {
            SceneManager.LoadScene("Win");
        }
    }
}
