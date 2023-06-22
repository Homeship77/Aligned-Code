using Core.SpellSystem;
using EventSystems;
using Interfaces;
using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class UIController : MonoBehaviour, IUIEvent
    {
        [SerializeField]
        UIDiscreteValueBar _healthBar;
        [SerializeField]
        UIDiscreteValueBar _manaBar;
        [SerializeField]
        TMP_Text  _coinsTextValue;
        [SerializeField]
        TMP_Text _coinsSpendedTextValue;

        [SerializeField]
        TouchUIElement _spellUIElement;

        [SerializeField]
        UIFlash _hitEffect;

        [SerializeField]
        private SpellsDatabase _spells;

        [SerializeField]
        private UIElementsDatabase _uiElements;

        //private UIBuilder _uiBuilder;

        private void Start () 
        {
            EventManager.Subscribe(this);
           // _uiBuilder = new UIBuilder(_spells, _uiElements, _spellUIElement);
        }

        private void OnDestroy() 
        { 
            EventManager.Unsubscribe(this);
        }

        private void SetCoinValue(int value)
        {
            _coinsTextValue.text = value.ToString();
        }

        private void SetManaValue(int value)
        {
            _manaBar.SetValues(value);
        }

        private void SetHealthValue(int value)
        {
            _healthBar.SetValues(value);
        }

        private void SetSpendedCoinsValue(int value) 
        {
            _coinsSpendedTextValue.text = value.ToString();
        }

        private void HitEffect()
        {
            _hitEffect.ApplyHitEffect();
        }


        public void ProcessUIEvent(EInteractableType eventType, int value)
        {
            switch (eventType)
            {
                case EInteractableType.eit_damageZone:
                    HitEffect();
                    SetHealthValue(value);
                    break;
                case EInteractableType.eit_base:
                    SetSpendedCoinsValue(value);
                    break;
                case EInteractableType.eit_health:
                    SetHealthValue(value);
                    break;
                case EInteractableType.eit_mana:
                    SetManaValue(value);
                    break;
                case EInteractableType.eit_coin:
                    SetCoinValue(value);
                    break;
                case EInteractableType.eit_none:
                    break;
            }
        }

        public void CriticalUIEvent(EEventType eventType)
        {
            switch (eventType)
            {
                case EEventType.eet_death:
                    SetHealthValue(0);
                    SetManaValue(0);
                    break;
                case EEventType.eet_respawn:
                    break;

            }
        }
    }
}
