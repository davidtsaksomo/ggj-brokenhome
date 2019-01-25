using UnityEngine;

[CreateAssetMenu(fileName = "DialogEvent", menuName = "Dialogs/DialogEvent")]
public class DialogEvent : ScriptableObject
{
    public Dialog[] dialogs;
}
