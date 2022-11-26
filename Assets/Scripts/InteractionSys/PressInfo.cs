using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressInfo : MonoBehaviour
{
    public GameObject InfoPool, DescrPool;
    public KeyCode InfoKey;
    public AudioSource SwitchSound;
    private bool isDescrActive = false;
    private bool isInfoActive = true;
    void Update()
    {
        if (Input.GetKeyDown(InfoKey))
        {
            isDescrActive = !isDescrActive;
            isInfoActive = !isInfoActive;
            DescrPool.SetActive(isDescrActive);
            InfoPool.SetActive(isInfoActive);
            SwitchSound.Play();
        }
        
    }
}
