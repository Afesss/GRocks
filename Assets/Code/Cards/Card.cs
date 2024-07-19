using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Cards
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private TMP_Text _numberText;
        [SerializeField] private Collider _collider;

        public event Action<Card> Overlap;
        private const float MoveToPosDuration = 0.3f;
        public State CurrentState { get; private set; }
        public CardSettings CardSettings { get; private set; }

        private Collider[] _overlapColliders = new Collider[100];
        private Vector3 _originPos;

        private Camera _mainCam;

        public void Construct(CardSettings cardSettings, Camera mainCam)
        {
            _mainCam = mainCam;
            CardSettings = cardSettings;
            _numberText.text = CardSettings.Strength.ToString();
            CurrentState = State.None;
        }

        public void Init(Transform initPos, bool isPlayer)
        {
            transform.DOMove(initPos.position, Board.CardMoveToStartPosDuration).OnComplete(() =>
            {
                _originPos = transform.position;
                transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), Board.CardRotationDuration)
                    .OnComplete(() => CurrentState = isPlayer ? State.Player : State.Bot);
            });
        }

        public void ResetCard(Vector3 position)
        {
            transform.position = position;
            transform.rotation = Quaternion.Euler(0, 0, 180);
            gameObject.SetActive(true);
            CurrentState = State.None;
        }

        public void ResetState()
        {
            CurrentState = State.None;
        }

        public void AutoMoveCardToCenter()
        {
            transform.DOMove(Vector3.zero, Board.CardMoveToStartPosDuration).OnComplete(() =>
            {
                CurrentState = State.BotStep;
            });
        }

        private void OnMouseDrag()
        {
            if (CurrentState is not State.Player)
                return;
            var pos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
            pos.y = 1;
            transform.position = pos + Vector3.forward * 2f;
        }

        private void OnMouseUp()
        {
            if (CurrentState is not State.Player)
                return;

            int overlapCount = Physics.OverlapSphereNonAlloc(transform.position, 1, _overlapColliders);
            for (int i = 0; i < overlapCount; i++)
            {
                if (_overlapColliders[i].TryGetComponent(out Card card))
                {
                    if (card.CurrentState is State.BotStep)
                    {
                        Overlap?.Invoke(this);
                        transform.DOMove(Vector3.up, MoveToPosDuration);
                        return;
                    }
                }
            }
            transform.DOMove(_originPos, MoveToPosDuration);
        }

        public enum State : byte
        {
            None = 0,
            Bot = 1,
            Player = 2,
            BotStep = 3
        }
    }
}