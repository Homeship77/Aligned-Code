using Core.Enemies;
using Core.ObjectPool;
using Core.SpellSystem;
using EventSystems;
using Interfaces;
using System;
using System.Collections.Generic;
using IntEventHandlersFactory = Core.Factory<Core.Interactables.AInteractableEventHandler, Core.Player.PlayerSessionData>;
using UnityEngine;
using Core.Interactables;
using Core.Attributes;

namespace Core.Player
{
    public class PlayerProcessor : MonoBehaviour, IGameEvent
    {
        [SerializeField]
        private GameObjectStore _store;
        [SerializeField]
        private GameSettings _settings;
        [SerializeField]
        private Transform _levelBase;

        [SerializeField]
        private LayerMask _interactableLayerMask;

        [SerializeField]
        private Transform _rightHand;
        [SerializeField]
        private Transform _leftHand;
        [SerializeField]
        private CapsuleCollider _capsuleCollider;
        [SerializeField]
        private SimpleSampleCharacterControl _charController;

        private PlayerSessionData _sessionData;
        private PoolService _effectService;
        private LevelGenerator _levelProcessor;

        //private float _coolDownTimer = 0f;
        //private const float DAMAGE_COOLDOWN = 1.75f;

        private Action<Vector3, Vector3> _spellCallback;
        private Vector3 _currentTarget;

        private Dictionary<EInteractableType, AInteractableEventHandler> _interactableHandlers = new Dictionary<EInteractableType, AInteractableEventHandler>();

        public event Action<float> OnFixedUpdate;

        // Start is called before the first frame update
        private void Start()
        {
            _sessionData = new PlayerSessionData(_settings.StartHealth, _settings.StartMana);
            _effectService = new PoolService(_store, null);
            Vector2 levelSize = new Vector2(_levelBase.localScale.x * 5, _levelBase.localScale.z * 5) * 0.9f;
            _levelProcessor = new LevelGenerator(_levelBase, _settings.MaxTakeable, _settings.MaxTreates, levelSize, _store);

            if (_capsuleCollider == null)
                _capsuleCollider = GetComponent<CapsuleCollider>();
            if (_charController == null)
                _charController = GetComponent<SimpleSampleCharacterControl>();

            InitInteracbaleHandlers();

            EventManager.Subscribe(this);
        }

        private void InitInteracbaleHandlers() 
        {
            var baseSpellVar = typeof(AInteractableEventHandler);
            Type[] types = baseSpellVar.Assembly.GetTypes();

            foreach (Type t in types)
            {
                if (t.IsAbstract)
                    continue;

                if (!baseSpellVar.IsAssignableFrom(t))
                    continue;

                var attr = t.GetCustomAttributes(typeof(InteractableHandlerAttribute), false);
                if (attr == null || attr.Length == 0)
                {
                    Debug.Log("Interactable event handler without InteractableHandlerAttribute: " + t);
                    continue;
                }

                var intID = ((InteractableHandlerAttribute)attr[0]).InteractableID;
                if (_interactableHandlers.ContainsKey(intID))
                    throw new Exception("Interactable event handler " + intID + " already added ");

                _interactableHandlers.Add(intID, new IntEventHandlersFactory(t).Build(_sessionData));
            }
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
            if (OnFixedUpdate!= null)
                OnFixedUpdate(Time.fixedDeltaTime);
            //_coolDownTimer -= Time.fixedDeltaTime;
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
            if (_interactableHandlers.TryGetValue(eventType, out AInteractableEventHandler _handler))
            {
                _handler.ProcessInteractableEvent(value);
            }
            else
            {
                throw new Exception("Interactable event handler " + eventType + " not found");
            }

            //switch(eventType)
            //{
            //    case EInteractableType.eit_damageZone:
            //        if (_coolDownTimer < 0f)
            //        {
            //            _sessionData.ChangeHealthValue(value);
            //            EventManager.RaiseEvent<IUIEvent>(handler => handler.ProcessUIEvent(eventType, _sessionData.Health));
            //            _coolDownTimer = DAMAGE_COOLDOWN;
            //        }
            //         break;
            //    case EInteractableType.eit_base:
            //        _sessionData.ChangeCoinsValue(value);
            //        EventManager.RaiseEvent<IUIEvent>(handler => handler.ProcessUIEvent(eventType, _sessionData.CoinsSpended));
            //        EventManager.RaiseEvent<IUIEvent>(handler => handler.ProcessUIEvent(EInteractableType.eit_coin, _sessionData.Coins));
            //        break;
            //    case EInteractableType.eit_health:
            //        _sessionData.ChangeHealthValue();
            //        EventManager.RaiseEvent<IUIEvent>(handler => handler.ProcessUIEvent(eventType, _sessionData.Health));
            //        break;
            //    case EInteractableType.eit_coin:
            //        _sessionData.ChangeCoinsValue(value);
            //        EventManager.RaiseEvent<IUIEvent>(handler => handler.ProcessUIEvent(eventType, _sessionData.Coins));
            //        break;
            //    case EInteractableType.eit_upgrade:
            //        if (value < 0)
            //        {
            //            _sessionData.AddLowUpgrade(-value);
            //        }
            //        else
            //        {
            //            _sessionData.AddHighUpgrade(value);
            //        }
            //        break;
            //    case EInteractableType.eit_none:
            //        break;
            //}
        }

        public void CriticalEvent(EEventType eventType)
        {
            switch (eventType)
            {
                case EEventType.eet_death:
                    EventManager.RaiseEvent<IUIEvent>(handler => handler.CriticalUIEvent(EEventType.eet_death));
                    EventManager.RaiseEvent<IUIEvent>(handler => handler.ProcessUIEvent(EInteractableType.eit_health, _sessionData.Health));
                    this.enabled = false;
                    _charController.enabled = false;
                    break;
                case EEventType.eet_respawn:
                    _sessionData.SetHealthValue(_settings.StartHealth);
                    EventManager.RaiseEvent<IUIEvent>(handler => handler.CriticalUIEvent(EEventType.eet_respawn));
                    EventManager.RaiseEvent<IUIEvent>(handler => handler.ProcessUIEvent(EInteractableType.eit_health, _sessionData.Health));
                    this.enabled = true;
                    _charController.enabled = true;
                    break;
                
            }
        }

        public void ObjectReturningToPool(Vector3 objPosition)
        {
            //
        }

        public void CriticalEnemyEvent(EEnemyEventType eventType, EnemyView enemy)
        {
            //
        }

        public void StartSpellEvent(SpellNode data, Vector3 target, Action<Vector3, Vector3> callback)
        {
            _spellCallback = callback;
            _currentTarget = target;

            switch (data.Type)
            {
                case ESpellType.espt_attack:
                    _charController.CharAnimator.SetTrigger("Attack");
                    break;
                case ESpellType.espt_shield:
                    _charController.CharAnimator.SetTrigger("Shield");
                    break;
                case ESpellType.espt_buff:
                    _charController.CharAnimator.SetTrigger("Buff");
                    break;
                case ESpellType.espt_debuff:
                    _charController.CharAnimator.SetTrigger("Debuff");
                    break;
            }            
        }

        public void SpellShooting()
        {
            if (_spellCallback == null || _currentTarget.Equals(Vector3.zero))
                return;
            Vector3 pos = _rightHand.transform.position;
            _spellCallback(pos, _currentTarget);

            _spellCallback = null;
            _currentTarget = Vector3.zero;
        }
    } 
}
