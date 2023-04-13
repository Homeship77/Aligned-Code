using EventSystems;
using Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.ObjectPool
{
    [Serializable]
    public struct SActiveObject
    {
        public GameObject Item;
        public string ID;

        public SActiveObject(GameObject item, string id)
        {
            Item = item;
            ID = id;
        }
    }
    public class PoolService : IGameEffectEvent
    {
        public event Action<float> OnUpdateEvent;

        private Dictionary<string, List<GameObject>> _pool;
        private List<SActiveObject> _activeObjects;
        private Transform _parent;

        public void AddEffect(string effectID, Vector3 startPos, Vector3 endPos, Action callback = null)
        {
            if (_pool.TryGetValue(effectID, out var value))
            {
                if (value.Count > 0) 
                {
                    _activeObjects.Add(new SActiveObject(value[0], effectID));
                    var go = value[0];
                    value.RemoveAt(0);

                    if (_parent != null)
                    {
                        go.transform.SetParent(_parent);
                    }
                    go.SetActive(true);
                    go.transform.position = startPos;
                    var effectCtrl = go.GetComponent<EffectController>();
                    if (effectCtrl != null)
                    {
                        effectCtrl.SetTargetPosition(endPos);
                        if (callback != null)
                        {
                            effectCtrl.SetCallback(callback);
                        }
                    }
                    return;
                }
                else // not enough objects in pool
                {

                }
            }
            else
            {
                if (callback != null)
                {
                    callback.Invoke();
                }
            }
            //throw new Exception("There is no " + effectID + " object in the pool");
        }

        public PoolService(GameObjectStore assetStore, Transform parent)
        {
            _parent = parent;
            _pool = new Dictionary<string, List<GameObject>>();
            _activeObjects = new List<SActiveObject>();
            foreach (var item in assetStore.ItemListData)
            {
                InitObjectTypeInPool(item);
            }
            EventManager.Subscribe(this);
        }

        public void OnUpdate()
        {
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            int counter = 0;
            while(_activeObjects.Count > 0 && counter < _activeObjects.Count) 
            { 
                if (!_activeObjects[counter].Item.activeSelf)
                {
                    EventManager.RaiseEvent<IGameEvent>(handler => handler.ObjectReturningToPool(_activeObjects[counter].Item.transform.position));
                    _pool[_activeObjects[counter].ID].Add(_activeObjects[counter].Item);
                    _activeObjects.RemoveAt(counter);
                }
                else
                {
                    counter++;
                }
            }
        }

        private void InitObjectTypeInPool(GameObjectItem item) 
        {
            if (_pool.ContainsKey(item.ID))
                return;
            List<GameObject> newSubPool = new List<GameObject>();
            for(int i = 0; i < item.PoolCount; i++) 
            {
                var go = GameObject.Instantiate(item.Prefab);
                go.SetActive(false);
                newSubPool.Add(go);
            }
            _pool[item.ID] = newSubPool;
        }



    }
}
