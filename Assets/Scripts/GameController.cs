using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	
	public string startingSceneName = "Demo";
	SceneController scenecontroller;
    public DialogEvent events;

    [Header("UI Objects")]
    public GameObject overlay;
    public GameObject startScreen;
    public GameObject question;

    public static GameController gameController; 
    void Awake()
    {
        if (gameController == null)
        {
            gameController = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public enum GameState
    {
        StartScreen,
        Paused,
        Running,
        ItemInspect
    }

    public GameState state;
    private IEnumerator Start ()
	{
		//Load Starting Scene
		scenecontroller = GetComponent<SceneController> ();
        if (startingSceneName != "")
		    yield return StartCoroutine (scenecontroller.LoadSceneAndSetActive (startingSceneName));

        state = GameState.StartScreen;
        //NadineController.cannotMove = true;
	}

    private void Update()
    {
        switch (state)
        {
            case GameState.StartScreen:
                if (Input.GetButtonDown("Start"))
                {
                    startScreen.SetActive(false);
                    question.SetActive(true);
                    state = GameState.Paused;
                }
                break;
        }
    }

    public void StartGame()
    {
        overlay.SetActive(false);
        question.SetActive(false);
        NadineController.cannotMove = false;
    }
}
