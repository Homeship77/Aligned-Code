using EventSystems;
using Interfaces;
using UnityEngine;

namespace Core.Player
{
    public class PlayerSessionData
    {
        public int Coins { get; private set; }
        public int Health { get; private set; }
        
        public int Mana { get; private set; }
        public int LowUpgrades { get; private set; }
        public int HighUpgrades { get; private set; }

        public int CoinsSpended { get; private set; }

        private int _startHealth;
        private int _startMana;


        public Vector3 PlayerPosition { get; private set; }
        

        public PlayerSessionData(int startHealth, int startMana)
        {
            _startHealth = startHealth;
            Health = startHealth;
            _startMana = startMana;
            Mana = startMana;
        }

        public void UpdatePosition(Vector3 newPos)
        {
            PlayerPosition = newPos;
        }

        public void ChangeCoinsValue(int value = 1)
        {
            Coins += value;
            if (value < 0)
                CoinsSpended -= value;
        }

        public void AddLowUpgrade(int value)
        {
            LowUpgrades += value;
        }

        public void AddHighUpgrade(int value)
        {
            HighUpgrades += value;
        }


        public void ChangeManaValue(int value = 1)
        {
            Mana += value;
            Mana = Mathf.Clamp(Mana, 0, _startMana);
        }

        public void SetManaValue(int value)
        {
            Mana = value;
        }

        public void ChangeHealthValue(int value = 1) 
        { 
            Health += value;
            Health = Mathf.Clamp(Health, 0, _startHealth);
            CheckHealth();
        }

        public void SetHealthValue(int value)
        {
            Health = value;
            CheckHealth();
        }

        private void CheckHealth() 
        { 
            if (Health <= 0)
            {
                EventManager.RaiseEvent<IGameEvent>(handler => handler.CriticalEvent(EEventType.eet_death));
            }
        }
    }
}
