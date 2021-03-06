using System.Collections;
using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour, ISceneLoader
{
    public void LoadScene(string scene)
    {
        StartCoroutine(LoadASync(scene));
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator LoadASync(string scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        while(!operation.isDone)
        {
            float loadingProgress = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }
    }

}