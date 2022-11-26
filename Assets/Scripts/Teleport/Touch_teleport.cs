using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Touch_teleport : Interactable
{
    GameObject player;
    [SerializeField] GameObject point;
    FirstPersonAIO fpc;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fpc = player.GetComponent<FirstPersonAIO>();
    }
    public void Teleport(GameObject teleportable)
    {
        bool canmove = fpc.playerCanMove;
        fpc.playerCanMove = true;
        teleportable.transform.position = point.transform.position;
        fpc.playerCanMove = canmove;
    }

    public override string GetDescription()
    {
        return "";
    }

    public override void Interact()
    {
        Debug.Log("Hello");
        Teleport(player);
    }
}
