using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{   GameObject Gun;
    public int Current_Ammo = 10;
    public int Max_Ammo = 10;
    GameObject camera;
    void Start()
    {
        Gun = GameObject.FindGameObjectWithTag("Gun");
        camera = Camera.main.gameObject;
    }

    void Update()
    {
        if (Gun != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (Current_Ammo > 0)
                {
                    Shoot_();
                }
            }
            if(Input.GetKeyDown(KeyCode.R)) 
            {
                if (Current_Ammo != Max_Ammo)
                {
                    Reload();
                }
            }
        }
    }
    void Shoot_()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, Mathf.Infinity))
        {
            
        }
    }
    void Reload()
    {

    }
}
