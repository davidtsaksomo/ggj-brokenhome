using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public static DialogManager dialogmanager;

    private Queue<Dialog> dialogQueue;
    private Queue<string> sentenceQueue;
    private Dialog currentDialog;

    void Awake()
    {
        if(dialogmanager == null)
        {
            dialogmanager = this;
        } else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        dialogQueue = new Queue<Dialog>();
        sentenceQueue = new Queue<string>();
    }

    public void StartDialogEvent(DialogEvent dialogEvent)
    {
        dialogQueue.Clear();
        foreach(Dialog dialog in dialogEvent.dialogs)
        {
            dialogQueue.Enqueue(dialog);
        }
        DisplayNextDialog();
    }
    
    void DisplayNextDialog()
    {
        if(dialogQueue.Count == 0)
        {
            EndDialog();
            return;
        }
        currentDialog = dialogQueue.Dequeue();
        sentenceQueue.Clear();
        foreach (string sentence in currentDialog.sentences)
        {
            sentenceQueue.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    void DisplayNextSentence()
    {
        if(sentenceQueue.Count == 0)
        {
            DisplayNextDialog();
            return;
        }
        string sentence = sentenceQueue.Dequeue();
        print(currentDialog.speaker + ": " + sentence);
    }

    void EndDialog()
    {
        print("End conversation");
    }
}
