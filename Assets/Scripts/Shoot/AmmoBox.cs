using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObject.FindGameObjectWithTag("Player")){
            other.gameObject.GetComponentInChildren<SimpleShoot>().Total_Ammo += 10;
            other.gameObject.GetComponentInChildren<SimpleShoot>().RefreshAmmo();
            Destroy(gameObject);
        }
    }
}
