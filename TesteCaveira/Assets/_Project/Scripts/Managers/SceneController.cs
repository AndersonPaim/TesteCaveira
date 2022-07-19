using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour, ISceneLoader
{
    public void LoadScene(string scene)
    {
        UniTask uniTask = LoadASync(scene);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private async UniTask LoadASync(string scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        float loadingProgress = Mathf.Clamp01(operation.progress / 0.9f);
        await UniTask.WaitUntil(() => operation.isDone);
    }
}