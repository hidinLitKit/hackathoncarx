using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Collider collider= GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyControllerAi enemy;
        if (other.gameObject.TryGetComponent<EnemyControllerAi>(out enemy))
        {
            enemy.Health--;
            Debug.Log("hit");
        }

    }
}
