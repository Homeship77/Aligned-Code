using Core.Player;
using EventSystems;
using Interfaces;
using UnityEngine;

namespace Core
{
    public abstract class AInteractableItem : MonoBehaviour, IGameInteractable
    {
        public abstract EInteractableType InteractableType { get; }

        void Start()
        {
            OnStart();
        }

        void Update()
        {
            OnUpdate();
        }

        void OnEnable()
        {
            OnStart();
        }

        void OnDisable() 
        { 
            OnDestroying(); 
        }

        private void OnDestroy()
        {
            OnDestroying();
        }

        public virtual void OnStart() { EventManager.Subscribe(this); }

        public virtual void OnDestroying() { EventManager.Unsubscribe(this); }

        public virtual void Action(PlayerSessionData sessionData) { }

        public virtual void OnUpdate() { }
    }
}