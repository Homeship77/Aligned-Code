using Core.Player;
using EventSystems;
using Interfaces;

namespace Core.Interactables
{
    public class InteractableMana : ATakeable
    {
        public override EInteractableType InteractableType => EInteractableType.eit_mana;

        public override ETakeableType TakeableType => ETakeableType.ett_mana;

        public override void Action(PlayerSessionData sessionData)
        {
            EventManager.RaiseEvent<IGameEffectEvent>(handler => handler.AddEffect(TakeEffectID, transform.position, sessionData.PlayerPosition, out go, () => { EventManager.RaiseEvent<IGameEvent>(handler => handler.ProcessEvent(InteractableType, 1)); }));
            gameObject.SetActive(false);
        }
    }
}
