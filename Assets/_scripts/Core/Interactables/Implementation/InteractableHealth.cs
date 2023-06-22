using Core.Player;
using EventSystems;
using Interfaces;
using UnityEngine;

namespace Core.Interactables
{
    public class InteractableHealth : ATakeable
    {
        public override EInteractableType InteractableType => EInteractableType.eit_health;

        public override ETakeableType TakeableType => ETakeableType.ett_health;

        public override void Action(PlayerSessionData sessionData)
        {
            EventManager.RaiseEvent<IGameEffectEvent>(handler => handler.AddEffect(TakeEffectID, transform.position, sessionData.PlayerPosition, out go, () => { EventManager.RaiseEvent<IGameEvent>(handler => handler.ProcessEvent(InteractableType, 1)); }));
            gameObject.SetActive(false);
        }
    }
}
