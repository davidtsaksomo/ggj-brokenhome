using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogManager : MonoBehaviour
{
    public static DialogManager dialogmanager;
    public GameObject dialogBox;
    public Text speaker;
    public Text sentence;

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
        Debug.Log("Dialog Queue");
        
        DisplayNextDialog();

        dialogBox.SetActive(true);
        NadineController.cannotMove = true;
    }
    
    public void DisplayNextDialog()
    {
        Debug.Log("New dialog started");
        if(dialogQueue.Count == 0)
        {
            EndDialog();
        }
        else{
        currentDialog = dialogQueue.Dequeue();
        
        sentenceQueue.Clear();
        foreach (string sentence in currentDialog.sentences)
        {
            sentenceQueue.Enqueue(sentence);
        }
        DisplayNextSentence();
        }
    }

    public void DisplayNextSentence()
    {
        Debug.Log("Sentence COunt");
        Debug.Log(sentenceQueue.Count);
        if(sentenceQueue.Count == 0)
        {
            Debug.Log("Ayu Next");
            DisplayNextDialog();
        }
        else{
        Debug.Log("Sentence queueue");
        foreach (string dialog in sentenceQueue){
        //    Debug.Log(dialog);
        }
        string current_sentence = sentenceQueue.Dequeue();
        //Debug.Log(1);
        //Debug.Log(current_sentence);
        speaker.text = currentDialog.speaker;
        sentence.text = current_sentence;
        }
    }

    void EndDialog()
    {
        dialogBox.SetActive(false);
        //speaker.gameObject.SetActive(false);
        //sentence.gameObject.SetActive(false);
        NadineController.cannotMove = false;

    }
}
