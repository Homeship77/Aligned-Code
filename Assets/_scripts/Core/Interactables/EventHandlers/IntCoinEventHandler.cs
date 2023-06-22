using Core.Attributes;
using Core.Player;
using EventSystems;
using Interfaces;

namespace Core.Interactables
{
    [InteractableHandler(EInteractableType.eit_coin)]
    public class IntCoinEventHandler : AInteractableEventHandler
    {
        public IntCoinEventHandler(PlayerSessionData plData) : base(plData) { }
        public override void ProcessInteractableEvent(int value)
        {
            _plData.ChangeCoinsValue(value);
            EventManager.RaiseEvent<IUIEvent>(handler => handler.ProcessUIEvent(EInteractableType.eit_coin, _plData.Coins));
        }
    }
}
