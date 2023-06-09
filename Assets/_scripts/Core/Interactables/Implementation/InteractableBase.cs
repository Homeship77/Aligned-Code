﻿using Core.Player;
using EventSystems;
using Interfaces;
using UnityEngine;

namespace Core.Interactables
{
    public class InteractableBase : AInteractableItem
    {
        [SerializeField]
        private GameSettings _settings;
        [SerializeField]
        private Transform _lowUprgadeTarget;
        [SerializeField]
        private GameObject _highGradePrefab;
        [SerializeField]
        private string _spentCoinsEffect;

        private int _lowUpgrades = 0;
        private int _highUpgrades = 0;
        private int _coinsSpended = 0;

        public override EInteractableType InteractableType =>EInteractableType.eit_base;

        public override void Action(PlayerSessionData sessionData)
        {
            var playerCoins = sessionData.Coins;
            EventManager.RaiseEvent<IGameEffectEvent>(handler => handler.AddEffect(_spentCoinsEffect, transform.position, sessionData.PlayerPosition, out go, () => { SpentCoins(playerCoins); }));
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

            var lowUpgr = _coinsSpended / _settings.CoinsToLowUpgrade;
            for (int i = 0; i < (lowUpgr - _lowUpgrades); i++) 
            {
                LowUpgrade();
            }
            var highUpgr = _coinsSpended / _settings.CoinsToHighUpgrade;
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
