using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	
	public string startingSceneName = "Demo";
	public Text debugText;
	SceneController scenecontroller;

	private IEnumerator Start ()
	{
		//Load Starting Scene
		scenecontroller = GetComponent<SceneController> ();
        if (startingSceneName != "")
		    yield return StartCoroutine (scenecontroller.LoadSceneAndSetActive (startingSceneName));
	}
}
