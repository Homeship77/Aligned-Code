using Core.Enemies;
using Core.SpellSystem;
using EventSystems;
using Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.ObjectPool
{
    public class LevelGenerator: IGameEvent
    {
        private Transform _levelParent;
        private Vector3 _playerPos;
        private float _lastSpawnTimer = 0f;

        private List<Vector3> _spawnedTreatable;
        private List<Vector3> _spawnedTakeable;

        private List<Vector3> _spawnedUniquePos = new List<Vector3>();
        private Vector2 _spawnedZoneSize = Vector2.zero;
        private int _maxTreatable;
        private int _maxTakeable;
        private string[] _uniqueIDs;
        private GameObjectStore _store;

        private ItemWeightedRandomSelector _takeableSelector;
        private ItemWeightedRandomSelector _treatableSelector;

        private const float FREE_ZONE = 3f;
        private const float SPAWN_PERIOD = 5f;
        private const int SPAWN_ATTEMPS = 10;

        private bool _isActive = true;

        public LevelGenerator(Transform levelParent, int maxTakeable, int maxTreatable, Vector2 spawnedZoneSize, GameObjectStore store)
        {
            EventManager.Subscribe(this);
            _levelParent = levelParent;
            _spawnedZoneSize = spawnedZoneSize;
            _maxTakeable = maxTakeable;
            _maxTreatable = maxTreatable;
            _store = store;
            
            _takeableSelector = new ItemWeightedRandomSelector(store, EItemFilterType.eift_takeable);
            _treatableSelector = new ItemWeightedRandomSelector(store, EItemFilterType.eift_treatable);

            _uniqueIDs = store.GetFilteredItemsIDs(EItemFilterType.eift_unique);

            LevelInit();
        }

        private void LevelInit() 
        {
            _spawnedUniquePos = new List<Vector3>();
            _spawnedTakeable = new List<Vector3>();
            _spawnedTreatable = new List<Vector3>();
            foreach (var id in _uniqueIDs)
            {
                var pos = SpawnObjectRandomly(id);
                if (!pos.Equals(Vector3.zero))
                    _spawnedUniquePos.Add(pos);
            }

            for(int i = 0; i < _maxTakeable; i++)
            {
                var name = _takeableSelector.GetRandomItem().ID;
                var pos = SpawnObjectRandomly(name);
                if (!pos.Equals(Vector3.zero))
                    _spawnedTakeable.Add(pos);
            }

            for (int i = 0; i < _maxTreatable; i++)
            {
                var name = _treatableSelector.GetRandomItem().ID;
                var pos = SpawnObjectRandomly(name);
                if (!pos.Equals(Vector3.zero))
                    _spawnedTreatable.Add(pos);
            }
            _isActive = true;
        }

        private bool CheckPosition(Vector3 pos)
        {
            var sqrDistance = FREE_ZONE * FREE_ZONE;

            if ((pos - _playerPos).sqrMagnitude < sqrDistance)
                return false;
            foreach (var obj in _spawnedUniquePos)
            {
                if ((pos - obj).sqrMagnitude < sqrDistance)
                {
                    return false;
                }
            }
            foreach (var obj in _spawnedTreatable)
            {
                if ((pos - obj).sqrMagnitude < sqrDistance)
                {
                    return false;
                }
            }
            foreach (var obj in _spawnedTakeable)
            {
                if ((pos - obj).sqrMagnitude < sqrDistance)
                {
                    return false;
                }
            }
            return true;
        }

        private GameObject go;

        private Vector3 SpawnObjectRandomly(string TakeEffectID)
        {
            Vector3 newPos = _playerPos;
            int counter = 0;
            bool checkedPos = CheckPosition(newPos);
            while (!checkedPos && SPAWN_ATTEMPS > counter)
            {
                newPos = new Vector3(UnityEngine.Random.Range(-_spawnedZoneSize.x, _spawnedZoneSize.x), _levelParent.position.y, UnityEngine.Random.Range(-_spawnedZoneSize.y, _spawnedZoneSize.y));
                checkedPos = CheckPosition(newPos);
                counter++;  
            }
            if (SPAWN_ATTEMPS > counter)
            {
                EventManager.RaiseEvent<IGameEffectEvent>(handler => handler.AddEffect(TakeEffectID, newPos, _playerPos, out go, null));
                return newPos;
            }
            return Vector3.zero;
        }

        public void OnUpdate(float deltaTime, Vector3 playerPosition)
        {
            if (!_isActive) 
            {
                return;
            }

            _playerPos = playerPosition;
            _lastSpawnTimer += deltaTime;
            if (_lastSpawnTimer >=SPAWN_PERIOD)
            {
                _lastSpawnTimer = 0f;
                for (int i = 0; i < (_maxTreatable - _spawnedTreatable.Count) / 2; i++ )
                {
                    var name = _treatableSelector.GetRandomItem().ID;
                    var pos = SpawnObjectRandomly(name);
                    if (!pos.Equals(Vector3.zero))
                        _spawnedTreatable.Add(pos);
                }
            }
        }

        public void ProcessEvent(EInteractableType eventType, int value)
        {
            switch (eventType)
            {
                case EInteractableType.eit_damageZone:
                    break;
                case EInteractableType.eit_base:
                    break;

                case EInteractableType.eit_health:
                    var name = _takeableSelector.GetRandomItem().ID;
                    var pos = SpawnObjectRandomly(name);
                    if (!pos.Equals(Vector3.zero))
                        _spawnedTakeable.Add(pos);
                    break;
                case EInteractableType.eit_coin:
                    var ids = _takeableSelector.GetRandomItem().ID;
                    var pos1 = SpawnObjectRandomly(ids);
                    if (!pos1.Equals(Vector3.zero))
                        _spawnedTakeable.Add(pos1);
                    break;
                case EInteractableType.eit_mana:
                    var ids1 = _takeableSelector.GetRandomItem().ID;
                    var pos2 = SpawnObjectRandomly(ids1);
                    if (!pos2.Equals(Vector3.zero))
                        _spawnedTakeable.Add(pos2);
                    break;

                case EInteractableType.eit_upgrade:
                   //some feature spawned
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
                    _isActive = false;
                    break;
                case EEventType.eet_respawn:
                    _isActive = true;
                    break;

            }
        }

        public void ObjectReturningToPool(Vector3 objPosition)
        {
            int counter = 0;
            while (_spawnedUniquePos.Count > 0 && counter < _spawnedUniquePos.Count)
            {
                if (!_spawnedUniquePos[counter].Equals(objPosition))
                    counter++;
                else
                {
                    _spawnedUniquePos.RemoveAt(counter);
                    return;
                }
            }
            counter = 0;
            while (_spawnedTreatable.Count > 0 && counter < _spawnedTreatable.Count)
            {
                if (!_spawnedTreatable[counter].Equals(objPosition))
                    counter++;
                else
                {
                    _spawnedTreatable.RemoveAt(counter);
                    return;
                }
            }
            counter = 0;
            while (_spawnedTakeable.Count > 0 && counter < _spawnedTakeable.Count)
            {
                if (!_spawnedTakeable[counter].Equals(objPosition))
                    counter++;
                else
                {
                    _spawnedTakeable.RemoveAt(counter);
                    return;
                }
            }
        }

        public void CriticalEnemyEvent(EEnemyEventType eventType, EnemyView enemy)
        {
            //
        }

        public void StartSpellEvent(SpellNode data, Vector3 target, Action<Vector3, Vector3> callback)
        {
            //
        }
    }
}
