using Core.Attributes;
using Core.Player;
using EventSystems;
using Interfaces;
using System;

namespace Core.Interactables
{
    [InteractableHandler(EInteractableType.eit_damageZone)]
    public class IntDamageZoneEventHandler : AInteractableEventHandler
    {
        private float _coolDownTimer = 0f;
        private const float DAMAGE_COOLDOWN = 1.75f;

        public IntDamageZoneEventHandler(PlayerSessionData plData) : base(plData) { }
        public override void ProcessInteractableEvent(int value)
        {
            if (_coolDownTimer < 0f)
            {
                _plData.ChangeHealthValue(value);
                EventManager.RaiseEvent<IUIEvent>(handler => handler.ProcessUIEvent(EInteractableType.eit_damageZone, _plData.Health));
                _coolDownTimer = DAMAGE_COOLDOWN;
            }
        }

        public override void InitEventHandler(Action<float> updateEvent)
        {
            updateEvent += NeedUpdate;
        }

        public override void NeedUpdate(float delta)
        {
            _coolDownTimer -= delta;
        }
    }
}
