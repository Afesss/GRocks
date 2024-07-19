using Code.Cards;
using Code.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code
{
    public class GameRules : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] private EndGameWindow _endGameWindow;

        private Card _botCard;

        private void Start()
        {
            _board.StartGame(OnGameStarted);
        }

        private void RestartGame()
        {
            _board.StartGame(OnGameStarted);
        }

        private void OnGameStarted()
        {
            _botCard = _board.BotCards[Random.Range(0, _board.BotCards.Count)];
            _botCard.AutoMoveCardToCenter();
            for (int i = 0; i < _board.PlayerCards.Count; i++)
            {
                _board.PlayerCards[i].Overlap += OnCardOverlap;
            }
        }

        private void OnCardOverlap(Card card)
        {
            for (int i = 0; i < _board.PlayerCards.Count; i++)
            {
                _board.PlayerCards[i].Overlap -= OnCardOverlap;
                _board.PlayerCards[i].ResetState();
            }
            CheckWinner(card);
        }

        private void CheckWinner(Card card)
        {
            string endGameText = "";
            Color color;
            if (card.CardSettings.Strength < _botCard.CardSettings.Strength)
            {
                card.gameObject.SetActive(false);
                endGameText = "Lose";
                color = Color.red;
            }
            else if(card.CardSettings.Strength > _botCard.CardSettings.Strength)
            {
                _botCard.gameObject.SetActive(false);
                endGameText = "Win";
                color = Color.green;
            }
            else
            {
                card.gameObject.SetActive(false);
                _botCard.gameObject.SetActive(false);
                endGameText = "Draw";
                color = Color.blue;
            }
            
            _endGameWindow.Show(endGameText, color, RestartGame);
        }
    }
}
