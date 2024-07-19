using UnityEngine;

namespace Code.Cards
{
    [CreateAssetMenu(fileName = nameof(CardSettings), menuName = "Card/" + nameof(CardSettings))]
    public class CardSettings : ScriptableObject
    {
        [Range(1, 6)] public byte Strength;
    }
}
