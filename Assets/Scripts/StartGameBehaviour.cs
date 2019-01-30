using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject button;
    [SerializeField]
    private GameObject loadingBar;
    [SerializeField]
    private Image bar;


    private void Awake()
    {
        button.SetActive(true);
        loadingBar.SetActive(false);
    }

    public void OnStartGameButtonClicked()
    {
        button.SetActive(false);
        loadingBar.SetActive(true);
        StartCoroutine(LoadAsync("SampleScene"));
    }

    IEnumerator LoadAsync (string sceneName)
    {

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        while (!op.isDone) 
        {
            var progress = Mathf.Clamp01(op.progress/0.9f);
            bar.fillAmount = progress;

            yield return null;
        }
    }
} 
