using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepCrouching : MonoBehaviour
{
    FirstPersonAIO script;
    void Start()
    {
        script = GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>();
    }


    void OnCollisionStay(Collision collisionInfo)
    {
        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal * 10, Color.white);
            script.isCrouching = true;
        }
    }


}
