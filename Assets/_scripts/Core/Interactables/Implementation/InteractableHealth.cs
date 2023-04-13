using Core.Player;
using EventSystems;
using Interfaces;
using System;
namespace Core.Interactables
{
    public class InteractableHealth : ATakeable
    {
        public override EInteractableType InteractableType => EInteractableType.eit_health;

        public override ETakeableType TakeableType => ETakeableType.ett_health;

        public override void Action(PlayerSessionData _sessionData)
        {
            _sessionData.ChangeHealthValue();
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
