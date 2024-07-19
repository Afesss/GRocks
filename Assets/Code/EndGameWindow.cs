using System;
using Code.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class EndGameWindow : MonoBehaviour
    {
        [SerializeField] private Text _endGameText;

        private Action _restart;

        public void Show(string text, Color color, Action restart)
        {
            _endGameText.text = text;
            _endGameText.color = color;
            _restart = restart;
            gameObject.SetActive(true);
        }

        public void Restart()
        {
            gameObject.SetActive(false);
            _restart?.Invoke();
        }

        public void Menu()
        {
            AllServices.Get<SceneLoadService>().LoadScene(SceneLoadService.MenuScene);
        }
    }
}
