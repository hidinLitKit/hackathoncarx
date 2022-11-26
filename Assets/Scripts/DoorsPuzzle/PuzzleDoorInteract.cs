using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoorInteract : MonoBehaviour
{
    private GameObject Player;
    public int counter;
    public GameObject doorZero, doorMultTwo, doorPlusOne, doorMinusOne, doorPlusThree;
    
    private void Awake()
    {
        Player = GameObject.Find("FirstPerson-AIO");
    }

    public void Update()
    {
        countInt(counter);
    }

    public void countInt(int op)
    {
        switch(op)
        {
            case 11:
            Invoke("case11",1.1f);
            break;
            case 22:
            Invoke("case22",1.1f);
            break;

            case 23:
            Invoke("case23",1.1f);
            break;

            case 999999:
            Invoke("case9",1.1f);
            break;
            
        }

    }
    private void case11()
    {
        doorMultTwo.SetActive(false);

    }
    private void case22()
    {
        doorPlusOne.SetActive(false);
    }

    private void case23()
    {
        doorPlusThree.SetActive(false);
    }
    private void case9()
    {
        doorMinusOne.SetActive(false);
    }



}
