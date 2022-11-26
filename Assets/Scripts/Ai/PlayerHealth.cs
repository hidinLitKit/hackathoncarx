using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int health;
    public int Health{
    get{
        return health; 
    }
    set{
        if(value == 0)
            PlayerIsDead();
        health = value;
    }
    }

    [SerializeField] int maxHealth = 5;
    // Start is called before the first frame update
    void Start()
    {
        Health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerIsDead()
    {

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    } 
}
