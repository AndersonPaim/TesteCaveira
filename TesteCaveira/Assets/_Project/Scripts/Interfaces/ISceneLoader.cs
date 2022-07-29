using Coimbra.Services;

namespace Interfaces
{
    public interface ISceneLoader //: IService
    {
        void LoadScene(string scene);
        void RestartScene();
    }
}