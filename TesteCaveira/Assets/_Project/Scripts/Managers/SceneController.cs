using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Managers
{
    public class SceneController : ISceneLoader
    {
        public void LoadScene(string scene)
        {
            LoadASync(scene);
        }

        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private async UniTaskVoid LoadASync(string scene)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
            operation.allowSceneActivation = false;

            while(operation.progress < 0.9f)
            {
                float loadingProgress = Mathf.Clamp01(operation.progress / 0.9f);
                await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            }

            await UniTask.Delay(2000);
            operation.allowSceneActivation = true;
        }

        public void Dispose()
        { }
    }
}