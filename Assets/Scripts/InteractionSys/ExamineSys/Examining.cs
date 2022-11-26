using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Examining : MonoBehaviour
{
    protected Vector3 posLastFrame;
    public Camera ExamCam;
    void Start()
    {

    } 
    void Update()
    {
         if (Input.GetMouseButtonDown(0))
            posLastFrame = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            var delta = Input.mousePosition - posLastFrame;
            posLastFrame = Input.mousePosition;

            var axis = Quaternion.AngleAxis(-90f, Vector3.forward)*delta;
            transform.rotation = Quaternion.AngleAxis(delta.magnitude*0.1f,axis)*transform.rotation;

        } 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            transform.GetChild(0).gameObject.GetComponent<ExaminationSys>().EndExamination();
            GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().enableCameraMovement = true;

        }
    }
}
