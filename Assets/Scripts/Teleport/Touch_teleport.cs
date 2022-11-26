using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_teleport : MonoBehaviour
{
    [SerializeField] GameObject point;
    FirstPersonAIO fpc;
    private void Start()
    {
        
    }
    public void Teleport(GameObject teleportable)
    {
        fpc = teleportable.GetComponent<FirstPersonAIO>();
        bool canmove = fpc.playerCanMove;
        fpc.playerCanMove = true;
        teleportable.transform.position = point.transform.position;
        fpc.playerCanMove = canmove;
    }
}
