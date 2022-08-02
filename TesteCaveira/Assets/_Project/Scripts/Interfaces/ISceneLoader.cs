using Coimbra.Services;

namespace _Project.Scripts.Interfaces
{
    public interface ISceneLoader : IService
    {
        void LoadScene(string scene);
        void RestartScene();
    }
}