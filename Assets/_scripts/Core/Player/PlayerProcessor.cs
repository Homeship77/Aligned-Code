using Core.ObjectPool;
using EventSystems;
using Interfaces;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Core.Player
{
    public class PlayerProcessor : MonoBehaviour, IGameEvent
    {
        [SerializeField]
        private int _startHealth = 5;
        [SerializeField]
        private int _maxTakeable = 35;
        [SerializeField]
        private int _maxTreates = 25;

        [SerializeField]
        private GameObjectStore _store;
        [SerializeField]
        private Transform _levelBase;

        [SerializeField]
        private LayerMask _interactableLayerMask;

        private PlayerSessionData _sessionData;
        private PoolService _effectService;
        private LevelGenerator _levelProcessor;
        private float _coolDownTimer = 0f;
        private const float DAMAGE_COOLDOWN = 1.75f;
        private CapsuleCollider _capsuleCollider;

        // Start is called before the first frame update
        private void Start()
        {
            _sessionData = new PlayerSessionData(_startHealth);
            _effectService = new PoolService(_store, null);
            Vector2 levelSize = new Vector2(_levelBase.localScale.x * 5, _levelBase.localScale.z * 5) * 0.9f;
            _levelProcessor = new LevelGenerator(_levelBase, _maxTakeable, _maxTreates, levelSize, _store);
            _capsuleCollider = GetComponent<CapsuleCollider>();
            EventManager.Subscribe(this);
        }

        // Update is called once per frame
        private void Update()
        {
            _sessionData.UpdatePosition(transform.position);
            _effectService.OnUpdate();
            _levelProcessor.OnUpdate(Time.deltaTime, _sessionData.PlayerPosition);
        }

        private void FixedUpdate()
        {
            ProcessCollisions();
            _coolDownTimer -= Time.fixedDeltaTime;
        }

        private List<Collider> CheckCollisionsInSphere(out int count)
        {
            Collider[] results = new Collider[10];
            count = Physics.OverlapSphereNonAlloc(transform.position, _capsuleCollider.height * 0.5f, results, _interactableLayerMask);
            return new List<Collider>(results);
        }

        private void ProcessCollisions()
        {
            List<Collider> results = CheckCollisionsInSphere(out int counter);
            for (int i = 0; i< counter; i++)
            {
                var collision = results[i];
                ActivateInteractable(collision.gameObject);
            }
        }

        private void ActivateInteractable(GameObject item)
        {
            if (item == null)
                return;
            var itemComponent = item.GetComponent<IGameInteractable>();
            if (itemComponent == null)
                return;
            
            itemComponent.Action(_sessionData);
        }

        public void ProcessEvent(EInteractableType eventType, int value)
        {
            switch(eventType)
            {
                case EInteractableType.eit_damageZone:
                    if (_coolDownTimer < 0f)
                    {
                        _sessionData.ChangeHealthValue(value);
                        EventManager.RaiseEvent<IUIEvent>(handler => handler.ProcessUIEvent(eventType, _sessionData.Health));
                        _coolDownTimer = DAMAGE_COOLDOWN;
                    }
                     break;
                case EInteractableType.eit_base:
                    _sessionData.ChangeCoinsValue(value);
                    EventManager.RaiseEvent<IUIEvent>(handler => handler.ProcessUIEvent(eventType, _sessionData.CoinsSpended));
                    EventManager.RaiseEvent<IUIEvent>(handler => handler.ProcessUIEvent(EInteractableType.eit_coin, _sessionData.Coins));
                    break;
                case EInteractableType.eit_health:
                    _sessionData.ChangeHealthValue();
                    EventManager.RaiseEvent<IUIEvent>(handler => handler.ProcessUIEvent(eventType, _sessionData.Health));
                    break;
                case EInteractableType.eit_coin:
                    _sessionData.ChangeCoinsValue(value);
                    EventManager.RaiseEvent<IUIEvent>(handler => handler.ProcessUIEvent(eventType, _sessionData.Coins));
                    break;
                case EInteractableType.eit_upgrade:
                    if (value < 0)
                    {
                        _sessionData.AddLowUpgrade(-value);
                    }
                    else
                    {
                        _sessionData.AddHighUpgrade(value);
                    }
                    break;
                case EInteractableType.eit_none:
                    break;
            }
        }

        public void CriticalEvent(EEventType eventType)
        {
            switch (eventType)
            {
                case EEventType.eet_death:
                    EventManager.RaiseEvent<IUIEvent>(handler => handler.CriticalUIEvent(EEventType.eet_death));
                    EventManager.RaiseEvent<IUIEvent>(handler => handler.ProcessUIEvent(EInteractableType.eit_health, _sessionData.Health));
                    this.enabled = false;
                    break;
                case EEventType.eet_respawn:
                    _sessionData.SetHealthValue(_startHealth);
                    EventManager.RaiseEvent<IUIEvent>(handler => handler.CriticalUIEvent(EEventType.eet_respawn));
                    EventManager.RaiseEvent<IUIEvent>(handler => handler.ProcessUIEvent(EInteractableType.eit_health, _sessionData.Health));
                    this.enabled = true;
                    break;
                
            }
        }

        public void ObjectReturningToPool(Vector3 objPosition)
        {
            //
        }
    } 
}
