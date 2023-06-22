using Core.Attributes;
using Core.Player;
using EventSystems;
using Interfaces;

namespace Core.Interactables
{
    [InteractableHandler(EInteractableType.eit_mana)]
    public class IntManaEventHandler : AInteractableEventHandler
    {
        public IntManaEventHandler(PlayerSessionData plData) : base(plData) { }
        public override void ProcessInteractableEvent(int value)
        {
            _plData.ChangeManaValue();
            EventManager.RaiseEvent<IUIEvent>(handler => handler.ProcessUIEvent(EInteractableType.eit_mana, _plData.Mana));
        }
    }
}
