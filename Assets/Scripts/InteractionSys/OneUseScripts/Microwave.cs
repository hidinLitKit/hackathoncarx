using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Microwave : MonoBehaviour
{
    public float audioTime;
    public GameObject CasseteAppear;
    public GameObject can;
    public void StartHeating()
    {
        gameObject.GetComponent<Light>().enabled = true;
        gameObject.GetComponent<AudioSource>().Play();
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().playerCanMove = false;
        StartCoroutine(waiting());
        CasseteAppear.SetActive(true);
    }

    IEnumerator waiting()
    {
        yield return new WaitForSeconds(audioTime);
        gameObject.GetComponent<Light>().enabled = false;
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().playerCanMove = true;
        can.SetActive(true);
    }

}
