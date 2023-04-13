using Core.Player;
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

        public abstract void OnStart();

        public abstract void Action(PlayerSessionData sessionData);
        
        public abstract void OnUpdate();
    }
}