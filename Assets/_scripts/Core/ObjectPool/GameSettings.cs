using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game/Game Settings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        public int StartHealth = 5;
        public int MaxTakeable = 35;
        public int MaxTreates = 25;

        public int CoinsToLowUpgrade = 5;
        public int CoinsToHighUpgrade = 25;
    }
}
