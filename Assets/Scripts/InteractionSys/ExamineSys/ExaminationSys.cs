using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Currently not using system 
public class ExaminationSys : Interactable
{
    public Camera PlayerCam;
    public Camera ExamCam;
    public GameObject ExamItem;
    GameObject Examinating;
    public bool Interacted;
    public Transform parent;
    public override string GetDescription() {
        if (Interacted == false) return "Examine";
        return null;
    }

    public override void Interact() {
        StartExamination();
    }
    void StartExamination()
    {
        Examinating = Instantiate(ExamItem, parent) as GameObject;
        Examinating.transform.localPosition = Vector3.zero;
        PlayerCam.enabled = false;
        ExamCam.enabled = true;
        Examinating.gameObject.layer=8;
        //this.gameObject.SetActive(false);
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().playerCanMove = false;
       // GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().lockAndHideCursor = false;
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().enableCameraMovement = false;
        GameObject.Find("Examination").GetComponent<Examining>().enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void EndExamination()
    {
        PlayerCam.enabled = true;
        ExamCam.enabled = false;
        GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().playerCanMove = true;
     //   GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().lockAndHideCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Destroy(ExamItem);
        GameObject.Find("Examination").GetComponent<Examining>().enabled = false;
        
    }
}
