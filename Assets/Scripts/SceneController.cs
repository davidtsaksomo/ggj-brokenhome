using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{

	public IEnumerator SwitchScenes (string sceneName)
	{

		yield return SceneManager.UnloadSceneAsync (SceneManager.GetActiveScene ().buildIndex);
		yield return StartCoroutine (LoadSceneAndSetActive (sceneName));

	}
	public IEnumerator LoadSceneAndSetActive (string sceneName)
	{
		yield return SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);
		Scene newlyLoadedScene = SceneManager.GetSceneAt (SceneManager.sceneCount - 1);
		SceneManager.SetActiveScene (newlyLoadedScene);
	}
    public void SetActiveLast()
    {
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newlyLoadedScene);
    }

}