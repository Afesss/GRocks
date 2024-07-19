using UnityEngine.SceneManagement;

namespace Code.Services
{
    public class SceneLoadService : IService
    {
        public const string MenuScene = "Menu";
        public const string GameScene = "Game";
        
        public void LoadScene(string scene)
        {
            SceneManager.LoadSceneAsync(scene);
        }
    }
}
