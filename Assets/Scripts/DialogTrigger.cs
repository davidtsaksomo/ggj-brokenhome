using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public DialogEvent dialogEvent;
    bool once = true;
    void Update()
    {
        if (Input.GetButtonDown("Submit"))//GetButtonDown("Submit")
        {
            DialogManager.dialogmanager.DisplayNextDialog();//DisplayNextDialog
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log(coll.tag);
        Debug.Log(coll.name);
        if (once)//coll.tag == "Player"//
        {
            DialogManager.dialogmanager.StartDialogEvent(dialogEvent);
            once = false;
        }
    }
}
