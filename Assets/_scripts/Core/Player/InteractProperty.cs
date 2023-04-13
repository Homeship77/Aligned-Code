using Core.ObjectPool;
using Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Player
{
    public class InteractProperty : MonoBehaviour
    {
        [SerializeField]
        private int _startHealth = 3;

        [SerializeField]
        private GameObjectStore _store;

        private List<Collider> _collisions = new List<Collider>();

        private PlayerSessionData _sessionData;
        private PoolService _effectService;

        // Start is called before the first frame update
        void Start()
        {
            _sessionData = new PlayerSessionData(_startHealth);
            _effectService = new PoolService(_store);
        }

        // Update is called once per frame
        void Update()
        {
            _sessionData.UpdatePosition(transform.position);
            _effectService.OnUpdate();
        }

        private void OnCollisionEnter(Collision collision)
        {
            ContactPoint[] contactPoints = collision.contacts;
            for (int i = 0; i < contactPoints.Length; i++)
            {
                if (!_collisions.Contains(collision.collider))
                {
                    _collisions.Add(collision.collider);
                }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            //if ()
            {
                if (!_collisions.Contains(collision.collider))
                {
                    _collisions.Add(collision.collider);
                }
            }
           // else
            {
                if (_collisions.Contains(collision.collider))
                {
                    _collisions.Remove(collision.collider);
                }
                if (_collisions.Count == 0) { }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (_collisions.Contains(collision.collider))
            {
                _collisions.Remove(collision.collider);
            }
            if (_collisions.Count == 0) { }
        }

        private void ActivateInteractable(GameObject item)
        {
            if (item == null)
                return;
            var itemComponent = item.GetComponent<IGameInteractable>();
            if (itemComponent != null)
                return;
            
            itemComponent.Action(_sessionData);
            //EventManager.RaiseEvent<IGameInteractable>(handler => handler.Action(_sessionData));
        }

    } 
}
