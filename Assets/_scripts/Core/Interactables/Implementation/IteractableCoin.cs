using Core.Player;
using EventSystems;
using Interfaces;
using System;

namespace Core.Interactables
{
    public class IteractableCoin : ATakeable
    {
        public override EInteractableType InteractableType => EInteractableType.eit_coin;

        public override ETakeableType TakeableType => ETakeableType.ett_coin;

        public override void Action(PlayerSessionData _sessionData)
        {
            EventManager.RaiseEvent<IGameEvent>(handler => handler.AddEffect( TakeEffectID, transform.position, _sessionData.PlayerPosition, ()=> { _sessionData.ChangeCoinsValue(); }));
            
        }

        public override void OnDestroying()
        {
            throw new NotImplementedException();
        }

        public override void OnStart()
        {
            throw new NotImplementedException();
        }

        public override void OnUpdate()
        {
            throw new NotImplementedException();
        }
    }
}
