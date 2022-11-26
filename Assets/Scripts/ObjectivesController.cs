using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TMPro;

public class ObjectivesController : MonoBehaviour
{
    List<string> objectives = new List<string>(){"Найдите напарника", "найдите способ выбраться \n (найдите все улики)", "Выберетесь!"};
    public TextMeshProUGUI text;
    int index = 0;

    void Start()
    {
        text.text = objectives[index];
    }

    void Update()
    {
        
    }

    public void NextObjective()
    {
        index++;
        text.text = objectives[index]; 
    }
}
