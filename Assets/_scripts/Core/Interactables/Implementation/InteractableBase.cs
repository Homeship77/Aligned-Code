using Core.Effects;
using Core.Player;
using EventSystems;
using Interfaces;
using UnityEngine;

namespace Core.Interactables
{
    public class InteractableBase : AInteractableItem
    {
        [SerializeField]
        private Transform _lowUprgadeTarget;
        [SerializeField]
        private int _coinsToLowUpgrade = 5;
        [SerializeField]
        private int _coinsToHighUpgrade = 25;
        [SerializeField]
        private GameObject _highGradePrefab;
        [SerializeField]
        private string _spetCoinsEffect;

        private int _lowUpgrades = 0;
        private int _highUpgrades = 0;
        private int _coinsSpended = 0;

        public override EInteractableType InteractableType =>EInteractableType.eit_base;

        public override void Action(PlayerSessionData sessionData)
        {
            var playerCoins = sessionData.Coins;
            EventManager.RaiseEvent<IGameEffectEvent>(handler => handler.AddEffect(_spetCoinsEffect, transform.position, sessionData.PlayerPosition, () => { SpentCoins(playerCoins); }));
        }

        public override void OnStart()
        {
            base.OnStart();
            _lowUpgrades = 0;
            _highUpgrades = 0;
        }

        private void SpentCoins(int value) 
        {
            EventManager.RaiseEvent<IGameEvent>(handler => handler.ProcessEvent(InteractableType, -value));
            _coinsSpended += value;

            var lowUpgr = _coinsSpended / _coinsToLowUpgrade;
            for (int i = 0; i < (lowUpgr - _lowUpgrades); i++) 
            {
                LowUpgrade();
            }
            var highUpgr = _coinsSpended / _coinsToHighUpgrade;
            for (int i = 0; i < (highUpgr - _highUpgrades); i++)
            {
                HighUpgrade();
            }
        }

        private void LowUpgrade()
        {
            _lowUpgrades++;
            _lowUprgadeTarget.localScale = _lowUprgadeTarget.localScale + Vector3.one * 0.1f;
            EventManager.RaiseEvent<IGameEvent>(handler => handler.ProcessEvent(EInteractableType.eit_upgrade, -1));
        }

        private void HighUpgrade() 
        {
            _highUpgrades++;

            if (_highGradePrefab != null)
            {
                var go = Instantiate(_highGradePrefab, transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.AngleAxis(_highUpgrades * 10f, Vector3.up);
            }

           EventManager.RaiseEvent<IGameEvent>(handler => handler.ProcessEvent(EInteractableType.eit_upgrade, 1));
        }

    }
}
