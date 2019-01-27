using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private void Start ()
	{
		//Load Starting Scene
		scenecontroller = GetComponent<SceneController> ();

        state = GameState.StartScreen;
        //NadineController.cannotMove = true;
        scenecontroller.SetActiveLast();
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
        if (Input.GetButtonDown("Submit"))//GetButtonDown("Submit")
        {
            Debug.Log(1);
            DialogManager.dialogmanager.DisplayNextSentence();//DisplayNextDialog
        }
    }

    public void StartGame()
    {
        overlay.SetActive(false);
        question.SetActive(false);
        NadineController.cannotMove = false;
    }

    public void RestartGame()
    {
        overlay.SetActive(true);
        question.SetActive(false);
        startScreen.SetActive(true);
        NadineController.cannotMove = true;
        state = GameState.StartScreen;
        SceneManager.LoadScene("Level");
    }
}

