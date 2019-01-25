using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName = "Dialogs/Dialog")]
public class Dialog : ScriptableObject 
{
    public string speaker;
    public string[] sentences;
}
