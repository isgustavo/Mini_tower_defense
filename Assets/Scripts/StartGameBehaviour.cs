using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameBehaviour : MonoBehaviour
{
    public void OnStartGameButtonClicked()
    {
        StartCoroutine(LoadAsync("SampleScene"));
    }

    IEnumerator LoadAsync (string sceneName)
    {

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        while (!op.isDone) 
        {

            yield return null;
        }
    }
} 
