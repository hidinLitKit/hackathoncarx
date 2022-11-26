using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject myPrefab;
    void Start()
    {
        Instantiate(myPrefab, gameObject.transform.position, gameObject.transform.rotation);
    }
}
