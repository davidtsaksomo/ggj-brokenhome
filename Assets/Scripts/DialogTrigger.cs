using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public DialogEvent dialogEvent;
    bool once = true;
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            DialogManager.dialogmanager.DisplayNextDialog();
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "Player" && once)
        {
            DialogManager.dialogmanager.StartDialogEvent(dialogEvent);
            once = false;
            print("berhasil");
        }
    }
}
