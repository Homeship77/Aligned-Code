using UnityEngine;

namespace Core.Player
{
    public class PlayerSessionData
    {
        public int Coins { get; private set; }
        public int Health { get; private set; }

        public Vector3 PlayerPosition { get; private set; }
        
        public PlayerSessionData(int startHealth)
        {
            Health = startHealth;
        }

        public void UpdatePosition(Vector3 newPos)
        {
            PlayerPosition = newPos;
        }

        public void ChangeCoinsValue(int value = 1)
        {
            Coins += value;
        }

        public void ChangeHealthValue(int value = 1) 
        { 
            Health += value;
        }

        public void SetHealthValue(int value)
        {
            Health = value;
        }
    }
}
