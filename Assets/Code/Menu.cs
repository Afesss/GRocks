using Code.Services;
using UnityEngine;

namespace Code
{
    public class Menu : MonoBehaviour
    {
        public void LoadGame()
        {
            AllServices.Get<SceneLoadService>().LoadScene(SceneLoadService.GameScene);
        }
    }
}
