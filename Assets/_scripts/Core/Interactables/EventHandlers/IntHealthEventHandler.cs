using Core.Attributes;
using Core.Player;
using EventSystems;
using Interfaces;

namespace Core.Interactables
{
    [InteractableHandler(EInteractableType.eit_health)]
    public class IntHealthEventHandler : AInteractableEventHandler
    {
        public IntHealthEventHandler(PlayerSessionData plData) : base(plData) { }
        public override void ProcessInteractableEvent(int value)
        {
            _plData.ChangeHealthValue();
            EventManager.RaiseEvent<IUIEvent>(handler => handler.ProcessUIEvent(EInteractableType.eit_health, _plData.Health));
        }
    }
}
