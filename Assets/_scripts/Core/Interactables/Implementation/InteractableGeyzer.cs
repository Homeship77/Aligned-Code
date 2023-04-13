using Core.Effects;
using Core.Player;
using EventSystems;
using Interfaces;
using UnityEngine;

namespace Core.Interactables
{
    public class InteractableGeyzer : AInteractableItem
    {
        public override EInteractableType InteractableType => EInteractableType.eit_damageZone;

        [SerializeField]
        private ExplosionPhysicsForce _explosion;
        [SerializeField]
        private float _timeOfExplosion = 5f;
        [SerializeField]
        private float _timeOfColliderOn = 2f;
        [SerializeField]
        private float _timeOfColliderOff = 8f;
        [SerializeField]
        private int _explosionValue = 2;

        private float _internalTimer = 0f;
        private bool _isAlive = false;
        private Collider _collider;
        
        public override void Action(PlayerSessionData sessionData) 
        {
            EventManager.RaiseEvent<IGameEvent>(handler => handler.ProcessEvent(InteractableType, -1));
        }

        public override void OnStart()
        {
            base.OnStart();
            _internalTimer = 0f;
            _collider = GetComponent<Collider>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _internalTimer += Time.deltaTime;
            if (_collider != null)
            {
                _collider.enabled = _internalTimer > _timeOfColliderOn && _internalTimer < _timeOfColliderOff;
            }


            if (_internalTimer >= _timeOfExplosion && !_isAlive)
            {
                _isAlive = true;
                if (_explosion.ApplyExplosion())
                {
                    EventManager.RaiseEvent<IGameEvent>(handler => handler.ProcessEvent(InteractableType, -_explosionValue));
                }
            }
        }

    }
}
