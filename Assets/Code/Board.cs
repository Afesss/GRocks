using System;
using System.Collections;
using System.Collections.Generic;
using Code.Cards;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Code
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private Camera _mainCam;
        [SerializeField] private Card _cardPrefab;
        [SerializeField] private CardSettings[] _cardSettings;
        [SerializeField] private Transform _playerCardDeckPosition;
        [SerializeField] private Transform _botCardDeckPosition;
        [SerializeField] private Transform _cardContainer;
        [SerializeField] private Transform[] _playerCarsPosition;
        [SerializeField] private Transform[] _botCarsPosition;

        public const float CardMoveToStartPosDuration = 1f;
        public const float CardRotationDuration = 0.5f;
        private const byte DeckLength = 30;

        public List<Card> BotCards => _botCards;
        public List<Card> PlayerCards => _playerCards;
        
        private List<Card> _playerCardDeck = new List<Card>();
        private List<Card> _botCardDeck = new List<Card>();
        private List<Card> _playerCards = new List<Card>();
        private List<Card> _botCards = new List<Card>();
        private void Awake()
        {
            CreateCardDeck(ref _playerCardDeck, _playerCardDeckPosition.position);
            CreateCardDeck(ref _botCardDeck, _botCardDeckPosition.position);
        }

        public void StartGame(Action onGameStarted)
        {
            _botCards.Clear();
            _playerCards.Clear();
            ResetCardPositions(ref _playerCardDeck, _playerCardDeckPosition.position);
            ResetCardPositions(ref _botCardDeck, _botCardDeckPosition.position);
            SortCard(ref _playerCardDeck);
            SortCard(ref _botCardDeck);
            InitCards(_playerCardDeck, _playerCarsPosition, ref _playerCards, true);
            InitCards(_botCardDeck, _botCarsPosition, ref _botCards, false);

            StartCoroutine(WaitStartGame(onGameStarted));
        }

        private IEnumerator WaitStartGame(Action onGameStarted)
        {
            yield return new WaitForSeconds(CardMoveToStartPosDuration + CardRotationDuration);
            onGameStarted?.Invoke();
        }

        private void ResetCardPositions(ref List<Card> cards, Vector3 position)
        {
            foreach (Card card in cards)
            {
                card.ResetCard(position);
            }
        }

        private void InitCards(List<Card> cards, Transform[] positions, ref List<Card> initializedCards, bool isPlayer)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                var card = cards[i];
                card.Init(positions[i], isPlayer);
                initializedCards.Add(card);
            }
        }

        private void CreateCardDeck(ref List<Card> cardList, Vector3 spawnPos)
        {
            for (int i = 0; i < _cardSettings.Length; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Card card = Instantiate(_cardPrefab, spawnPos, Quaternion.Euler(0, 0, 180), _cardContainer);
                    card.Construct(_cardSettings[i], _mainCam);
                    cardList.Add(card);
                }
            }
        }

        private void SortCard(ref List<Card> cardList)
        {
            List<Card> tmpList = new List<Card>();
            for (int i = 0; i < DeckLength; i++)
            {
                var card = cardList[Random.Range(0, cardList.Count)];
                cardList.Remove(card);
                tmpList.Add(card);
            }
            cardList = tmpList;
        }
    }
}
