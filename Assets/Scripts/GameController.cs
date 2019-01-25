using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	
	public string startingSceneName = "Demo";
	public Text debugText;
	SceneController scenecontroller;
    public DialogEvent events;
	private IEnumerator Start ()
	{
		//Load Starting Scene
		scenecontroller = GetComponent<SceneController> ();
        if (startingSceneName != "")
		    yield return StartCoroutine (scenecontroller.LoadSceneAndSetActive (startingSceneName));
        DialogManager.dialogmanager.StartDialogEvent(events);
	}
}
